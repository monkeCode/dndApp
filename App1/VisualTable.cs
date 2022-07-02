using App1;
using App1.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App
{
    public class VisualTable : Grid, INotifyPropertyChanged
    {
        public int Columns
        {
            get => ColumnDefinitions.Count;
            set
            {
                if (value <= 0)
                    value = 1;
                if (value > Columns)
                {
                    for (int i = Columns; i < value; i++)
                    {
                        ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 150, MaxWidth = 300 });
                        for (int j = 0; j < Rows; j++)
                        {
                            TextBox textBox = new TextBox
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                TextWrapping = TextWrapping.Wrap
                            };
                            SetColumn(textBox, i);
                            SetRow(textBox, j);
                            Children.Add(textBox);
                        }
                    }
                }
                else if (value < Columns)
                {
                    var elements = Children.Where(el =>
                        GetColumn((TextBox)el) > value - 1).ToList();
                    foreach (var el in elements)
                    {
                        Children.Remove(el);
                    }

                    int col = Columns;
                    for (int i = value; i <= col; i++)
                    {
                        ColumnDefinitions.Remove(ColumnDefinitions[i]);
                    }
                }
                //OnPropertyChanged();
                SetValue(ColumnsProperty, ColumnDefinitions.Count);
            }
        }
        public int Rows
        {
            get => RowDefinitions.Count;
            set
            {
                if (value <= 0)
                    value = 1;
                if (value > Rows)
                    for (int i = Rows; i < value; i++)
                    {
                        RowDefinitions.Add(new RowDefinition());
                        for (int j = 0; j < Columns; j++)
                        {
                            TextBox textBox = new TextBox
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                TextWrapping = TextWrapping.Wrap
                            };
                            SetRow(textBox, i);
                            SetColumn(textBox, j);
                            Children.Add(textBox);
                        }
                    }
                else if (Rows > value)
                {
                    var elements = Children.Where(el =>
                        GetRow((TextBox)el) > value - 1).ToList();
                    foreach (var el in elements)
                    {
                        Children.Remove(el);
                    }

                    int row = Rows;
                    for (int i = value; i <= row; i++)
                    {
                        RowDefinitions.Remove(RowDefinitions[i]);
                    }
                }
                //OnPropertyChanged();
                SetValue(RowsProperty, RowDefinitions.Count);
            }
        }

        List<string> _data;
        public List<string> Data
        {
            get => _data; 
            set
            {
                _data = value;
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        ((TextBox)Children.First(it => GetColumn((FrameworkElement)it) == j && GetRow((FrameworkElement)it) == i)).Text = _data[i * Columns + j];
                    }
                }
            }
        }

        public static readonly DependencyProperty ColumnsProperty =
        DependencyProperty.Register(
        "Columns", typeof(Int32), typeof(VisualTable), new PropertyMetadata(1)
        );
        public static readonly DependencyProperty RowsProperty =
        DependencyProperty.Register(
        "Rows", typeof(Int32), typeof(VisualTable), new PropertyMetadata(1)
        );
        
        public Table LoadTableData()
        {
            var list = Children.ToList();
            //sorting list by rows and after that by columns
            list.Sort((el1, el2) =>
            {
                int row1 = GetRow((FrameworkElement)el1);
                int row2 = GetRow((FrameworkElement)el2);
                if (row1 == row2)
                    return GetColumn((FrameworkElement)el1).CompareTo(GetColumn((FrameworkElement)el2));
                return row1.CompareTo(row2);
            });
            var table = new Table();
            table.Fields = list.Select(it => ((TextBox)it).Text).ToList();
            table.Columns = Columns;
            table.Rows = Rows;
            return table;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
