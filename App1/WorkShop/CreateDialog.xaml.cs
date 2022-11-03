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
using Model;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.WorkShop
{
    public sealed partial class CreateDialog : Page
    {
        public CreateDialog(IEnumerable<DataItem> items)
        {
            this.InitializeComponent();
            Items = items.ToList();
        }

        public Monster SelectedItem { get; set; }
        private List<DataItem> Items { get; set; }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SelectedItem = args.SelectedItem as Monster;
        }

        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<DataItem>();
                if (sender.Text.Length == 0)
                {
                    sender.ItemsSource = suitableItems;
                    return;
                }
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var item in Items)
                {
                    var found = splitText.All((key) =>
                    {
                        return item.Name.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(item);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    //suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }

        }
    }
}
