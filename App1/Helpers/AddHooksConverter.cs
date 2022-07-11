using System;
using Windows.UI.Xaml.Data;

namespace App.Helpers;

public class AddHooksConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(string.IsNullOrEmpty(value?.ToString()))
            return "";
        return "(" + value.ToString() + ")";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}