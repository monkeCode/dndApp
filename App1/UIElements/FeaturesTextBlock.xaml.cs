﻿using Model;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace App
{
    public sealed partial class FeaturesTextBlock : UserControl
    {
        public Collection<Feature> FeatureList { get { return (Collection<Feature>)GetValue(FeatureListProperty); } set { SetValue(FeatureListProperty, value); } }

        public event Action<Type, int> MarkDownTextClicked;

        public static readonly DependencyProperty FeatureListProperty = DependencyProperty.Register(nameof(FeatureList), typeof(Collection<Feature>), typeof(FeaturesTextBlock), new PropertyMetadata(null));

        public FeaturesTextBlock()
        {
            this.InitializeComponent();
        }

        private void MarkdownText_HyperlinkClicked(Type arg1, int arg2)
        {
            MarkDownTextClicked?.Invoke(arg1, arg2);
        }
    }
}
