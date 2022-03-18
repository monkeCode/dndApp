using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace App1
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return StaticValues.monsterSize.First(obj => obj.Value == (int)value).Key;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}