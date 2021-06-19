using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
   public class MagicItem
    {
        public enum ItemQuality { common, uncommon, rare, very_rare, legendary, varies }
        public int Id { set; get; }
        public string Name { set; get; }
        public string Quality { set; get; }
        public string Type { set; get; }
        public MagicItem(int id, string name, ItemQuality quality, string type)
        {
            this.Id = id;
            this.Name = name;
            this.Quality = quality.ToString();
            this.Type = type;
        }
        public MagicItem()
        {
           
        }

    }
}
