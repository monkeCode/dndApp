using System;
using Windows.UI.Xaml.Data;

namespace App
{
    internal class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
                return !string.IsNullOrEmpty(value.ToString());
            var val = int.Parse(value.ToString());
            return val != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
