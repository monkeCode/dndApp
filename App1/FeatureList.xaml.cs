using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using App1;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace App
{
    public sealed partial class FeatureList : UserControl
    {
        public Collection<Features> Features
        {
            get => (Collection<Features>)GetValue(FeaturesProperty);
            set => SetValue(FeaturesProperty, value);
        }

        public string ListName
        {
            get => (string)GetValue(ListNameProperty);
            set => SetValue(ListNameProperty,value);
        }
        
        public static readonly DependencyProperty FeaturesProperty = DependencyProperty.Register(nameof(Features), typeof(Collection<Features>), typeof(FeatureList),new PropertyMetadata(new Collection<Features>()) );
        
        public static readonly DependencyProperty ListNameProperty = DependencyProperty.Register(nameof(ListName), typeof(string), typeof(FeatureList), new PropertyMetadata(""));
        
        public FeatureList()
        {
            this.InitializeComponent();
        }
        private void DeleteFeature(object sender, RoutedEventArgs e)
        {
            Features.Remove((Features) (sender as Button).DataContext);
        }
    }
}
