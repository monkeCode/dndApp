using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Directories
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
        }

        private void TypeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedType = (sender as ListView).SelectedItems;
        }

        private void ChallengeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedRate = (sender as ListView).SelectedItems;
        }

        private void SizeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MonsterModelView model = (DataContext as MonsterModelView);
            ListView lv = sender as ListView;
            model.SelectedSize = lv.SelectedItems;
        }

        private void HabbiatList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedHabitat = (sender as ListView).SelectedItems;
        }
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedSource = (sender as ListView).SelectedItems;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(MonsterPage), (e.ClickedItem as Monster).Id);
        }

        private void NameSort(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            ExecuteSort( m => m.Name );
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
            ExecuteSort(m=>
                StaticValues.MonsterRate.IndexOf(m.Challenge));
        }

        private void ExecuteSort(Func<Monster, object> sortDel)
        {
            (DataContext as MonsterModelView).SortCommand.Execute(sortDel);
        }
    }
}
