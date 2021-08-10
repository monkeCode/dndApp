using System;

//using Windows.UI;
using System.Drawing;
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
            else
            {
                switch (quality)
                {
                    case 0:
                        return "Обычное";

                    case 1:
                        return "Необычное";

                    case 2:
                        return "Редкое";

                    case 3:
                        return "Крайне редкое";

                    case 4:
                        return "Легендарное";

                    default:
                        return "Варьируется";
                }
            }
        }

        private static Windows.UI.Color ColorToColorWTF(Color color)
        {
            return Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}