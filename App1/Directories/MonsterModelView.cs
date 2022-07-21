using DataBaseLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App1.Directories
{
    public class MonsterModelView : DirectoriesModelView<Monster>
    {
        public MonsterModelView()
        {
            DataCollection = new ObservableCollection<Monster>();
            sortPred = m => m.Name;
            GetListData();
            whereReq = new string[5];
        }
        public IList<object> SelectedType { set { whereReq[0] = value.Count > 0 ? "Type IN ( " + ChangeSelect(value) : null; GetListData(); } }
        public IList<object> SelectedRate { set { whereReq[1] = value.Count > 0 ? "ChallengeRate IN ( " + ChangeSelect(value) : null; GetListData(); } }
        public IList<object> SelectedSize { set { whereReq[2] = value.Count > 0 ? "Size IN (" + ChangeSelect(value.Select(i => (object)StaticValues.monsterSize[i.ToString()]).ToList()) : null; GetListData(); } }
        public IList<object> SelectedHabitat { set { whereReq[3] = value.Count > 0 ? "Habitat IN ( " + ChangeSelect(value) : null; GetListData(); } }
        public IList<object> SelectedSource { set { whereReq[4] = value.Count > 0 ? "Source IN ( " + ChangeSelect(value) : null; GetListData(); } }
        public override void GetListData()
        {
            DataCollection.Clear();
            var list = new List<Monster>();
            string s = whereReq?.GetAllElemets(" AND ");
            foreach (object[] i in DataAccess.GetData("Monsters", s, "Name", "*"))
            {
                if (!string.IsNullOrEmpty(SubstringFilter))
                    if (i[1].ToString().ToLower().IndexOf(SubstringFilter) == -1)
                        continue;
                list.Add(new Monster
                {
                    Id = (int)(long)i[0],
                    Name = i[1].ToString(),
                    Size = (int)(long)i[2],
                    Challenge = i[5].ToString(),
                    Type = i[3].ToString()

                });
            }
                foreach (var m in Sort(list))
                    DataCollection.Add(m);
        }
    }
}
