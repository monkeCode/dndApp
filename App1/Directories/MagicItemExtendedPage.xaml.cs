using App;
using App.Helpers;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace App1.Directories
{
    public sealed partial class MagicItemExtendedPage : Page
    {
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

        private void MarkdownText_OnHyperlinkClicked(Type obj, int id)
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

            // Create a new PrintHelper instance
            // "container" is a XAML panel that will be used to host printable control.
            // It needs to be in your visual tree but can be hidden with Opacity = 0
            var printHelper = new PrintHelper(new Grid(), defaultPrintHelperOptions);
            ExtendedMagicItem data = DataContext as ExtendedMagicItem;
            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock() { Text = data.Name, FontSize = 24, Margin = new Thickness(30) });
            printHelper.AddFrameworkElementToPrint(panel);

            await printHelper.ShowPrintUIAsync("DnD Helper", true);
            // Add controls that you want to print
            //printHelper.AddFrameworkElementToPrint(await PrepareWebViewForPrintingAsync());

        }
        //private async void PrintHelper_OnPrintSucceeded()
        //{
        //    printHelper.Dispose();
        //    var dialog = new MessageDialog("Printing done.");
        //    await dialog.ShowAsync();
        //}

        //private async void PrintHelper_OnPrintFailed()
        //{
        //    printHelper.Dispose();
        //    var dialog = new MessageDialog("Printing failed.");
        //    await dialog.ShowAsync();
        //}

    }
}