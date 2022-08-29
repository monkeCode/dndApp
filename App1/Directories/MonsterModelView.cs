using Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App.Directories
{
    public class MonsterModelView : DirectoriesModelView<Monster>
    {
        public MonsterModelView()
        {
            DataCollection = new ObservableCollection<Monster>();
            sortPred = m => m.Name;
            GetListData();
        }
        private List<string> _selectedType = new();
        private List<string> _selectedRate = new();
        private List<int> _selectedSize = new();
        private List<string> _selectedHabitat = new();
        private List<string> _selectedSource = new();
        public IList<string> SelectedType { set { _selectedType = value.ToList(); GetListData(); } }
        public IList<string> SelectedRate { set { _selectedRate = value.ToList(); GetListData(); } }
        public IList<string> SelectedSize { set { _selectedSize = value.Select(i => StaticValues.monsterSize[i.ToString()]).ToList(); GetListData(); } }
        public IList<string> SelectedHabitat { set { _selectedHabitat = value.ToList(); GetListData(); } }
        public IList<string> SelectedSource { set { _selectedSource = value.ToList(); GetListData(); } }
        public override void GetListData()
        {
            DataCollection.Clear();
            var list = new List<Monster>();
            foreach (var monster in App.DataContext.GetMonsters().Where(m =>
            (_selectedRate.Contains(m.Challenge) || _selectedRate.Count == 0) &&
            (_selectedSize.Contains(m.Size) || _selectedSize.Count == 0) &&
            (_selectedType.Contains(m.Type) || _selectedType.Count == 0) &&
            (_selectedSource.Contains(m.Source) || _selectedSource.Count == 0) &&
            (_selectedHabitat.Intersect(m.Habitat).Count() > 0 || _selectedHabitat.Count == 0)))
            {
                if (!string.IsNullOrEmpty(SubstringFilter))
                    if (monster.Name.ToLower().IndexOf(SubstringFilter) == -1)
                        continue;
                list.Add(monster);
            }
            foreach (var m in Sort(list))
                DataCollection.Add(m);
        }
    }
}
