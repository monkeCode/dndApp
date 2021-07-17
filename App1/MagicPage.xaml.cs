using DataBaseLib;
using System;
using System.Collections;
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
            string s = whereReq.GetAllElemets(" AND ");
            foreach (object[] i in DataAccess.GetData("MagicItems", s, "*"))
            {
                
               string subString = searchBox.Text.ToLower().Trim();
                if (subString == "" || i[1].ToString().ToLower().IndexOf(subString) != -1)
                {
                    if (i[1].ToString().ToLower().IndexOf(subString) !=-1)
                        magicItems.Add(new MagicItem(int.Parse(i[0].ToString()), i[1].ToString(), (MagicItem.ItemQuality)(long)i[2], i[3].ToString(), i[4].ToString()));
                }
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

                string str = "Type IN ( ";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $" \"{((TextBlock)a).Text}\", ";
                    else str += $"\"{((TextBlock)a).Text}\" )";
                }
             
                whereReq[0] = str;
            }
            Refresh();
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
                    {"Варьируется", 5 }
                };
                string str = "Quality IN ( ";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $"{quality[((TextBlock)a).Text]}, ";
                    else str += $"{quality[((TextBlock)a).Text]} )";
                }
                whereReq[1] = str;
            }
            Refresh();
        }
        private void Refresh()
        {
            GetListData();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void DropFilters(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
