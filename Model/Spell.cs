using System.Collections.Generic;

namespace Model
{
    public class Spell : DataItem
    {
        public enum Component { Verbal = 1, Somatic = 2, Material = 3 }

        public List<Component> Components { get; set; }

        public int Lvl { get; set; }
        public string School { get; set; }

        public bool Concentration { get; set; }

        public string Source { get; set; }

        public Spell()
        {
            ItemType = DataType.Spell;
        }

        public void SetComponents(string data)
        {
            if (string.IsNullOrEmpty(data)) return;
            Components = new List<Component>();
            foreach (var c in data)
            {
                var i = int.Parse(c.ToString());
                Components.Add((Component)i);
            }
        }

    }
}
