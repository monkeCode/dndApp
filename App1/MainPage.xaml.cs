﻿using App.Directories;
using App.Encounters;
using App.Generator;
using App.GroupMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace App
{
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Dice> _dices = new ObservableCollection<Dice>();

        public event Action<object, string> TabChanged;

        public Frame ContentFrame => mainFrame;

        public MainPage()
        {
            this.InitializeComponent();
            mainFrame.Navigated += MainFrame_Navigated1;
        }

        private void MainFrame_Navigated1(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var type = e.Content.GetType();
            var text = type.Name;
            TabChanged?.Invoke(Frame, text);
        }

        private List<(string Tag, System.Type Page)> TagPage = new List<(string, System.Type)>
        {
           ("MagicItems", typeof(MagicPage)),
           ("Workshop", typeof(Workshop)),
           ("Monsters", typeof(BestiaryPage)),
           ("Encounters", typeof(EncounterPage)),
            ("GroupManager", typeof(GroupPage)),
            ("Quests", typeof(Quest.QuestPage)),
            ("LootGenerator", typeof(LootGeneratorPage)),
            ("Параметры", typeof(SettingPage))
        };

        private void NavView_Navigate(
    string navItemTag,
    Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            System.Type page;

            try
            {
                var item = TagPage.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                page = item.Page;
            }
            catch { return; }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = mainFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(page is null) && !System.Type.Equals(preNavPageType, page))
            {
                mainFrame.Navigate(page, null, transitionInfo);
            }
        }


        private bool TryGoBack()
        {
            if (!mainFrame.CanGoBack)
                return false;

            //// Don't go back if the nav pane is overlayed.
            //if (NavPanel.IsPaneOpen &&
            //    (NavPanel.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
            //     NavView.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
            //    return false;

            mainFrame.GoBack();
            return true;
        }

        private void navPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            mainFrame.Navigated += MainFrame_Navigated;
        }

        private void MainFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            NavPanel.IsBackEnabled = mainFrame.CanGoBack;
        }

        private void DiceText_acceptEvent(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (!string.IsNullOrEmpty(diceText.Text))
                    try
                    {
                        _dices.Add(new Dice(diceText.Text));
                        if (_dices.Count > 10)
                            _dices.RemoveAt(0);
                        listDices.ScrollIntoView(listDices.Items.Last(), ScrollIntoViewAlignment.Leading);
                        listDices.UpdateLayout();
                    }
                    catch { }
            }
        }

        private void dicePanel_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            if (e.NewSize.Width < 50)
                foreach (var ch in stackPanel.Children)
                    ch.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
                foreach (var ch in stackPanel.Children)
                    ch.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void NavPanel_OnItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            string tag = args.InvokedItemContainer.Tag.ToString();
            NavView_Navigate(tag, args.RecommendedNavigationTransitionInfo);
        }

        private void NavPanel_OnBackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }
    }
}