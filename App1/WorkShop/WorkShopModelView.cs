using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Model;
using DataBaseLib;

namespace App.WorkShop
{
    internal class WorkShopModelView
    {
        public ObservableCollection<DataItem> MagicItems { get; set; }

        public WorkShopModelView()
        {
            MagicItems = new ObservableCollection<DataItem>();
            foreach (var item in DataAccess.GetData("SELECT _id, Name from MagicItems"))
            {
                MagicItems.Add(new DataItem()
                {
                    Id = (int)(long)item[0],
                    Name = item[1].ToString(),
                    ItemType = DataItem.DataType.MagicItem
                });
            }


        }
    }
}
