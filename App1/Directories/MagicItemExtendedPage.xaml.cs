using App1.WorkShop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Directories
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MagicItemExtendedPage : Page
    {
        public MagicItemExtendedPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = new ExtendedMIviewModel((int)e.Parameter);
            TableLoading();
        }
        private void ReFormateText(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            Formator.StringtoText(textBlock);
            foreach(var r in textBlock.Inlines)
            {
                if(r.GetType() == typeof(Hyperlink))
                {
                    Hyperlink hyperlink = r as Hyperlink;
                    hyperlink.Click += Hyperlink_Click;
                }
            }    
        }

        private void Hyperlink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            ExtendedMIviewModel model = DataContext as ExtendedMIviewModel; 
            Link link = model.Links.First(obj => obj.Text == (sender.Inlines.First() as Run).Text);
            Frame.Navigate(link.Page, link.Id);
        }

        void TableLoading()
        {
            Table table = (DataContext as ExtendedMIviewModel).Table;
            table.LoadTable(TableGrid);
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if(FeaturesList.Items.Count == 0)
            {
                MainPanel.Children.Remove((UIElement)sender);
            }
        }
    }
}
