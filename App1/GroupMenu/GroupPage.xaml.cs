using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.GroupMenu
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class GroupPage : Page
    {
        private GroupMV _dataContext;
        public GroupPage()
        {
            DataContext = new GroupMV();
            _dataContext = (GroupMV)DataContext;
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
    }
}
