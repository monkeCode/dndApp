﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App1;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.WorkShop
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateMonster : Page
    {
        private bool isSaved;
        
        public CreateMonster()
        {
            this.InitializeComponent();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid grid = grid2;
            if (e.NewSize.Width < 700)
            {
                Grid.SetRow(grid, 1);
                Grid.SetColumn(grid, 0);
                ((Grid)sender).ColumnDefinitions[1].Width = GridLength.Auto;
            }
            else
            {
                Grid.SetRow(grid, 0);
                Grid.SetColumn(grid, 1);
                ((Grid)sender).ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            }
        }


        private void AddColumn(object sender, RoutedEventArgs e)
        {
            Table.Columns++;
            // ColumnCounter.Text = Table.Columns.ToString();
        }

        private void RemoveColumn(object sender, RoutedEventArgs e)
        {
            Table.Columns--;
            // ColumnCounter.Text = Table.Columns.ToString();
        }

        private void RemoveRow(object sender, RoutedEventArgs e)
        {
            Table.Rows--;
            //RowCounter.Text = Table.Rows.ToString();
        }

        private void AddRow(object sender, RoutedEventArgs e)
        {
            Table.Rows++;
            // RowCounter.Text = Table.Rows.ToString();
        }

        private void TableStateChanged(object sender, RoutedEventArgs e)
        {
            TablePanel.Visibility = (bool)(sender as CheckBox).IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
        private async Task<ContentDialogResult> ShowDialog_Click()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Сохранить изменения?";
            dialog.PrimaryButtonText = "Сохранить";
            dialog.SecondaryButtonText = "Не сохранять";
            dialog.CloseButtonText = "Отмена";
            dialog.DefaultButton = ContentDialogButton.Primary;

            var result = await dialog.ShowAsync();
            return result;
        }

        private async void SaveBeforeExit(Type page)
        {
            var result = await ShowDialog_Click();
            switch (result)
            {
                case ContentDialogResult.Primary:
                    ((CreateItemMV)DataContext).Save();
                    isSaved = true;
                    Frame.Navigate(page);
                    break;
                case ContentDialogResult.Secondary:
                    isSaved = true;
                    Frame.Navigate(page);
                    break;
                case ContentDialogResult.None:
                    return;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

            if (!isSaved)
            {
                e.Cancel = true;
                SaveBeforeExit(e.SourcePageType);
            }
            else
            {
                base.OnNavigatingFrom(e);
            }

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var table = Table.LoadTableData();
            (DataContext as CreateItemMV).Item.Table = table;
            (DataContext as CreateItemMV).Save();
            isSaved = true;
            Frame.Navigate(typeof(Workshop));
        }

        private void HabittatLoaded(object sender, RoutedEventArgs e)
        {
            var list = sender as ListView;
            if(list == null) return;
            foreach (var hab in (DataContext as CreateMonsterVM).Monster.Habitat)
            {
                list.SelectedItems.Add(hab);
            }
        }

        private void Legendary(object sender, RoutedEventArgs e)
        {
            LegendaryPanel.Visibility = (sender as CheckBox).IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
