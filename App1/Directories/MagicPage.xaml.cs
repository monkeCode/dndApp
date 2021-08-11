using App1.Directories;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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
                        str += $" \"{a}\", ";
                    else str += $"\"{a}\" )";
                }

                view.SelectedQuality = str;
            }
        }

        private void ListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MagicItemsModelView view = DataContext as MagicItemsModelView;
            ListView listView = sender as ListView;
            if (listView.SelectedIndex == -1)
            {
                view.SelectedType = null;
            }
            else
            {
                string str = "Quality IN ( ";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $"{listView.Items.IndexOf(a)}, ";
                    else str += $"{listView.Items.IndexOf(a)} )";
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
            SourceList.DeselectRange(new ItemIndexRange(0, (uint)SourceList.Items.Count));
        }

        private void ItemsPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MagicItemExtendedPage), (e.ClickedItem as MagicItem).Id);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MagicItemsModelView view = DataContext as MagicItemsModelView;
            ListView listView = sender as ListView;
            if (listView.SelectedIndex == -1)
            {
                view.SelectedSource = null;
            }
            else
            {
                string str = "Source IN ( ";
                foreach (var a in listView.SelectedItems)
                {
                    if (a != listView.SelectedItems.Last())
                        str += $" \"{a}\", ";
                    else str += $"\"{a}\" )";
                }
                view.SelectedSource = str;
            }
        }
    }
}