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

namespace App1.Encounters
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class EncounterPage : Page
    {
        public EncounterPage()
        {
            DataContext = new EncounterModelView();
            this.InitializeComponent();
        }

        private void EncounterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            (DataContext as EncounterModelView).SelectionEncountersChanged(list.SelectedItems.GetEnumerator());
        }

        private void AddEncounter(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
