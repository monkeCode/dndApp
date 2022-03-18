using System;

//using Windows.UI;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace App1
{
    public class RarityConvector : IValueConverter
    {
        private ResourceDictionary myResourceDictionary = new ResourceDictionary();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            myResourceDictionary.Source = new Uri("ms-appx:///Dictionary.xaml");
            //Enum.TryParse((string)value, out MagicItem.ItemQuality quality);
            int quality = (int)value;
            if (parameter != null && parameter.ToString() == "Color")
                switch (quality)
                {
                    case 0:
                        return myResourceDictionary["Common"];

                    case 1:
                        return myResourceDictionary["Uncommon"];

                    case 2:
                        return myResourceDictionary["Rare"];

                    case 3:
                        return myResourceDictionary["VeryRare"];

                    case 4:
                        return myResourceDictionary["Legendary"];

                    case 5:
                        return myResourceDictionary["Artifact"];

                    case 6:
                        return myResourceDictionary["Variant"];

                    default:
                        return myResourceDictionary["Common"];
                }
            return StaticValues.magicItemQality.First(obj => obj.Value == quality).Key;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}