using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace App
{
    public abstract class DirectoriesModelView<T> : INotifyPropertyChanged
    {
        public ObservableCollection<T> DataCollection { get; set; }
        protected string substringFilter = "";
        public string SubstringFilter { get { return substringFilter; } set { substringFilter = value.Trim().ToLower(); OnPropertyChanged(); } }
        public void Search(object s)
        {
            substringFilter = ((TextBox)s).Text.ToLower();
            GetListData();
        }

        protected Func<T, object> sortPred;
        protected bool isDesc;
        protected CustomCommand sortCommand;

        public CustomCommand SortCommand
        {
            get
            {
                return sortCommand ??= new CustomCommand(obj =>
                {
                    if (sortPred == obj) isDesc = !isDesc;
                    else
                    {
                        sortPred = (Func<T, object>)obj;
                        isDesc = false;
                    }

                    GetListData();
                });
            }
        }
        protected List<T> Sort(List<T> list)
        {
            if (sortPred == null)
                return list;
            if (isDesc)
            {
                return list.OrderByDescending(sortPred).ToList();
            }
            return list.OrderBy(sortPred).ToList();
        }
        public abstract void GetListData();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public virtual void DropFilters()
        {
            SubstringFilter = string.Empty;
            GetListData();
        }
    }
}