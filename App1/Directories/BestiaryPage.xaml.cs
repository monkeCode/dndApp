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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1.Directories
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BestiaryPage : Page
    {
        public BestiaryPage()
        {
            this.InitializeComponent();
            DataContext = new MonsterModelView();
        }

        string ChangeSelect(ListView sender, string column)
        {
            if (sender.SelectedIndex == -1)
            {
                return null;
            }
           
            string str = column+" IN ( ";
            foreach (var a in sender.SelectedItems)
            {
                if (a != sender.SelectedItems.Last())
                    str += $" \"{a}\", ";
                else str += $"\"{a}\" )";
            }

            return str;
        }
        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TypeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedType = ChangeSelect(sender as ListView, "Type");
        }

        private void ChallengeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedRate = ChangeSelect(sender as ListView, "ChallengeRate");
        }

        private void SizeList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MonsterModelView model = (DataContext as MonsterModelView);
            ListView lv = sender as ListView;
            ;
            if (lv.SelectedIndex == -1)
            {
                model.SelectedSize = null;
            }
            else
            {

                string str = "Size IN ( ";
                foreach (var a in lv.SelectedItems)
                {
                    if (a != lv.SelectedItems.Last())
                        str += $" \"{StaticValues.monsterSize[a.ToString()]}\", ";
                    else str += $"\"{StaticValues.monsterSize[a.ToString()]}\" )";
                }

                model.SelectedSize = str;
            }
        }

        private void HabbiatList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedHabitat = ChangeSelect(sender as ListView, "Habbitat");
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as MonsterModelView).SelectedSource = ChangeSelect(sender as ListView, "Source");
        }
    }
}
