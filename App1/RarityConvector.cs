using System;
//using Windows.UI;
using System.Drawing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace App1
{
    public class RarityConvector: IValueConverter
    {
        ResourceDictionary myResourceDictionary = new ResourceDictionary();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           myResourceDictionary.Source =  new Uri("ms-appx:///Dictionary.xaml");
            Enum.TryParse((string)value, out MagicItem.ItemQuality quality);
            if(parameter != null && parameter.ToString() == "Color")
            switch (quality)
            {
                case MagicItem.ItemQuality.common:
                    return myResourceDictionary["Common"];

                case MagicItem.ItemQuality.uncommon:
                    return myResourceDictionary["Uncommon"];
                case MagicItem.ItemQuality.rare:
                    return myResourceDictionary["Rare"];
                case MagicItem.ItemQuality.very_rare:
                    return myResourceDictionary["VeryRare"];
                case MagicItem.ItemQuality.legendary:
                    return myResourceDictionary["Legendary"];
                default:
                    return myResourceDictionary["Variant"];

            }
            else
            {
                switch (quality)
                {
                    case MagicItem.ItemQuality.common:
                        return "Обычное";
                    case MagicItem.ItemQuality.uncommon:
                        return "Необычное";
                    case MagicItem.ItemQuality.rare:
                        return "Редкое";
                    case MagicItem.ItemQuality.very_rare:
                        return "Крайне редкое";
                    case MagicItem.ItemQuality.legendary:
                        return "Легендарное";
                    default:
                        return "Варьируется";
                }

            }    
        }

        static Windows.UI.Color ColorToColorWTF(Color color)
        {
            return Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
