using System;

namespace App1
{
    public class MagicItem
    {
        public enum ItemQuality { common, uncommon, rare, very_rare, legendary, varies }

        public string Quality { set; get; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { set; get; }
        public string Attunement { get; set; }

        public MagicItem(int id, string name, ItemQuality quality, string type, bool attunement)
        {
            Id = id;
            Name = name;
            Quality = quality.ToString();
            Type = type;
            Attunement = attunement ? "(Настройка)" : "";
        }

        public MagicItem()
        {
        }
    }
}