using App.WorkShop;
using Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Workshop : Page
    {
        public Workshop()
        {
            this.InitializeComponent();
            DataContext = new WorkShopModelView();
        }

        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateItem));
        }


        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse ||
                e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
            }
        }

        private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataItem = (e.AddedItems[0] as DataItem);
            switch (dataItem.ItemType)
            {
                case DataItem.DataType.MagicItem:
                    Frame.Navigate(typeof(CreateItem), dataItem.Id);
                    break;
                case DataItem.DataType.Monster:
                    Frame.Navigate(typeof(CreateMonster), dataItem.Id);
                break;
                case DataItem.DataType.Spell:

                break;
            }
        }

    private void AddNewMonster(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateMonster));
        }

    private void DeleteItem(object sender, RoutedEventArgs e)
    {
            var context = (sender as FrameworkElement).DataContext as DataItem;
            (DataContext as WorkShopModelView).Delete(context);
            
    }  
    }
}