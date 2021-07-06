using System;
//using Windows.UI;
using System.Drawing;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace App1
{
    public class RarityConvector: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Enum.TryParse((string)value, out MagicItem.ItemQuality quality);
            if(parameter != null && parameter.ToString() == "Color")
            switch (quality)
            {
                case MagicItem.ItemQuality.common:
                    return new SolidColorBrush(ColorToColorWTF(Color.Black));

                case MagicItem.ItemQuality.uncommon:
                    return new SolidColorBrush(ColorToColorWTF(Color.MediumSpringGreen));
                case MagicItem.ItemQuality.rare:
                    return new SolidColorBrush(ColorToColorWTF(Color.BlueViolet));
                case MagicItem.ItemQuality.very_rare:
                    return new SolidColorBrush(ColorToColorWTF(Color.Magenta));
                case MagicItem.ItemQuality.legendary:
                    return new SolidColorBrush(ColorToColorWTF(Color.Goldenrod));
                default:
                    return new SolidColorBrush(ColorToColorWTF(Color.Black));

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
