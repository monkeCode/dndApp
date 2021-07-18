using DataBaseLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            InitializeComponent();
            DataContext = new MagicItemsModelView();
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MagicItemsModelView view = DataContext as MagicItemsModelView;
            ListView listView = sender as ListView;
            if (listView.SelectedIndex == -1)
            {
                view.SelectedQuality = null;
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

               view.SelectedQuality = str;
            }
        }

        private void ListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MagicItemsModelView view = DataContext as MagicItemsModelView;
            ListView listView = sender as ListView;
            var i = listView.SelectedItem;
            if (listView.SelectedIndex == -1)
            {
                view.SelectedType = null;
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
                view.SelectedType = str;
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((MagicItemsModelView)DataContext).Search(sender);
        }

        private void DropFilters(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MagicItemsModelView view = DataContext as MagicItemsModelView;
            //searchBox.Text = "";
            QualityList.DeselectRange(new Windows.UI.Xaml.Data.ItemIndexRange(0, (uint)QualityList.Items.Count));
            TypeList.DeselectRange(new Windows.UI.Xaml.Data.ItemIndexRange(0, (uint)TypeList.Items.Count));
        }
    }
}