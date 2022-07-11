using App1.WorkShop;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App;
using DataBaseLib;

namespace App1
{
    public class Table
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<string> Fields { get; set; }
        public Table() { }
        public Table(object[] dataList)
        {
            Rows = (int)(long)dataList[1];
            Columns = (int)(long)dataList[2];
            Fields = new List<string>(dataList[3].ToString().Split('@').ToList());
        }
        public List<MarkdownText> LoadTable(Grid grid)
        {
            List<MarkdownText> textBlocks = new List<MarkdownText>();
            for (int i = 0; i <= this.Rows; i++)
                grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i <= this.Columns; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { MinWidth = 100});
            var enumerator = this.Fields.GetEnumerator();
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("ms-appx:///Dictionary.xaml");
            for (int r = 0; r < this.Rows; r++)
                for (int c = 0; c < this.Columns; c++)
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
                        BorderBrush = (Windows.UI.Xaml.Media.Brush)dict["AccentDark1"],
                        Padding = new Thickness(5),
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    MarkdownText textBlock = new MarkdownText();

                    border.Child = textBlock;
                    Grid.SetColumn(border, c);
                    Grid.SetRow(border, r);
                    if(enumerator.MoveNext())
                        textBlock.Text = enumerator.Current;
                    grid.Children.Add(border);
                    textBlocks.Add(textBlock);
                }

            return textBlocks;
        }

        public void UpdateTable(string dbTable, int id)
        {
            DeleteTable(dbTable, id);
            string data = string.Join("@", Fields.Select(it=> Formator.CreateDbValidStr(it)));
            DataAccess.RawRequest(
                $"INSERT INTO {dbTable} (ParentId, Rows, Columns, Data) " +
                $"values ({id},{Rows}, {Columns}, \'{data}\')");
        }

        public static void DeleteTable(string dbTable, int id)
        {
            DataAccess.RawRequest($"Delete from {dbTable} where ParentId = {id}");
        }
    }
}