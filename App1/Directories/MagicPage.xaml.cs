using App.Directories;
using Model;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    public sealed partial class MagicPage : Page
    {
        public MagicPage()
        {
            InitializeComponent();
            _model = new MagicItemsModelView();
            DataContext = _model;
        }
        private MagicItemsModelView _model;
        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            _model.SelectedType = listView.SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void ListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListView listView = sender as ListView;
            _model.SelectedQuality = listView.SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((MagicItemsModelView)DataContext).Search(sender);
        }

        private void DropFilters(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            QualityList.DeselectRange(new Windows.UI.Xaml.Data.ItemIndexRange(0, (uint)QualityList.Items.Count));
            TypeList.DeselectRange(new Windows.UI.Xaml.Data.ItemIndexRange(0, (uint)TypeList.Items.Count));
            SourceList.DeselectRange(new ItemIndexRange(0, (uint)SourceList.Items.Count));
            _model.DropFilters();
        }

        private void ItemsPanel_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MagicItemExtendedPage), (e.ClickedItem as MagicItem).Id);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            _model.SelectedSource = listView.SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void NameClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            ExecuteSort(mn => mn.Name);
        }

        private void QualityClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            ExecuteSort(mn => mn.Quality.ToString());
        }

        private void ConsentrationClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            ExecuteSort(mn => mn.Attunement);
        }

        private void TypeClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            ExecuteSort(mn => mn.Type);
        }

        private void ExecuteSort(Func<MagicItem, object> sortDel)
        {
            (DataContext as MagicItemsModelView).SortCommand.Execute(sortDel);
        }
    }
}