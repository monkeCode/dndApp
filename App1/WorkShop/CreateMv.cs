using App;
using App.Annotations;
using System.ComponentModel;
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
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public abstract void Save();
    }
}
