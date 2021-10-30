using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System.Linq;

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