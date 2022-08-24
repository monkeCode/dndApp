using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.Generator
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class LootGeneratorPage : Page
    {
        public LootGeneratorPage()
        {
            this.InitializeComponent();
            DataContext = new LootGeneratorMv();
        }
    }
}
