using Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Encounters
{
    public class BattleMonster : INotifyPropertyChanged
    {
        public Monster Monster { get; set; }
        private int quantity;
        public int Quantity { get => quantity; set { quantity = value > 99 ? 99 : value; OnPropertyChanged(); } }

        CustomCommand positiveCommand;
        public CustomCommand PositiveCommand
        {
            get
            {
                if (positiveCommand == null)
                {
                    positiveCommand = new CustomCommand(delegate (object obj) { Quantity++; }, delegate (object obj) { return true; });
                }
                return positiveCommand;
            }
        }
        CustomCommand negativeCommand;
        public CustomCommand NegativeCommand
        {
            get
            {
                if (negativeCommand == null)
                {
                    negativeCommand = new CustomCommand(delegate (object obj) { Quantity--; }, delegate (object obj) { return true; });
                }
                return negativeCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
