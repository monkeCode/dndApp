using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseLib;

namespace App1.Directories
{
    class MonsterModelView:DirectoriesModelView<Monster>
    {
        MonsterModelView()
        {
            DataCollection = new ObservableCollection<Monster>();
            GetListData();
            whereReq = new string[3];
        }
        public object SelectedType { set { whereReq[0] = value?.ToString(); GetListData(); } }
        public object SelectedRate { set { whereReq[1] = value?.ToString(); GetListData(); } }
        public object SelectedSize { set { whereReq[2] = value?.ToString(); GetListData(); } }
        public object SelectedHabitat { set { whereReq[3] = value?.ToString(); GetListData(); } }
        public object SelectedSource { set { whereReq[4] = value?.ToString(); GetListData(); } }
        public override void GetListData()
        {
            DataCollection.Clear();

            string s = whereReq?.GetAllElemets(" AND ");
            foreach (object[] i in DataAccess.GetData("Monsters", s, "Name", "*"))
            {
                if (!string.IsNullOrEmpty(SubstringFilter))
                    if (i[1].ToString().ToLower().IndexOf(SubstringFilter) == -1)
                        continue;
                DataCollection.Add(new Monster
                {
                    Id = (int) i[0],
                    Name = i[1].ToString(),
                    Size = (int) i[2],
                    Challenge = i[3].ToString(),
                    Type = i[4].ToString()

                });


            }
        }
    }
}
