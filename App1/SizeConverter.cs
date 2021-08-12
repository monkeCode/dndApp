using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace App1
{
    public class SizeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           return StaticValues.MonsterSize.First(obj => obj.Value == int.Parse(value.ToString())).Key;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}