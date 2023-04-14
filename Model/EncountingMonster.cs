using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class EncountingMonster : INotifyPropertyChanged
    {
        public Monster Monster { get; set; }
        private int _quantity;
        public int Quantity { get => _quantity; set { _quantity = value > 99 ? 99 : value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
