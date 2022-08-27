
namespace Model
{
    public class DataItem
    {
        public enum DataType
        {
            MagicItem, Monster, Spell
        }
        public string Name { get; set; } = "";
        public DataType ItemType { get; set; }
        public int Id { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
