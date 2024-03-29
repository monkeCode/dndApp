﻿using System;
using Windows.UI.Xaml.Data;

namespace App.Helpers
{
    internal class AttunementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !string.IsNullOrEmpty(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool val)
            {
                return val ? "(Настройка)" : "";
            }

            return "";
        }
    }
}
