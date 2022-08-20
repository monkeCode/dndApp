using System;
using Windows.UI.Xaml.Data;

namespace App
{

    public class StatsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int val = int.Parse(value.ToString());
            int mod = (val - 10) / 2;
            return $"{val}({(mod >= 0 ? "+" : "")}{mod})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}