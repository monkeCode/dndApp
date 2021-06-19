using DataBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MagicPage : Page
    {
        public MagicPage()
        {
            this.InitializeComponent();
            GetListData();
        }
        void GetListData()
        {
            List<string[]> l = DataAccess.GetData("MagicItems", whereReq[0], "*");
            List<MagicItem> items = new List<MagicItem>();
            foreach (var i in l)
            {
                items.Add(new MagicItem(int.Parse(i[0]), i[1], (MagicItem.ItemQuality)int.Parse(i[2]), i[3]));
            }
            ItemsPanel.Items.Clear();
            foreach (var i in items)
            {
                ItemsPanel.Items.Add(i);
            }
        }
        string[] whereReq = new string[2];
        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedIndex == -1)
            {
                whereReq[0] = null;
            }
            else
            {

                string str = "";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $"Type = \"{((TextBlock)a).Text}\" OR ";
                    else str += $"Type = \"{((TextBlock)a).Text}\"";
                }
                whereReq[0] = str;
            }
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetListData();
        }
    }
}
