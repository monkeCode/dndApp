﻿using App.WorkShop;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace App
{
    public sealed partial class MarkdownText : UserControl
    {
        private bool _isLoaded;
        public event Action<Type, int> HyperlinkClicked;
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
                //UpdateText(MarkableTextBlock, value);
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text", typeof(string), typeof(MarkdownText), new PropertyMetadata("")
            );
        public MarkdownText()
        {
            this.InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isLoaded) return;
            UpdateText(sender as TextBlock, (sender as TextBlock)?.Text);
        }

        private void UpdateText(TextBlock sender, string text)
        {
            _isLoaded = true;
            TextBlock textBlock = sender;
            textBlock.Text = text;
            Formator.StringtoText(textBlock);
            foreach (var r in textBlock.Inlines)
            {
                if (r is Hyperlink)
                {
                    Hyperlink hyperlink = r as Hyperlink;
                    if (Dice.ContainDice((hyperlink.Inlines[0] as Run).Text))
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
            var itemList = App.DataContext.GetDataItems();
            string name = (sender.Inlines.First() as Run).Text;
            var item = itemList.OrderBy(it => name.LevenshteinDistance(it.Name)).First();
            //ExtendedMagicItem model = DataContext as ExtendedMagicItem;
            //Link link = model.Links.First(obj => obj.Text == (sender.Inlines.First() as Run).Text);
            Link link = new Link(item.ItemType, item.Id, null);
            HyperlinkClicked?.Invoke(link.Page, link.Id);
        }
    }
}
