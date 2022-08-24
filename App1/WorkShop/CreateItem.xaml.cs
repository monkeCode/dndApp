using App.Model;
using App.WorkShop;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateItem : Page
    {
        private bool isSaved;
        public CreateItem()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var id = (int?)e.Parameter;
            DataContext = id.HasValue ? new CreateItemMV(id.Value) : new CreateItemMV();
            base.OnNavigatedFrom(e);
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

        private void RefName_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var text = sender.Text.ToLower();
                var items = DataItem.GetItems(text);
                sender.ItemsSource = items;
            }
        }
        private async Task<ContentDialogResult> ShowDialog_Click()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Сохранить изменения?";
            dialog.PrimaryButtonText = "Сохранить";
            dialog.SecondaryButtonText = "Не сохранять";
            dialog.CloseButtonText = "Отмена";
            dialog.DefaultButton = ContentDialogButton.Primary;

            var result = await dialog.ShowAsync();
            return result;
        }

        private async void SaveBeforeExit(Type page)
        {
            var result = await ShowDialog_Click();
            switch (result)
            {
                case ContentDialogResult.Primary:
                    Save();
                    Frame.Navigate(page);
                    break;
                case ContentDialogResult.Secondary:
                    isSaved = true;
                    Frame.Navigate(page);
                    break;
                case ContentDialogResult.None:
                    return;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

            if (!isSaved)
            {
                e.Cancel = true;
                SaveBeforeExit(e.SourcePageType);
            }
            else
            {
                base.OnNavigatingFrom(e);
            }

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Frame.Navigate(typeof(Workshop));
        }

        private void Save()
        {
            var table = Table.LoadTableData();
            (DataContext as CreateItemMV).Item.Table = table;
            (DataContext as CreateItemMV).Save();
            isSaved = true;
        }

        private void DeleteFeature(object sender, RoutedEventArgs e)
        {
            (DataContext as CreateItemMV).Item.Features.Remove((Features)(sender as Button).DataContext);
        }
    }
}