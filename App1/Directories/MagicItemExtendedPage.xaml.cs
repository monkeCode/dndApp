﻿using App1.WorkShop;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Controls;

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
            DataContext = new ExtendedMagicItem((int)e.Parameter);
            TableLoading();
        }

        private void ReFormateText(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            Formator.StringtoText(textBlock);
            foreach (var r in textBlock.Inlines)
            {
                if (r.GetType() == typeof(Hyperlink))
                {
                    Hyperlink hyperlink = r as Hyperlink;
                    if (Regex.IsMatch((hyperlink.Inlines[0] as Run).Text, Dice.DICE_PATTERN))
                        hyperlink.Click += Hyperlink_rollDice;
                    else
                        hyperlink.Click += Hyperlink_Click;
                }
            }
        }

        private void Hyperlink_rollDice(Hyperlink sender, HyperlinkClickEventArgs args)
        {
           var match =  Regex.Match((sender.Inlines[0] as Run).Text, Dice.DICE_PATTERN);
            Dice dice = new Dice(match.Value);

            (sender.Inlines[0] as Run).Text = match.Value +" = "+dice.Result;
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
                table.LoadTable(TableGrid);
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (FeaturesList.Items.Count == 0)
            {
                MainPanel.Children.Remove((UIElement)sender);
            }
        }
    }
}