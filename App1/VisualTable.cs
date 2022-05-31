using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using App1.Annotations;

namespace App
{
    public class VisualTable:Grid, INotifyPropertyChanged
    {
        public int Columns { 
            get => ColumnDefinitions.Count;
            set
            {
                if (value <= 0)
                    value = 1;
                if (value > Columns)
                {
                    for (int i = Columns; i < value; i++)
                    {
                        ColumnDefinitions.Add(new ColumnDefinition { MinWidth = 150, MaxWidth = 300});
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
                else if(value < Columns)
                {
                    var elements = Children.Where(el =>
                        GetColumn((TextBox) el) > value - 1).ToList();
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
                OnPropertyChanged();
            } }

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
                else if(Rows > value)
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
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
