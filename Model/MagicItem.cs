
namespace Model
{
    public class MagicItem : DataItem
    {
        public int Quality { set; get; }
        public string Type { set; get; }
        public string Attunement { get; set; }
        public string ItemSource { set; get; }

        public MagicItem(int id, string name, int quality, string type, bool attunement) : this()
        {
            Id = id;
            Name = name;
            Quality = quality;
            Type = type;
            Attunement = attunement ? "(Настройка)" : "";
        }

        public MagicItem()
        {
            ItemType = DataType.MagicItem;
            Type = "Чудесный предмет";
            ItemSource = "Homebrew";
        }
    }
}