using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Model
{
    internal class ExtendedMonster:Monster
    {
        public int AC { get; set; }
        public string HP { get; set; }
        public string Speed { get; set; }
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Con { get; set; }
        public int Intel { get; set; }
        public int Wis { get; set; }
        public int Cha { get; set; }
        public string Skills { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Description { get; set; }
        public string LairActions { get; set; }
        public string RegionalEf { get; set; }
        public string UnderType { get; set; }
        public string WorldView { get; set; }
        public string ACType { get; set; }
        public List<Feautes> Actions;
        public List<Feautes> Feautes;
        public List<Feautes> LegendaryActions;

        public ExtendedMonster(int id)
        {

        }
    }
}
