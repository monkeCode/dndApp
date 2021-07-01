using DataBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<MagicItem> magicItems = new ObservableCollection<MagicItem>();
        public MagicPage()
        {
            this.InitializeComponent();
            GetListData();
        }
        void GetListData()
        {
            magicItems.Clear();
            string s = null;
            if (whereReq[0] != null && whereReq[1] != null)
                s = whereReq[0] + "AND" + whereReq[1];
            else if(whereReq[0] != null || whereReq[1] != null) s = whereReq[0] + whereReq[1];
            foreach (var i in DataAccess.GetData("MagicItems", s, "*"))
            {
                magicItems.Add(new MagicItem(int.Parse(i[0]), i[1], (MagicItem.ItemQuality)int.Parse(i[2]), i[3], i[4] != "0"));
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

                string str = "( ";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $"Type = \"{((TextBlock)a).Text}\" OR ";
                    else str += $"Type = \"{((TextBlock)a).Text}\" )";
                }
                whereReq[0] = str;
            }
        }

        private void ListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedIndex == -1)
            {
                whereReq[1] = null;
            }
            else
            {
                Dictionary<string, int> quality = new Dictionary<string, int>()
                {
                    {"Обычный", 0 },
                    {"Необычный", 1 },
                    {"Редкий", 2 },
                    {"Крайне редкий", 3 },
                    {"Легендарный", 4 },
                };
                string str = "( ";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $"Quality = {quality[((TextBlock)a).Text]} OR ";
                    else str += $"Quality = {quality[((TextBlock)a).Text]} )";
                }
                whereReq[1] = str;
            }
        }
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GetListData();
        }
    }
}
