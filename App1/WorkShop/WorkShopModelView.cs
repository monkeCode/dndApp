using DataBaseLib;
using Model;
using System.Collections.ObjectModel;
using System.Drawing;
using Windows.UI.Xaml.Media;

namespace App.WorkShop
{
    class CompletedDataItem : DataItem
    {
        public Brush Color { get; set; }
    }
    internal class WorkShopModelView
    {
        public ObservableCollection<CompletedDataItem> MagicItems { get; set; }

        public WorkShopModelView()
        {
            MagicItems = new ObservableCollection<CompletedDataItem>();
            foreach (var item in DataAccess.GetData("select _id,Name," +
                                                    "(case when (select Description " +
                                                    "from ExtendedMagicItems " +
                                                    "where MagicItems._id = ExtendedMagicItems._id) IS NOT NULL " +
                                                    "then 1 else 0 end) as State from MagicItems"))
            {
                MagicItems.Add(new CompletedDataItem()
                {
                    Id = (int)(long)item[0],
                    Name = item[1].ToString(),
                    ItemType = DataItem.DataType.MagicItem,
                    Color = new SolidColorBrush((int)(long)item[2] == 1 ? Windows.UI.Color.FromArgb(Color.YellowGreen.A, Color.YellowGreen.R, Color.YellowGreen.G, Color.YellowGreen.B) : Windows.UI.Color.FromArgb(0, 0, 0, 0))
                });
            }


        }
    }
}
