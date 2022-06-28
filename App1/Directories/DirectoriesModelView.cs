using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

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
            substringFilter = ((TextBox)s).Text.ToLower();
            GetListData();
        }

        protected string ChangeSelect(IList<object> list)
        {
            if (list.Count == 0)
            {
                return null;
            }

            string str = "";
            foreach (var a in list)
            {
                if (a != list.Last())
                    str += $" \"{a}\", ";
                else str += $"\"{a}\" )";
            }

            return str;
        }

        protected Func<T, string> sortPred;
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
                        sortPred = (Func<T, string>)obj;
                        isDesc = false;
                    }

                    GetListData();
                });
            }
        }
        protected List<T> Sort(List<T> list)
        {
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
            for (int i = 0; i < whereReq.Length; i++)
            {
                whereReq[i] = string.Empty;
            }

            SubstringFilter = string.Empty;
            GetListData();
        }
    }
}