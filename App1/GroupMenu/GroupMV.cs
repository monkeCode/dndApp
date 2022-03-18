using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.GroupMenu
{
    class GroupMV
    {
        public ObservableCollection<Player> PlayerWithoutGroup { get; } = new ObservableCollection<Player>();
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
       public GroupMV()
       {
            foreach(var groups in DataBaseLib.DataAccess.GetData("SELECT _id FROM Parties"))
            {
                Groups.Add(new Group((int)(long)groups[0]));
            }
            foreach (var p in DataBaseLib.DataAccess.GetData("SELECT Name,PlayerName,Class,Lvl From Players Where Group_id IS NULl"))
            {
                PlayerWithoutGroup.Add(new Player
                {
                    Name = (string)p[0],
                    PlayerName = (string)p[1],
                    Class = (string)p[2],
                    Lvl = (int)(long)p[3],
                });
            }
       }
    }
}
