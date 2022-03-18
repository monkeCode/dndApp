using App1.Annotations;
using DataBaseLib;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App1.Encounters
{
    class Encounter : INotifyPropertyChanged
    {

        public ObservableCollection<BattleMonster> Monsters { get; } = new();
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (!string.IsNullOrEmpty(_name.Trim()))
                    DataAccess.RawRequest($"UPDATE Encounters SET Name = \'{_name}\' WHERE _Id = {Id}").Close();
                else
                {
                    DataAccess.RawRequest($"UPDATE Encounters SET Name = \'Боевая сцена\' WHERE _Id = {Id}").Close();
                }
            }
        }
        public int Id { get; set; }
        private string _difficulty = "";
        private readonly int _offset;
        public string Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged();
            }
        }

        private int _totalEx;
        public int TotalEx
        {
            get => _totalEx;
            set
            {
                _totalEx = value;
                OnPropertyChanged();
            }
        }

        private int _adaptEx;
        public int AdaptEx
        {
            get => _adaptEx;
            set
            {
                _adaptEx = value;
                OnPropertyChanged();
            }
        }
        public int Deadly { get; }
        private readonly int _hard;
        private readonly int _medium;
        private readonly int _easy;

        private float _modificator;
        public float Modificator
        {
            get => _modificator;
            set
            {
                _modificator = value;
                OnPropertyChanged();
            }
        }
        public Encounter(int id, string name, int offset, in Group group)
        {
            Monsters.CollectionChanged += Monsters_CollectionChanged;
            _offset = offset;
            _name = name;
            Id = id;
            _easy = group.Easy;
            _medium = group.Medium;
            _hard = group.Hard;
            Deadly = group.Deadly;
            //иницилизация монстров из бд
            foreach (var item in DataAccess.GetData("Monsters", $"_id IN (SELECT EncountersToMonsters.Monster_Id FROM EncountersToMonsters WHERE Encounter_Id = {id} )", null, "*"))
            {
                int MonsterId = (int)(long)item[0];
                Monsters.Add(new BattleMonster()
                {
                    Monster = new Monster()
                    {
                        Id = MonsterId,
                        Name = item[1].ToString(),
                        Size = (int)(long)item[2],
                        Type = item[3].ToString(),
                        Challenge = item[5].ToString()
                    },
                    Quantity = (int)(long)DataAccess.GetData("EncountersToMonsters", $"Encounter_Id = {id} AND Monster_Id = {MonsterId}", null, "Monster_Quantity")[0][0]

                });

            }
            //обновление сложности
            //SetDificulty();
        }
        private void Monsters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (BattleMonster i in e.NewItems)
                    i.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args) { if (args.PropertyName == "Quantity") MonstersChanged((sender as BattleMonster)); };

            SetDificulty();
        }

        private void MonstersChanged(BattleMonster monster)
        {
            if (monster.Quantity < 1)
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

            TotalEx = res;
            Modificator = DifficultyModificator(quantity);
            AdaptEx = (int)Math.Round(res * Modificator);
            if (AdaptEx < _easy)
            {
                Difficulty = "Нет угрозы";
            }
            else if (AdaptEx < _medium)
            {
                Difficulty = "Легко";
            }
            else if (AdaptEx < _hard)
            {
                Difficulty = "Средне";
            }
            else if (AdaptEx < Deadly)
            {
                Difficulty = "Трудно";
            }
            else
            {
                Difficulty = "Смертельно";
            }
        }

        private float DifficultyModificator(int quantity)
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
            return quantity switch
            {
                <= 1 => modificators[1 - _offset],
                2 => modificators[2 - _offset],
                >= 3 and <= 6 => modificators[3 - _offset],
                >= 7 and <= 10 => modificators[4 - _offset],
                >= 11 and <= 14 => modificators[5 - _offset],
                >= 15 => modificators[6 - _offset]

            };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string SaveData()
        {
            DataAccess.RawRequest($"DELETE FROM EncountersToMonsters WHERE Encounter_Id = {Id}");
            string values = "";
            if (Monsters.Count > 0)
            {
                foreach (var monster in Monsters)
                {
                    //  DataAccess.AddData("EncountersToMonsters", null, new object[] { Id, monster.Monster.Id, monster.Quantity });
                    values += $"({Id}, {monster.Monster.Id}, {monster.Quantity}), ";

                }

                values = values.Remove(values.Length - 2, 2);
                return $"INSERT INTO EncountersToMonsters VALUES {values}";

            }

            return "";
        }

        //ивент удаления из листа в viewmodel
        public event Action<int> DeleteEvent;
        public CustomCommand DeleteCommand
        {
            get => new CustomCommand(obj =>
            {
                DataAccess.RawRequest($"DELETE FROM Encounters WHERE _id = {Id}");
                DeleteEvent.Invoke(Id);
            });
        }
    }
}
