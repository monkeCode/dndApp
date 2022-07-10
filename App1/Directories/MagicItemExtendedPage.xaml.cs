using System;
using App1.WorkShop;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using App;
using App.Helpers;


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



        private void Hyperlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            ExtendedMagicItem model = DataContext as ExtendedMagicItem;
            Link link = model.Links.First(obj => obj.Text == (sender.Inlines.First() as Run).Text);
            Frame.Navigate(link.Page, link.Id);
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
    }
}