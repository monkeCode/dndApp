using System.Collections.ObjectModel;

namespace Model
{
    public class ExtendedMagicItem : MagicItem
    {
        public ObservableCollection<Feature> Features { get; set; } = new ObservableCollection<Feature>();
        public string Description { get; set; }
        public string UnderType { get; set; }
        public string UnderQuality { get; set; }
        public string OptionableText { get; set; }

        public Table Table { get; set; }


        public ExtendedMagicItem()
        {

        }

    }
}