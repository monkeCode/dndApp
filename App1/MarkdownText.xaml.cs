﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
using App1;
using App1.WorkShop;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace App
{
    public sealed partial class MarkdownText : UserControl
    {
        public event Action<Type,int> HyperlinkClicked; 
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
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
            TextBlock textBlock = sender as TextBlock;
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
            ExtendedMagicItem model = DataContext as ExtendedMagicItem;
            Link link = model.Links.First(obj => obj.Text == (sender.Inlines.First() as Run).Text);
            HyperlinkClicked?.Invoke(link.Page, link.Id);
        }
    }
}