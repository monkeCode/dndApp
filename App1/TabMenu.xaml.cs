using Microsoft.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TabMenu : Page
    {
        private static TabMenu _instance;
        public TabMenu()
        {
            this.InitializeComponent();
            _instance = this;
        }
        private void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TabView).TabItems.Add(CreateNewTab());

        }

        private void TabView_AddButtonClick(TabView sender, object args)
        {
            var tab = CreateNewTab();
            sender.TabItems.Add(tab);
            sender.SelectedItem = tab;
        }

        private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            if (sender.TabItems.Count == 1) return;
            sender.TabItems.Remove(args.Tab);
        }

        private TabViewItem CreateNewTab()
        {
            TabViewItem newItem = new TabViewItem();
            newItem.Header = $"Пустая вкладка";
            newItem.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };

            Frame frame = new Frame();
            frame.Navigate(typeof(MainPage));
            (frame.Content as MainPage).TabChanged += TabMenu_TabChanged; ;
            newItem.Content = frame;
            return newItem;
        }

        private void TabMenu_TabChanged(object sender, string header)
        {
            (Tabs.TabItems.First(it => ((TabViewItem)it).Content == sender) as TabViewItem).Header = header;
        }

        private void OpenAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var tab = CreateNewTab();
            Tabs.TabItems.Add(tab);
            Tabs.SelectedItem = tab;
            args.Handled = true;
        }

        private void CloseAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var invokedTabView = (args.Element as TabView);

            if (Tabs.TabItems.Count == 1 || !((TabViewItem)invokedTabView.SelectedItem).IsClosable) return;
            Tabs.TabItems.Remove(Tabs.SelectedItem);
            args.Handled = true;

        }

        public static void UpdateText(Frame frame, string text)
        {
            var it = _instance.Tabs.TabItems.FirstOrDefault(it => (((Frame)(((TabViewItem)it).Content)).Content as MainPage)?.ContentFrame == frame) as TabViewItem;
            if (it != null)
                it.Header = text;
        }
    }
}
