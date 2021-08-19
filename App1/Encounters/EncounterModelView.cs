using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using App1.Annotations;
using App1.Directories;
using DataBaseLib;

namespace App1.Encounters
{
    class EncounterModelView : INotifyPropertyChanged
    {
        public MonsterModelView MonsterModel { get; } = new MonsterModelView();
        public ObservableCollection<BattleMonster> MonsterList { get; private set; } = new();
        public ObservableCollection<Encounter> Encounters { get; set; } = new ObservableCollection<Encounter>();

        private Group playerGroup;
        public Group PlayerGroup
        {
            get { return playerGroup; }
            set { playerGroup = value; OnPropertyChanged(); }
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
            ChangeGroup(2);
            GetMonsterData();
        }

        public void GetMonsterData()
        {
            MonsterModel.GetListData();
            MonsterList.Clear();
          foreach(var monster in  MonsterModel.DataCollection)
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
                res += (enumerator.Current as Encounter).TotalEx;

            }

            TotalMonstersEx = res;
        }
        public void ChangeGroup(int groupId)
        {
            PlayerGroup = new Group(groupId);
            if (playerGroup.Players.Count < 3)
                _exModifierOffset = 1;
            else if (playerGroup.Players.Count > 5)
                _exModifierOffset = -1;
            else
                _exModifierOffset = 0;
           SetDailyEx();
            foreach (var item in DataAccess.GetData("Encounters", $"Group_id = {groupId}", null, "*"))
            {
                Encounters.Add(new Encounter((int)(long)item[2], item[1].ToString(), _exModifierOffset, playerGroup));
            }
        }

        private void SetDailyEx()
        {
            int res = 0;
            foreach (Player player in playerGroup.Players)
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


        CustomCommand addNewEncounter;
        public CustomCommand AddEncounterCommand { get
            {
                if(addNewEncounter == null)
                {
                    addNewEncounter = new CustomCommand(obj => { AddEncounter(); }, obj => true);
                }
                return addNewEncounter;
            } }
        private void AddEncounter()
        {
            DataAccess.AddData("Encounters", new[] { "Group_id" }, new[] { (object)playerGroup.Id });
            foreach (var item in DataAccess.GetData("Encounters", $"Group_id = {playerGroup.Id}", null, "*"))
            {
                if(Encounters.FirstOrDefault(obj => obj.Id == (int)(long)item[2]) == null)
                Encounters.Add(new Encounter((int)(long)item[2], item[1].ToString(), _exModifierOffset, playerGroup));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
