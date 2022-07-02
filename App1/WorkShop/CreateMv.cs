using App.Model;
using App1;
using App1.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace App.WorkShop
{
    public abstract class CreateMv<T> : INotifyPropertyChanged
    {
        private T _item;
        public T Item
        {
            get => _item;
            protected set { _item = value; OnPropertyChanged(); }
        }
        protected bool isNew { get; private set; }
        public CreateMv(bool isNew)
        {
            this.isNew = isNew;
        }
        private CustomCommand _featureCommand;

        public CustomCommand FeatureCommand
        {
            get
            {
                return _featureCommand ??= new CustomCommand(obj =>
                {
                    AddFeature();
                });
            }
        }
        public abstract void AddFeature();
        public abstract void AddLink(DataItem item);
        public ICollection<DataItem> GetItemsByName(string name)
        {
            List<DataItem> dataItems = new List<DataItem>();
            var request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from Monsters");
            foreach (var it in request)
            {
                dataItems.Add(new DataItem
                {
                    Id = (int)(long)it[0],
                    Name = it[1].ToString(),
                    ItemType = DataItem.DataType.Monster
                });
            }
            request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from MagicItems");
            foreach (var it in request)
            {
                dataItems.Add(new DataItem
                {
                    Id = (int)(long)it[0],
                    Name = it[1].ToString(),
                    ItemType = DataItem.DataType.MagicItem
                });
            }
            request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from Spells where Name ");
            foreach (var it in request)
            {
                dataItems.Add(new DataItem
                {
                    Id = (int)(long)it[0],
                    Name = it[1].ToString(),
                    ItemType = DataItem.DataType.Spell
                });
            }
            return dataItems.Where(it => it.Name.ToLower().Contains(name)).OrderByDescending(it => it.Name).ToList();
        }
        public abstract void DeleteLink(Link link);
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public abstract void Save();
    }
}
