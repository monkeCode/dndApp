using System.Collections.Generic;
using System.Linq;

namespace App.Model
{
    public class DataItem
    {
        public enum DataType
        {
            MagicItem, Monster, Spell
        }
        public string Name { get; set; }
        public DataType ItemType { get; set; }
        public int Id { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public static ICollection<DataItem> GetItems(string name = null)
        {
            List<DataItem> dataItems = new List<DataItem>();
            var request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from Monsters");
            foreach (var it in request)
            {
                dataItems.Add(new DataItem
                {
                    Id = (int)(long)it[0],
                    Name = it[1].ToString(),
                    ItemType = DataItem.DataType.Monster
                });
            }
            request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from MagicItems");
            foreach (var it in request)
            {
                dataItems.Add(new DataItem
                {
                    Id = (int)(long)it[0],
                    Name = it[1].ToString(),
                    ItemType = DataItem.DataType.MagicItem
                });
            }
            request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from Spells");
            foreach (var it in request)
            {
                dataItems.Add(new DataItem
                {
                    Id = (int)(long)it[0],
                    Name = it[1].ToString(),
                    ItemType = DataItem.DataType.Spell
                });
            }
            if (name != null)
                return dataItems.Where(it => it.Name.ToLower().Contains(name)).OrderByDescending(it => it.Name).ToList();
            return dataItems;
        }
    }
}
