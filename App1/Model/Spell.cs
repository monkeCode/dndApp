namespace App.Model
{
    internal class Spell : DataItem
    {
        public Spell()
        {
            ItemType = DataType.Spell;
        }
        public Spell(int id) : this()
        {

        }
    }
}
