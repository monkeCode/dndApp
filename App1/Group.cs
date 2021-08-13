using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseLib;

namespace App1
{
    class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Player> Players { get;} = new();

       public Group(int id)
        {
            Id = id;
            Name = DataAccess.GetData("Parties", $"_id = {id}", null, "*")[0][1].ToString();
            foreach (object[] player in DataAccess.GetData("Players", $"Group_Id = {id}", null, "*"))
            {
                Players.Add(new Player
                {
                    Name = player[1].ToString(),
                    PlayerName = player[2].ToString(),
                    Class = player[3].ToString(),
                    Lvl = (int)(long)player[4]
                });
            }
        }
    }

    internal class Player
    {
        public string Name { get; set; }
        public string PlayerName { get; set; }
        public string Class { get; set; }
        public int Lvl { get; set; }
        
    }
}
