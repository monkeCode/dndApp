using Model;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace App.Directories
{
    public sealed partial class MonsterPage : Page
    {
        public MonsterPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = App.DataContext.GetExtendedMonsterById((int)e.Parameter);
            TableGrid.Table = (DataContext as ExtendedMonster).Table;
            TableLoading();
            TabMenu.UpdateText(Frame,(DataContext as Monster).Name);
        }

        private void TableLoading()
        {
            Table table = (DataContext as ExtendedMonster).Table;
            if (table != null)
            {

                foreach (MarkdownText tex in TableGrid.MarkdownTexts)
                {

                    tex.HyperlinkClicked += MarkdownText_OnHyperlinkClicked;
                }
            }
        }

        private void MarkdownText_OnHyperlinkClicked(Type arg1, int arg2)
        {
            Frame.Navigate(arg1, arg2);
        }
    }
}
