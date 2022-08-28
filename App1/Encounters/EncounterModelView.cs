using App.Annotations;
using App.Directories;
using DataBaseLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Model;

namespace App.Encounters
{
    class EncounterModelView : INotifyPropertyChanged, IDisposable
    {
        public MonsterModelView MonsterModel { get; } = new();
        public ObservableCollection<BattleMonster> MonsterList { get; private set; } = new();
        public ObservableCollection<Encounter> Encounters { get; set; } = new();

        private Group _playerGroup;
        public Group PlayerGroup
        {
            get { return _playerGroup; }
            set { _playerGroup = value; OnPropertyChanged(); }
        }
        
        private int _dailyEx;
        private int _totalMonstersEx;
        public int TotalMonstersEx
        {
            get => _totalMonstersEx;
            set { _totalMonstersEx = value; OnPropertyChanged(); }
        }
        public int DailyEx
        {
            get => _dailyEx;
            set
            {
                _dailyEx = value;
                OnPropertyChanged();
            }

        }

        public EncounterModelView()
        {
            // TODO: load active group
            ChangeGroup(2);
            GetMonsterData();
        }

        public void GetMonsterData()
        {
            MonsterModel.GetListData();
            MonsterList.Clear();
            foreach (var monster in MonsterModel.DataCollection)
            {
                MonsterList.Add(new BattleMonster
                {
                    Monster = monster,
                    Quantity = 1
                });
            }
        }
        public void SelectionEncountersChanged(IEnumerator<object> enumerator)
        {
            int res = 0;
            while (enumerator.MoveNext())
            {
                res += (enumerator.Current as Encounter).AdaptEx;

            }

            TotalMonstersEx = res;
        }
        public void ChangeGroup(int groupId)
        {
            try
            {
                SaveEncounters();
            }
            finally
            {
                PlayerGroup = App.DataContext.GetGroups().First(g => g.Id == groupId);
                SetDailyEx();
                Encounters.Clear();
                foreach (var encounter in App.DataContext.GetEncounters().Where(enc => enc.GroupId == groupId))
                {
                    Encounters.Add(encounter);
                    encounter.CalculateForGroup(PlayerGroup);
                }
            }
        }

        private void SetDailyEx()
        {
            int res = 0;
            foreach (Player player in _playerGroup.Players)
            {
                res += player.Lvl switch
                {
                    1 => 300,
                    2 => 600,
                    3 => 1200,
                    4 => 1700,
                    5 => 3500,
                    6 => 4000,
                    7 => 5000,
                    8 => 6000,
                    9 => 7500,
                    10 => 9000,
                    11 => 10500,
                    12 => 11500,
                    13 => 13500,
                    14 => 15000,
                    15 => 18000,
                    16 => 20000,
                    17 => 25000,
                    18 => 27000,
                    19 => 30000,
                    20 => 40000,
                    _ => 0
                };
            }

            DailyEx = res;
        }

        private async void SaveEncounters()
        {
            //var encList = App.DataContext.GetEncounters().Where(en => en.GroupId == PlayerGroup.Id).ToList();
            foreach (var encounter in Encounters)
            {

                if (encounter.Name == null) encounter.Name = "Боевая сцена";
                if(encounter.Id == -1)
                    await App.DataContext.AddEncounter(encounter);
                else
                    await App.DataContext.UpdateEncounter(encounter);
            }

            //foreach (var en in encList.Where(it => Encounters.Count(e => e.Id == it.Id) == 0))
            //{
            //    App.DataContext.DeleteEncounter(en.Id);
            //}
        }


        CustomCommand _addNewEncounter;
        public CustomCommand AddEncounterCommand
        {
            get
            {
                return _addNewEncounter ??= new CustomCommand(obj => { AddEncounter(); }, _ => true);
            }
        }
        private void AddEncounter()
        {
            var enc = new Encounter()
            {
                GroupId = PlayerGroup.Id,
                Id = -1
            };
            Encounters.Add(enc);
        }

        public void DeleteEncounter(Encounter en)
        {
            Encounters.Remove(en);
            if (en.Id != -1)
                App.DataContext.DeleteEncounter(en.Id);
        }
        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            SaveEncounters();
        }
    }
}
