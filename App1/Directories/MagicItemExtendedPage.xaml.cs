using System;
using App1.WorkShop;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Graphics.Printing;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using App;
using App.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;


namespace App1.Directories
{
    public sealed partial class MagicItemExtendedPage : Page
    {
        private PrintHelper _printHelper;
        public MagicItemExtendedPage()
        {
            this.InitializeComponent();
        }

        private async void LoadImage()
        {
           var name = (DataContext as ExtendedMagicItem).Name;
           var result = await Web.GetImageUri(name + " днд");
           ItemImage.Source = new BitmapImage(new Uri(result));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = new ExtendedMagicItem((int)e.Parameter);
            TableLoading();
            //LoadImage();
        }


        private void TableLoading()
        {
            Table table = (DataContext as ExtendedMagicItem).Table;
            if (table != null)
            {

                foreach (MarkdownText tex in table.LoadTable(TableGrid))
                {

                    tex.HyperlinkClicked += MarkdownText_OnHyperlinkClicked;
                }
            }
        }

        private void MarkdownText_OnHyperlinkClicked(Type obj,int id)
        {
            Frame.Navigate(obj, id);
        }

        private void PrintButton_OnClick(object sender, RoutedEventArgs e)
        {
            Print();
        }

        private async void Print()
        {
            var defaultPrintHelperOptions = new PrintHelperOptions();

            // Configure options that you want to be displayed on the print dialog
            defaultPrintHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            defaultPrintHelperOptions.Orientation = PrintOrientation.Portrait;

            _printHelper = new PrintHelper(PrintContent);
            //StackPanel panel = new StackPanel();
            //ExtendedMagicItem data = DataContext as ExtendedMagicItem;
            //panel.Children.Add(new TextBlock() { Text = data.Name, FontSize = 24, Margin = new Thickness(30) });
            MainPanel.Children.Remove(ItemPanel);
            _printHelper.AddFrameworkElementToPrint(ItemPanel);

            await _printHelper.ShowPrintUIAsync("DnD Helper", defaultPrintHelperOptions);
  

        }

    }
}