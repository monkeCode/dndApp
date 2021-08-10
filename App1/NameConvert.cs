using System;
using Windows.UI.Xaml.Data;

namespace App1
{
    internal class NameConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string text = value as string;
            if (!string.IsNullOrEmpty(text))
            {
                return text + ":";
            }
            else return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}