using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace App1
{
    public abstract class DirectoriesModelView<T> : INotifyPropertyChanged
    {
        public ObservableCollection<T> DataCollection { get; set; }
        protected string[] whereReq = default;
        protected string substringFilter = "";
        public string SubstringFilter { get { return substringFilter; } set { substringFilter = value.Trim().ToLower(); OnPropertyChanged(); } }
        public void Search(object s)
        {
            substringFilter = ((TextBox)s).Text;
            GetListData();
        }

        // protected CustomCommand searchCommand;
        // public CustomCommand SearchCommand { get { return searchCommand??(searchCommand = new CustomCommand(obj => { GetListData(); })); } }

        public abstract void GetListData();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public virtual void DropFilters()
        {
            for (int i = 0; i < whereReq.Length; i++)
            {
                whereReq[i] = string.Empty;
            }

            SubstringFilter = string.Empty;
            GetListData();
        }
    }

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