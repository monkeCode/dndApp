using App.Model;
using App.WorkShop;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateItem : Page
    {
        public CreateItem()
        {
            this.InitializeComponent();
            var context = new CreateItemMV(2);
            DataContext = context;

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid grid = grid2;
            if (e.NewSize.Width < 700)
            {
                Grid.SetRow(grid, 1);
                Grid.SetColumn(grid, 0);
                ((Grid)sender).ColumnDefinitions[1].Width = GridLength.Auto;
            }
            else
            {
                Grid.SetRow(grid, 0);
                Grid.SetColumn(grid, 1);
                ((Grid)sender).ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private void Description_Loaded(object sender, RoutedEventArgs e)
        {
            RichEditBox editBox = sender as RichEditBox;
        }


        private void AddColumn(object sender, RoutedEventArgs e)
        {
            Table.Columns++;
            // ColumnCounter.Text = Table.Columns.ToString();
        }

        private void RemoveColumn(object sender, RoutedEventArgs e)
        {
            Table.Columns--;
            // ColumnCounter.Text = Table.Columns.ToString();
        }

        private void RemoveRow(object sender, RoutedEventArgs e)
        {
            Table.Rows--;
            //RowCounter.Text = Table.Rows.ToString();
        }

        private void AddRow(object sender, RoutedEventArgs e)
        {
            Table.Rows++;
            // RowCounter.Text = Table.Rows.ToString();
        }

        private void TableStateChanged(object sender, RoutedEventArgs e)
        {
            TablePanel.Visibility = (bool)(sender as CheckBox).IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AttunBox_Checked(object sender, RoutedEventArgs e)
        {
            AttunText.Visibility = (bool)(sender as CheckBox).IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AddRef(object sender, RoutedEventArgs e)
        {

        }

        private void RefName_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ((CreateItemMV)DataContext).AddLink((DataItem)args.SelectedItem);
        }

        private void RefName_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var text = sender.Text.ToLower();
                var items = ((CreateItemMV)DataContext).GetItemsByName(text);
                sender.ItemsSource = items;
            }
        }


    }
}