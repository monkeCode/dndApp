using System;
using Windows.UI.Xaml.Data;

namespace App
{

    public class StatsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int val = int.Parse(value.ToString());
            int mod = (int)Math.Floor((val - 10) / 2.0f);
            return $"{val}({(mod >= 0 ? "+" : "")}{mod})";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}