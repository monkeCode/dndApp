using App.Model;
using DataBaseLib;

namespace App1
{
    public class MagicItem:DataItem
    {
        public int Quality { set; get; }
        public string Type { set; get; }
        public string Attunement { get; set; }

        public MagicItem(int id, string name, int quality, string type, bool attunement): this()
        {
            Id = id;
            Name = name;
            Quality = quality;
            Type = type;
            Attunement = attunement ? "(Настройка)" : "";
        }

        public MagicItem(int id):this()
        {
            var it = DataAccess.GetData("MagicItems", $"_id = {id}", "Name", "*")[0];
            Id = id;
            Name = it[1].ToString();
            Quality = (int) (long) it[2];
            Type = it[3].ToString();
            Attunement = it[4].ToString() != "0"?"(Настройка)" : "";
        }

        public MagicItem()
        {
            ItemType = DataType.MagicItem;
        }
    }
}