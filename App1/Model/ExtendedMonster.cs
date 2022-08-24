using DataBaseLib;
using System;
using System.Collections.ObjectModel;

namespace App1.Model
{
    internal class ExtendedMonster : Monster
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
        public ObservableCollection<Features> Actions { get; set; } = new();
        public ObservableCollection<Features> ReciprocalActions { get; set; } = new();
        public ObservableCollection<Features> Features { get; set; } = new();
        public ObservableCollection<Features> LegendaryActions { get; set; } = new();
        public Table Table { get; set; }

        public ExtendedMonster(int id) : base(id)
        {
            var list = DataAccess.GetData("ExtendedMonsters", $"_id = {id}", null, "*")[0];
            AC = (int)(long)list[1];
            HP = list[2].ToString();
            Speed = list[3].ToString();
            Str = (int)(long)list[4];
            Dex = (int)(long)list[5];
            Con = (int)(long)list[6];
            Intel = (int)(long)list[7];
            Wis = (int)(long)list[8];
            Cha = (int)(long)list[9];
            SavingThrows = list[10].ToString();
            Skills = list[11].ToString();
            Senses = list[12].ToString();
            Languages = list[13].ToString();
            Description = list[14].ToString();
            LairActions = list[15].ToString();
            RegionalEf = list[16].ToString();
            UnderType = list[17].ToString();
            WorldView = list[18].ToString();
            ACType = list[19].ToString();
            Immunity = list[20].ToString();
            Resistance = list[21].ToString();
            Vulnerability = list[22].ToString();
            ImmunityState = list[23].ToString();

            foreach (var act in DataAccess.GetData("MonsterActions", $"_id = {id}", null, "Name, Description"))
            {
                Actions.Add(new Features()
                {
                    Name = act[0].ToString(),
                    Description = act[1].ToString()
                });
            }
            foreach (var act in DataAccess.GetData("MonsterFeatures", $"_id = {id}", null, "Name, Description"))
            {
                Features.Add(new Features()
                {
                    Name = act[0].ToString(),
                    Description = act[1].ToString()
                });
            }
            foreach (var act in DataAccess.GetData("MonsterReciprocalActions", $"_id = {id}", null, "Name, Description"))
            {
                ReciprocalActions.Add(new Features()
                {
                    Name = act[0].ToString(),
                    Description = act[1].ToString()
                });
            }
            foreach (var act in DataAccess.GetData("MonsterLegendaryActions", $"_id = {id}", null, "Name, Description"))
            {
                LegendaryActions.Add(new Features()
                {
                    Name = act[0].ToString(),
                    Description = act[1].ToString()
                });
            }

        }

        private void LoadTableFromDb(int id)
        {
            try
            {
                var data = DataAccess.GetData("TablesMonsters", $"ParentId = {id}", null, "*");
                Table = new Table(data[0]);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        public ExtendedMonster()
        {
        }
    }
}
