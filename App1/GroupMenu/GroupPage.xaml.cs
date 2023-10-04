using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Model;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.GroupMenu
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class GroupPage : Page
    {
        private GroupMv _dataContext;
        public GroupPage()
        {
            DataContext = new GroupMv();
            _dataContext = (GroupMv)DataContext;
            this.InitializeComponent();
        }

        private async void AddPlayerClick(object sender, RoutedEventArgs e)
        {
            var playerDialog = new ChangePlayerDialog();
            var res = await playerDialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                _dataContext.AddPlayer(playerDialog.Player);
            }
        }

        private void DeletePlayerClick(object sender, RoutedEventArgs e)
        {
            var context = (sender as MenuFlyoutItem).DataContext as Player;
            _dataContext.DeletePlayer(context);

        }

        private async void ChangePlayerClick(object sender, RoutedEventArgs e)
        {
            var context = (sender as MenuFlyoutItem).DataContext as Player;

            var playerDialog = new ChangePlayerDialog(context);
            var res = await playerDialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                _dataContext.UpdatePlayer(playerDialog.Player);
            }
        }
    }
}
