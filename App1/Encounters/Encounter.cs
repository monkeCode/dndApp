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
using Windows.UI.Xaml.Controls;

namespace App1.Encounters
{
    class Encounter:INotifyPropertyChanged
    {
        
        public ObservableCollection<BattleMonster> Monsters { get;} = new ObservableCollection<BattleMonster>();
        public string Name { get; set; }
        public int Id { get; set; }
        private string _difficulty = "";
        private float _exModifier = 1;
        private readonly int _offcet = 0;
        public int[] Difficults { get; set; } = new int[4];
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
        public int Deadly { get; private set; }
        private int _hard;
        private int _medium;
        private int _easy;

        public Encounter(int id, string name, int offset, in Group group)
        {
            _offcet = offset;
            Monsters.CollectionChanged += Monsters_CollectionChanged;
            Name = name;
            Id = id;
            _easy = group.Easy;
            _medium = group.Medium;
            _hard = group.Hard;
            Deadly = group.Deadly;
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
            foreach (var monster in Monsters)
            SetDificulty();
        }
        private void Monsters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            foreach(BattleMonster i in e.NewItems)
                i.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args) { if (args.PropertyName == "Quantity") MonstersChanged((sender as BattleMonster)); };

            SetDificulty();
        }

        private void MonstersChanged(BattleMonster monster)
        {
            if(monster.Quantity <1)
            {
                Monsters.Remove(monster);
            }
            SetDificulty();
        }
        public void SetDificulty()
        {
            int res = 0;
            int quantity = 0;
            foreach (var monster in Monsters)
            {
                res += monster.Quantity * monster.Monster.Ex;
                quantity += monster.Quantity;
            }
            DifficultyModificator(quantity);
            TotalEx = (int)Math.Round(res * _exModifier);
            if (TotalEx < _easy)
            {
                Difficulty = "Нет угрозы";
            }
            else if(TotalEx < _medium)
            {
                Difficulty = "Легко";
            }
            else if(TotalEx < _hard)
            {
                Difficulty = "Средне";
            }
            else if(TotalEx < Deadly)
            {
                Difficulty = "Сложно";
            }
            else
            {
                Difficulty = "Смертельно";
            }
        }

        private void DifficultyModificator(int quantity)
        {
            float[] modificators = new float[8]
            {
                0.5f,
                1,
                1.5f,
                2,
                2.5f,
                3,
                4,
                5
            };
            _exModifier = quantity switch
            {
                <=1 => modificators[1 - _offcet],
                2 => modificators[2 - _offcet],
                >= 3 and <= 6 => modificators[3 - _offcet],
                >= 7 and <= 10 => modificators[4 - _offcet],
                >= 11 and <= 14 => modificators[5 - _offcet],
                >= 15 => modificators[6 - _offcet]

            };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
    public class BattleMonster:INotifyPropertyChanged
    { 
        public Monster Monster { get; set; }
        private int quantity;
        public int Quantity { get => quantity; set { quantity = value > 99 ? 99 : value; OnPropertyChanged(); } }

        CustomCommand positiveCommand;
       public CustomCommand PositiveCommand { 
            get {
                if(positiveCommand == null)
                {
                    positiveCommand = new CustomCommand(delegate (object obj) { Quantity++;  }, delegate (object obj) { return true; });
                }
                return positiveCommand;
                } }
        CustomCommand negativeCommand;
        public CustomCommand NegativeCommand {
            get 
            {
                if (negativeCommand == null)
                {
                    negativeCommand = new CustomCommand(delegate (object obj) { Quantity--; }, delegate (object obj) { return true; });
                }
                return negativeCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
