using App.Annotations;
using App.Directories;
using DataBaseLib;
using System;
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

        private int _exModifierOffset = 0;
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
                PlayerGroup = DataBaseContext.Instance.GetGroups().First(g => g.Id == groupId);
                if (_playerGroup.Players.Count < 3)
                    _exModifierOffset = 1;
                else if (_playerGroup.Players.Count > 5)
                    _exModifierOffset = -1;
                else
                    _exModifierOffset = 0;
                SetDailyEx();
                Encounters.Clear();
                foreach (var item in DataAccess.GetData("Encounters", $"Group_id = {groupId}", null, "*"))
                {
                    Encounter e = new Encounter((int)(long)item[2], item[1].ToString(), _exModifierOffset, _playerGroup);
                    e.DeleteEvent += DeleteElement;
                    Encounters.Add(e);

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

        private void SaveEncounters()
        {
            string req = "";
            foreach (var E in Encounters)
            {

                req += E.SaveData() + ";";
            }
            if (req != "")
                // DataAccess.RawRequest(req);
                DataAccess.RawRequestAsync(req);
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
            DataAccess.AddData("Encounters", new[] { "Group_id" }, new[] { (object)_playerGroup.Id });
            foreach (var item in DataAccess.GetData("Encounters", $"Group_id = {_playerGroup.Id}", null, "*"))
            {
                if (Encounters.FirstOrDefault(obj => obj.Id == (int)(long)item[2]) == null)
                {
                    Encounter e = new Encounter((int)(long)item[2], item[1].ToString(), _exModifierOffset,
                        _playerGroup);
                    Encounters.Add(e);
                    e.DeleteEvent += DeleteElement;

                }
            }
        }

        private void DeleteElement(int id)
        {
            Encounters.Remove(Encounters.First(obj => obj.Id == id));
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
