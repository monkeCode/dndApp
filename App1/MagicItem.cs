namespace App1
{
    public class MagicItem
    {
        public enum ItemQuality { common, uncommon, rare, very_rare, legendary, varies }

        public int Id { set; get; }
        public string Name { set; get; }
        public string Quality { set; get; }
        public string Type { set; get; }
        public string Attunement { get; set; }

        public MagicItem(int id, string name, ItemQuality quality, string type, string attunement)
        {
            this.Id = id;
            this.Name = name;
            this.Quality = quality.ToString();
            this.Type = type;
            Attunement = attunement !="0" ? "(Настройка)" : "";

        }

        public MagicItem()
        {
        }
    }
}