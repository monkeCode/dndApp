using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace Model
{
    public class Encounter : INotifyPropertyChanged
    {

        public ObservableCollection<BattleMonster> Monsters { get; } = new();
        
        public string Name { get; set; }
        public int Id { get; set; }

        private string _difficulty = "";
        
        private int _offset;
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
        public int Deadly { get; set; }
        private int _hard;
        private int _medium;
        private int _easy;
        
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
       
        public int GroupId { get; set; }
        private void Monsters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (BattleMonster i in e.NewItems)
                    i.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args) { if (args.PropertyName == nameof(BattleMonster.Quantity)) MonstersChanged((sender as BattleMonster)); };

            SetDifficulty();
        }


        public Encounter()
        {
            Monsters.CollectionChanged += Monsters_CollectionChanged;
        }

        public void CalculateForGroup(Group playerGroup)
        {
            _easy = playerGroup.Easy;
            _medium = playerGroup.Medium;
            _hard = playerGroup.Hard;
            Deadly = playerGroup.Deadly;
            if (playerGroup.Players.Count < 3)
               _offset = 1;
            else if (playerGroup.Players.Count > 5)
                _offset = -1;
            else
               _offset = 0;
        }
        
        private void MonstersChanged(BattleMonster monster)
        {
            if (monster.Quantity < 1)
            {
                Monsters.Remove(monster);
            }
            SetDifficulty();
        }
        public void SetDifficulty()
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
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
