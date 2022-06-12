using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App;
using App.WorkShop;

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
            DataContext = new CreateItemMV(0);
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
            TablePanel.Visibility = (bool) (sender as CheckBox).IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AttunBox_Checked(object sender, RoutedEventArgs e)
        {
            AttunText.Visibility = (bool)(sender as CheckBox).IsChecked?Visibility.Visible:Visibility.Collapsed;
        }
    }
}