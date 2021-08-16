using System.Collections.Generic;
using App1.WorkShop;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Drawing;
using System;

namespace App1
{
    internal class Table
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public ObservableCollection<string> Fields { get; set; }

        public Table(object[] dataList)
        {
            Rows = (int) (long) dataList[1];
            Columns = (int)(long)dataList[2];
            Fields = new ObservableCollection<string>(dataList[3].ToString().Split('@').ToList());
        }
        public List<TextBlock> LoadTable(Grid grid)
        {
            List<TextBlock> textBlocks = new List<TextBlock>();
            for (int i = 0; i <= this.Rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i <= this.Columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { MinWidth = 150 });
            var enumerator = this.Fields.GetEnumerator();
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("ms-appx:///Dictionary.xaml");
            for (int r = 0; r < this.Rows; r++)
                for (int c = 0; c < this.Columns; c++)
                {
                    Border border = new Border()
                    {
                        BorderThickness = new Thickness(1),
                        BorderBrush = (Windows.UI.Xaml.Media.Brush)dict["AccentDark1"],
                        Padding = new Thickness(5),
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    TextBlock textBlock = new TextBlock();
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    border.Child = textBlock;
                    Grid.SetColumn(border, c);
                    Grid.SetRow(border, r);
                    if (enumerator.MoveNext())
                        Formator.StringtoText(textBlock, enumerator.Current);
                    grid.Children.Add(border);
                    textBlocks.Add(textBlock);
                }

            return textBlocks;
        }
    }
}