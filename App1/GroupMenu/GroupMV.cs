using Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace App.GroupMenu
{
    class GroupMV
    {
        public ObservableCollection<Player> PlayerWithoutGroup { get; } = new ObservableCollection<Player>();
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();

        public void AddPlayer(Player p)
        {
            PlayerWithoutGroup.Add(p);
        }

        public void UpdatePlayer(Player p)
        {
            //Groups.SelectMany(g => g.Players).FirstOrDefault(player => player.Id == p.Id)
        }

        public GroupMV()
        {
            foreach (var group in App.DataContext.GetGroups())
            {
                Groups.Add(group);
            }

            foreach (var player in App.DataContext.GetPlayers().Where(p => p.GroupId == -1))
            {
                PlayerWithoutGroup.Add(player);
            }


        }
    }
}
