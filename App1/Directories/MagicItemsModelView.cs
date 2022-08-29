using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App
{
    public class MagicItemsModelView : DirectoriesModelView<MagicItem>
    {
        public MagicItemsModelView()
        {
            DataCollection = new ObservableCollection<MagicItem>();
            sortPred = item => item.Name;
            GetListData();
        }

        private List<string> _selectedTypes = new();
        public IList<string> SelectedType { set { _selectedTypes = value.ToList(); GetListData(); } }

        private List<int> _selectedQuality = new();
        public IList<string> SelectedQuality { set { _selectedQuality = value.Select(i => StaticValues.magicItemQality[i]).ToList(); GetListData(); } }

        private List<string> _selectedSource = new();
        public IList<string> SelectedSource { set { _selectedSource = value.ToList(); GetListData(); } }
        public override void GetListData()
        {
            DataCollection.Clear();
            List<MagicItem> list = new List<MagicItem>();
            foreach (var i in App.DataContext.GetMagicItems().Where(it => (_selectedQuality.Contains(it.Quality) || _selectedQuality.Count == 0) &&
            (_selectedTypes.Contains(it.Type) || _selectedTypes.Count == 0) &&
            (_selectedSource.Contains(it.ItemSource) || _selectedSource.Count == 0)))
            {
                if (!string.IsNullOrEmpty(SubstringFilter))
                    if (!i.Name.ToLower().Contains(SubstringFilter))
                        continue;
                list.Add(i);
            }
            foreach (var item in Sort(list))
                DataCollection.Add(item);
        }
    }
}