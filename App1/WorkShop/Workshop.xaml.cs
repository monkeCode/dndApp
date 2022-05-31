using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Workshop : Page
    {
        public Workshop()
        {
            this.InitializeComponent();
        }

        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateItem));
        }
    }
}