using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace App
{
    public sealed partial class TextBlockTable : UserControl, INotifyPropertyChanged
    {
        public Table Table { get { return (Table)GetValue(TableProperty); } 
            set { SetValue(TableProperty, value); MarkdownTexts = UpdateTabe(GridTable); } }

        private List<MarkdownText> _markdownTexts;
        public List<MarkdownText> MarkdownTexts { get => _markdownTexts; set
            {
                _markdownTexts = value;
                OnPropertyChanged();
            } }

        private void OnPropertyChanged([CallerMemberName] string v = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public DependencyProperty TableProperty = DependencyProperty.Register(nameof(Table), typeof(Table), typeof(TextBlockTable), new PropertyMetadata(new Table()));

        public event PropertyChangedEventHandler PropertyChanged;

        public TextBlockTable()
        {
            this.InitializeComponent();
            MarkdownTexts = UpdateTabe(GridTable);
        }

        public List<MarkdownText> UpdateTabe(Grid grid)
        {
            if (Table == null)
            {
                return new List<MarkdownText>();
            }
            List<MarkdownText> textBlocks = new List<MarkdownText>();
            for (int i = 0; i <= Table.Rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i <= Table.Columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { MinWidth = 100 });
            var enumerator = Table.Fields.GetEnumerator();
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("ms-appx:///Dictionary.xaml");
            for (int r = 0; r < Table.Rows; r++)
                for (int c = 0; c < Table.Columns; c++)
                {
                    int val = (int)dict["tableBorderSize"];
                    Thickness thickness = new Thickness(0, 0, val, val);
                    if (r == 0)
                        thickness.Top = val;
                    if (c == 0)
                        thickness.Left = val;
                    Border border = new Border()
                    {
                        BorderThickness = thickness,
                        BorderBrush = (Brush)dict["AccentDark1"],
                        Padding = new Thickness(5),
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    MarkdownText textBlock = new MarkdownText();

                    border.Child = textBlock;
                    Grid.SetColumn(border, c);
                    Grid.SetRow(border, r);
                    if (enumerator.MoveNext())
                        textBlock.Text = enumerator.Current;
                    grid.Children.Add(border);
                    textBlocks.Add(textBlock);
                }

            return textBlocks;
        }
    }
}
