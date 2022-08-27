using Model;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.Directories
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BestiaryPage : Page
    {
        public BestiaryPage()
        {
            this.InitializeComponent();
            DataContext = new MonsterModelView();
        }


        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as MonsterModelView).Search(sender);
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            (DataContext as MonsterModelView).DropFilters();
            TypeList.DeselectRange(new ItemIndexRange(0, (uint)TypeList.Items.Count));
            ChallengeList.DeselectRange(new ItemIndexRange(0, (uint)ChallengeList.Items.Count));
            SizeList.DeselectRange(new ItemIndexRange(0, (uint)SizeList.Items.Count));
            HabbiatList.DeselectRange(new ItemIndexRange(0, (uint)HabbiatList.Items.Count));
            SourceList.DeselectRange(new ItemIndexRange(0, (uint)SourceList.Items.Count));
        }

        private void TypeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedType = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void ChallengeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedRate = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void SizeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MonsterModelView model = (DataContext as MonsterModelView);
            ListView lv = sender as ListView;
            model.SelectedSize = lv.SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void HabbiatList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedHabitat = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
        }
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedSource = (sender as ListView).SelectedItems.Select(it => it.ToString()).ToList();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(MonsterPage), (e.ClickedItem as Monster).Id);
        }

        private void NameSort(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            ExecuteSort(m => m.Name);
        }

        private void TypeSort(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            ExecuteSort(m => m.Type);
        }

        private void SizeSort(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            ExecuteSort((m) => m.Size.ToString());
        }

        private void DangerousSort(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            ExecuteSort(m =>
                StaticValues.MonsterRate.IndexOf(m.Challenge));
        }

        private void ExecuteSort(Func<Monster, object> sortDel)
        {
            (DataContext as MonsterModelView).SortCommand.Execute(sortDel);
        }
    }
}
