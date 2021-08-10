namespace App1
{
    public class MagicItem
    {
        public int Quality { set; get; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { set; get; }
        public string Attunement { get; set; }

        public MagicItem(int id, string name, int quality, string type, bool attunement)
        {
            Id = id;
            Name = name;
            Quality = quality;
            Type = type;
            Attunement = attunement ? "(Настройка)" : "";
        }

        public MagicItem()
        {
        }
    }
}