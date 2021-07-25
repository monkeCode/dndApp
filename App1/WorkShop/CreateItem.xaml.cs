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

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateItem : Page
    {
        public CreateItem()
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

        private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
