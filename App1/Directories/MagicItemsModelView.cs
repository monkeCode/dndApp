﻿using DataBaseLib;
using System.Collections.ObjectModel;

namespace App1
{
    public class MagicItemsModelView : DirectoriesModelView<MagicItem>
    {
        public MagicItemsModelView()
        {
            DataCollection = new ObservableCollection<MagicItem>();
            GetListData();
            whereReq = new string[2];
        }

        public object SelectedType { set { whereReq[0] = value?.ToString(); GetListData(); } }
        public object SelectedQuality { set { whereReq[1] = value?.ToString(); GetListData(); } }

        public override void GetListData()
        {
            DataCollection.Clear();

            string s = null;
            s = whereReq?.GetAllElemets(" AND ");
            foreach (object[] i in DataAccess.GetData("MagicItems", s, "*"))
            {
                if (!string.IsNullOrEmpty(SubstringFilter))
                    if (i[1].ToString().ToLower().IndexOf(SubstringFilter) == -1)
                        continue;
                DataCollection.Add(new MagicItem(int.Parse(i[0].ToString()), i[1].ToString(), (MagicItem.ItemQuality)(long)i[2], i[3].ToString(), i[4].ToString() != "0"));
            }
        }
    }
}