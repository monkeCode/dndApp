using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using App1.Annotations;
using DataBaseLib;
using Microsoft.Graphics.Canvas.Svg;

namespace App1.Encounters
{
    class Encounter:INotifyPropertyChanged
    {
        
        public ObservableCollection<BattleMonster> Monsters { get;} = new ObservableCollection<BattleMonster>();
        public string Name { get; set; }
        public int Id { get; set; }
        private string _difficulty = "";

        public string Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged();
            }
        }

        private int _totalEx = 0;
        public int TotalEx
        {
            get => _totalEx;
            set
            {
                _totalEx = value;
                OnPropertyChanged();
            }
        }
        public Encounter(int id, string name)
        {
            Name = name;
            Id = id;
            foreach (var item in DataAccess.GetData("Monsters", $"_id IN (SELECT EncountersToMonsters.Monster_Id FROM EncountersToMonsters WHERE Encounter_Id = {id} )",null,"*"))
            {
                int MonsterId = (int) (long) item[0];
                Monsters.Add(new BattleMonster()
                {
                    Monster = new Monster()
                    {
                        Id =MonsterId,
                        Name = item[1].ToString(),
                        Size = (int)(long)item[2],
                        Type = item[3].ToString(),
                        Challenge = item[5].ToString()
                    },
                    Quantity = (int)(long)DataAccess.GetData("EncountersToMonsters", $"Encounter_Id = {id} AND Monster_Id = {MonsterId}",null, "Monster_Quantity")[0][0]

                });

            }
            SetDificulty();
        }

        public void SetDificulty()
        {
            int res = 0;
            foreach (var monster in Monsters)
            {
                res += monster.Quantity * monster.Monster.Ex;
            }

            TotalEx = res;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class BattleMonster
    {
        public Monster Monster { get; set; }
        public int Quantity { get; set; }
    }
}
