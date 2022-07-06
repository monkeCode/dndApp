﻿using System;
using App1.WorkShop;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
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

        private void ReFormateText(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            Formator.StringtoText(textBlock);
            foreach (var r in textBlock.Inlines)
            {
                if (r is Hyperlink)
                {
                    Hyperlink hyperlink = r as Hyperlink;
                    if(Dice.ContainDice((hyperlink.Inlines[0] as Run).Text))
                        hyperlink.Click += Hyperlink_rollDice;
                    else
                    {
                        hyperlink.Click += Hyperlink_Click;
                    }
                }
            }
        }



        private void Hyperlink_rollDice(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            var match = Regex.Match((sender.Inlines[0] as Run).Text, Dice.DICE_PATTERN);
            Dice dice = new Dice(match.Value);

            (sender.Inlines[0] as Run).Text = match.Value + " = " + dice.Result;
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

                foreach (TextBlock tex in table.LoadTable(TableGrid))
                {

                    foreach (var r in tex.Inlines)
                    {
                        if (r.GetType() == typeof(Hyperlink))
                        {
                            Hyperlink hyperlink = r as Hyperlink;
                            if (Regex.IsMatch((hyperlink.Inlines[0] as Run).Text, Dice.DICE_PATTERN))
                                hyperlink.Click += Hyperlink_rollDice;
                            else
                            {
                                hyperlink.Click += Hyperlink_Click;
                            }
                        }
                    }
                }
            }
        }

    }
}