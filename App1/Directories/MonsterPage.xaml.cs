﻿using System;
using App1.Model;
using App1.WorkShop;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;
using App;

namespace App1.Directories
{
    public sealed partial class MonsterPage : Page
    {
        public MonsterPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = new ExtendedMonster((int)e.Parameter);
        }

        private void TableLoading()
        {
            Table table = (DataContext as ExtendedMonster).Table;
            //if (table != null)
            //{

            //    foreach (MarkdownText tex in table.LoadTable(/*TableGrid*/ null))
            //    {

            //        foreach (var r in tex.Inlines)
            //        {
            //            if (r.GetType() == typeof(Hyperlink))
            //            {
            //                Hyperlink hyperlink = r as Hyperlink;
            //                if (Regex.IsMatch((hyperlink.Inlines[0] as Run).Text, Dice.DICE_PATTERN))
            //                    hyperlink.Click += Hyperlink_rollDice;
            //                else
            //                {
            //                    //ToolTip toolTip = new ToolTip();
            //                    //toolTip.Content = new MagicPage();
            //                    //ToolTipService.SetToolTip(hyperlink, toolTip);
            //                    hyperlink.Click += Hyperlink_Click;
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void MarkdownText_OnHyperlinkClicked(Type arg1, int arg2)
        {
            Frame.Navigate(arg1, arg2);
        }
    }
}
