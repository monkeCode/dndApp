using Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace App
{
    public sealed partial class FeatureList : UserControl
    {
        public Collection<Feature> Features
        {
            get => (Collection<Feature>)GetValue(FeaturesProperty);
            set => SetValue(FeaturesProperty, value);
        }

        public string ListName
        {
            get => (string)GetValue(ListNameProperty);
            set => SetValue(ListNameProperty, value);
        }

        public static readonly DependencyProperty FeaturesProperty = DependencyProperty.Register(nameof(Features), typeof(Collection<Feature>), typeof(FeatureList), new PropertyMetadata(new Collection<Feature>()));

        public static readonly DependencyProperty ListNameProperty = DependencyProperty.Register(nameof(ListName), typeof(string), typeof(FeatureList), new PropertyMetadata(""));

        public FeatureList()
        {
            this.InitializeComponent();
        }
        private void DeleteFeature(object sender, RoutedEventArgs e)
        {
            Features.Remove((Feature)(sender as Button).DataContext);
        }
    }
}
