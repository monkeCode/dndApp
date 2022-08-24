
using System.Collections.ObjectModel;

namespace Model
{
    public class ExtendedMonster : Monster
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
        public string SavingThrows { get; set; }
        public string Skills { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Description { get; set; }
        public string LairActions { get; set; }
        public string RegionalEf { get; set; }
        public string UnderType { get; set; }
        public string WorldView { get; set; }
        public string ACType { get; set; }
        public string Immunity { get; set; }
        public string Resistance { get; set; }
        public string Vulnerability { get; set; }
        public string ImmunityState { get; set; }
        public ObservableCollection<Feature> Actions { get; set; } = new();
        public ObservableCollection<Feature> ReciprocalActions { get; set; } = new();
        public ObservableCollection<Feature> Features { get; set; } = new();
        public ObservableCollection<Feature> LegendaryActions { get; set; } = new();
        public Table Table { get; set; }
    }
}
