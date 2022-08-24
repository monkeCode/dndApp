namespace App.Model
{
    internal class Spell : DataItem
    {
        public enum Component { Verbal, Somatic, Material }

        public Component[] Components { get; set; }

        public Spell()
        {
            ItemType = DataType.Spell;
        }

        public Spell(int id) : this()
        {
        }
    }
}
