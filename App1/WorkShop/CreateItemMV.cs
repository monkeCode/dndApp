using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using App1;
using App1.Annotations;

namespace App.WorkShop
{
    internal class CreateItemMV:INotifyPropertyChanged
    {
        private ExtendedMagicItem _item;
        public ExtendedMagicItem Item { get => _item;
            set { _item = value; OnPropertyChanged(); }
        }
        public bool IsAttunemended { get; set; }
        public CreateItemMV(int id)
        {
            Item = new ExtendedMagicItem(id);
        }

        public CreateItemMV()
        {
            Item = new ExtendedMagicItem();
            IsAttunemended = Item.Attunement != "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private CustomCommand _addFeature;

        public CustomCommand AddFeature
        {
            get
            {
                return _addFeature ??= new CustomCommand(obj =>
                {
                    Item.Features.Add(new Features());
                });
            }
        }
        private CustomCommand _addLink;

        public CustomCommand AddLink
        {
            get
            {
                return _addLink ??= new CustomCommand(obj =>
                {
                    Item.Links.Add(new Link("MA", 0, ""));
                });
            }
        }
        public void Save()
        {

        }
    }
}
