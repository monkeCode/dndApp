using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Model
{
    public class Group
    {
        private static readonly int[,] _difficultyList = new int[20, 4]
        {
            { 25,  50,  75,  100 },
            { 50,  100, 150, 200 },
            {75,  150, 225, 400 },
            {125, 250, 375, 500 },
            { 250, 500, 750, 1100 },
            { 300, 600, 900, 1400 },
            { 350, 750, 1100, 1700 },
            { 450, 900, 1400,    2100 },
            { 550, 1100,    1600,    2400 },
            { 600, 1200,    1900,    2800 },
            { 800, 1600,    2400,    3600 },
            { 1000,    2000,    3000,    4500 },
            { 1100,    2200,    3400,    5100 },
            { 1250,    2500,    3800,    5700 },
            { 1400,    2800,    4300,    6400 },
            { 1600,    3200,    4800,    7200 },
            { 2000,    3900,    5900,    8800 },
            { 2100,    4200,    6300,    9500 },
            { 2400,    4900 ,   7300,    10900 },
            { 2800,    5700,    8500,    12700 }
        };

        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Player> Players { get; } = new();
        public int Deadly { get; private set; }
        public int Hard { get; private set; }
        public int Medium { get; private set; }
        public int Easy { get; private set; }

        public Group(int id, string name, ICollection<Player> players) : base()
        {
            Id = id;
            Name = name;
            foreach (var player in players)
            {
                Players.Add(player);
            }
        }

        public Group()
        {
            Players.CollectionChanged += (sender, args) => CalculateDificulty();
        }

        private void CalculateDificulty()
        {
            foreach (var player in Players)
            {
                Easy += _difficultyList[player.Lvl - 1, 0];
                Medium += _difficultyList[player.Lvl - 1, 1];
                Hard += _difficultyList[player.Lvl - 1, 2];
                Deadly += _difficultyList[player.Lvl - 1, 3];
            }
        }
    }
}