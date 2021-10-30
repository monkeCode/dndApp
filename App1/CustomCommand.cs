using System;
using System.Windows.Input;

namespace App1
{
    public class CustomCommand : ICommand
    {
        private Action<object> action;
        private Predicate<object> canExecut;

        public CustomCommand(Action<object> action, Predicate<object> canExecut)
        {
            this.action = action != null
                ? action
                : throw new ArgumentNullException();
            this.canExecut = canExecut;
        }

        public CustomCommand(Action<object> action)
            : this(action, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            //if (parameter == null) return true;
            var t = canExecut?.Invoke((object)parameter) ?? true;
            return t;
        }

        public void Execute(object parameter)
        {
            action((object)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}