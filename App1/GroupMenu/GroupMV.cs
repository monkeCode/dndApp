using System;
using Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace App.GroupMenu
{
    class GroupMv
    {
        public ObservableCollection<Player> PlayerWithoutGroup { get; } = new();
        public ObservableCollection<Group> Groups { get; } = new();

        public async void AddPlayer(Player p)
        {
            p.GroupId = -1;
            await App.DataContext.AddPlayer(p);
            p = App.DataContext.GetPlayers().Last(player => p.Name == player.Name);
            PlayerWithoutGroup.Add(p);
        }

        public async void AddGroup(string name)
        {
            var group = new Group() { Name = name };
            await App.DataContext.AddGroup(group);
            group = App.DataContext.GetGroups().Last(it => it.Name == name);
            Groups.Add(group);
        }
        public async void UpdatePlayer(Player p)
        {
            if (p.GroupId != -1)
            {
                var group = Groups.First(it => it.Id == p.GroupId);
                var player = group.Players.First(it => it.Id == p.Id);
                var index = group.Players.IndexOf(player);
                group.Players.RemoveAt(index);
                group.Players.Insert(index, p);
                return;
            }
            else
            {
                var playerWithoutG = PlayerWithoutGroup.First(player => player.Id == p.Id);
                var i = PlayerWithoutGroup.IndexOf(playerWithoutG);
                PlayerWithoutGroup.RemoveAt(i);
                PlayerWithoutGroup.Insert(i, p);
            }
            await App.DataContext.UpdatePlayer(p);
        }

        public GroupMv()
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

        public async void DeletePlayer(Player player)
        {
            if (player.GroupId != -1)
                Groups.First(it => it.Id == player.GroupId).Players.Remove(player);
            else
                PlayerWithoutGroup.Remove(player);
            await App.DataContext.DeletePlayer(player.Id);
        }
    }
}
