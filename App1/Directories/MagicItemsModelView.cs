using DataBaseLib;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App1
{
    public class MagicItemsModelView : DirectoriesModelView<MagicItem>
    {
        public MagicItemsModelView()
        {
            DataCollection = new ObservableCollection<MagicItem>();
            GetListData();
            whereReq = new string[3];
        }

        public IList<object> SelectedType { set { whereReq[0] = value.Count > 0 ? "Type IN ( " + ChangeSelect(value) : null; GetListData(); } }
        public IList<object> SelectedQuality { set { whereReq[1] = value.Count > 0 ? "Quality IN ( " + ChangeSelect(value.Select(i => (object)StaticValues.magicItemQality[i.ToString()]).ToList()) : null; GetListData(); } }
        public IList<object> SelectedSource { set { whereReq[2] = value.Count > 0 ? "Source IN ( " + ChangeSelect(value) : null; GetListData(); } }
        public ICollection<MagicItem> Sort(ICollection<MagicItem> list)
        {
           return list.OrderBy(i => i.Name).ToList();
        }
        public override void GetListData()
        {
            DataCollection.Clear();
            List<MagicItem> list = new List<MagicItem>();
            string s = whereReq?.GetAllElemets(" AND ");
            foreach (var i in DataAccess.GetData("MagicItems", s, "Name", "*"))
            {
                if (!string.IsNullOrEmpty(SubstringFilter))
                    if (i[1].ToString().ToLower().IndexOf(SubstringFilter) == -1)
                        continue;
                list.Add(new MagicItem(int.Parse(i[0].ToString()), i[1].ToString(), (int)(long)i[2], i[3].ToString(), i[4].ToString() != "0"));
            }
            foreach(var item in Sort(list))
                DataCollection.Add(item);
        }
    }
}