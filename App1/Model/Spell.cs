using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Model
{
    internal class Spell:DataItem
    {
        public Spell()
        {
            ItemType = DataType.Spell;
        }
        public Spell(int id):this()
        {

        }
    }
}
