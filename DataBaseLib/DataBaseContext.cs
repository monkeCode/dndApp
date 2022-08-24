using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace DataBaseLib
{
    public class DataBaseContext : IDataContext
    {

        private static DataBaseContext _inst;
        public static DataBaseContext Instance => _inst ??= new DataBaseContext();
        private DataBaseContext() { }

        public IEnumerable<DataItem> GetDataItems()
        {
            var request = DataAccess.GetData("SELECT _id, Name from Monsters");
            List<DataItem> dataItems = request.Select(it => new DataItem { Id = (int)(long)it[0], Name = it[1].ToString(), ItemType = DataItem.DataType.Monster }).ToList();
            request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from MagicItems");
            dataItems.AddRange(request.Select(it => new DataItem { Id = (int)(long)it[0], Name = it[1].ToString(), ItemType = DataItem.DataType.MagicItem }));

            request = DataBaseLib.DataAccess.GetData("SELECT _id, Name from Spells");
            dataItems.AddRange(request.Select(it => new DataItem { Id = (int)(long)it[0], Name = it[1].ToString(), ItemType = DataItem.DataType.Spell }));

            return dataItems;
        }

        public IEnumerable<MagicItem> GetMagicItems()
        {
            var data = DataAccess.GetData("MagicItems", null, "Name", "*");
            List<MagicItem> items = new List<MagicItem>();
            foreach (var item in data)
            {
                items.Add(new MagicItem()
                {
                    Id = (int)(long)item[0],
                    Name = item[1].ToString(),
                    Quality = (int)(long)item[2],
                    Type = item[3].ToString(),
                    Attunement = item[4].ToString() != "0" ? "(Настройка)" : "",
                    ItemSource = item[5].ToString(),
                });
            }
            return items;
        }

        public ExtendedMagicItem GetExtendedMagicById(int id)
        {
            var data = DataAccess.GetData("MagicItems", $"_id = {id}", "Name", "*")[0];
            var item = new ExtendedMagicItem()
            {
                Id = (int)(long)data[0],
                Name = data[1].ToString(),
                Quality = (int)(long)data[2],
                Type = data[3].ToString(),
                ItemSource = data[5].ToString(),
            };
            data = DataAccess.GetData("MagicItems, ExtendedMagicItems", $"MagicItems._id = {id} And MagicItems._Id = ExtendedMagicItems._id", null, "*")[0];
            item.Id = id;
            item.Name = data[1].ToString();
            item.Quality = (int)(long)data[2];
            item.Type = data[3].ToString();
            item.ItemSource = data[5].ToString();
            item.Description = data[8].ToString();
            if (data[9].ToString() != "")
                item.UnderType = data[9].ToString();
            if (data[10].ToString() != "")
                item.UnderQuality = data[10].ToString();
            if (data[11].ToString() != "")
                item.Attunement = data[11].ToString();
            else
            {
                item.Attunement = data[4].ToString() != "0" ? "требует настроенности" : "";
            }
            item.OptionableText = data[12].ToString();
            item.Features = new ObservableCollection<Feature>();
            foreach (var feature in GetFeatures(id, "FeaturesOfMagicItem"))
            {
                item.Features.Add(feature);
            }

            item.Table = GetTable(id, "TablesMagicItems");
            return item;
        }

        public IEnumerable<Monster> GetMonsters()
        {
            var list = DataAccess.GetData("Monsters", null, null, "*");
            var monsters = new List<Monster>();
            foreach (var monster in list)
            {
                monsters.Add(new Monster()
                {
                    Id = (int)(long)monster[0],
                    Name = list[1].ToString(),
                    Size = (int)(long)monster[2],
                    Type = monster[3].ToString(),
                    Habitat = new ObservableCollection<string>(monster[4].ToString().Split("@")),
                    Challenge = monster[5].ToString(),
                    IsLegendary = monster[6].ToString() == "1",
                    Source = monster[7].ToString(),
                });
            }

            return monsters;
        }

        public ExtendedMonster GetExtendedMonsterById(int id)
        {
            var list = DataAccess.GetData("ExtendedMonsters", $"_id = {id}", null, "*")[0];
            ExtendedMonster monster = new ExtendedMonster()
            {
                AC = (int)(long)list[1],
                HP = list[2].ToString(),
                Speed = list[3].ToString(),
                Str = (int)(long)list[4],
                Dex = (int)(long)list[5],
                Con = (int)(long)list[6],
                Intel = (int)(long)list[7],
                Wis = (int)(long)list[8],
                Cha = (int)(long)list[9],
                SavingThrows = list[10].ToString(),
                Skills = list[11].ToString(),
                Senses = list[12].ToString(),
                Languages = list[13].ToString(),
                Description = list[14].ToString(),
                LairActions = list[15].ToString(),
                RegionalEf = list[16].ToString(),
                UnderType = list[17].ToString(),
                WorldView = list[18].ToString(),
                ACType = list[19].ToString(),
                Immunity = list[20].ToString(),
                Resistance = list[21].ToString(),
                Vulnerability = list[22].ToString(),
                ImmunityState = list[23].ToString(),
            };

            list = DataAccess.GetData("Monsters", $"_id = {id}", null, "*")[0];
            monster.Id = id;
            monster.Name = list[1].ToString();
            monster.Size = (int)(long)list[2];
            monster.Type = list[3].ToString();
            monster.Habitat = new ObservableCollection<string>(list[4].ToString().Split("@"));
            monster.Challenge = list[5].ToString();
            monster.IsLegendary = list[6].ToString() == "1";
            monster.Source = list[7].ToString();

            monster.Features = new ObservableCollection<Feature>();
            monster.Actions = new ObservableCollection<Feature>();
            monster.ReciprocalActions = new ObservableCollection<Feature>();
            monster.LegendaryActions = new ObservableCollection<Feature>();

            foreach (var feature in GetFeatures(id, "MonsterFeatures"))
            {
                monster.Features.Add(feature);
            }
            foreach (var feature in GetFeatures(id, "MonsterActions"))
            {
                monster.Actions.Add(feature);
            }
            foreach (var feature in GetFeatures(id, "MonsterReciprocalActions"))
            {
                monster.ReciprocalActions.Add(feature);
            }

            if (!monster.IsLegendary) return monster;

            foreach (var feature in GetFeatures(id, "MonsterLegendaryActions"))
            {
                monster.LegendaryActions.Add(feature);
            }

            return monster;
        }


        public IEnumerable<Spell> GetSpells()
        {
           var data = DataAccess.GetData("Spells", null, null, "*");
           var spells = new List<Spell>();
           foreach (var spell in data)
           {
               var s = new Spell()
               {
                   Id = (int) (long) spell[0],
                   Name = spell[1].ToString(),
                   Lvl = (int) (long) spell[2],
                   School = spell[3].ToString(),
                   Concentration = (long)spell[5] != 0 ,
                   Source = spell[6].ToString()
               };
                s.SetComponents(spell[4].ToString());
               spells.Add(s);
               
           }

           return spells;
        }

        public ExtendedSpell GetExtendedSpellById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Group> GetGroups()
        {
            var data = DataAccess.GetData("Parties", null, null, "*");
            List<Group> groups = new List<Group>();
            foreach (var gr in data)
            {

                var group = new Model.Group()
                {
                    Id = (int) (long) gr[0],
                    Name = gr[1].ToString()
                };
                var players = DataAccess.GetData("Players", $"Group_id = {group.Id}", null,"*");
                foreach (var p in players)
                {
                    var player = new Player()
                    {
                        GroupId = (int) (long) p[0],
                        Name = p[1].ToString(),
                        PlayerName = p[2].ToString(),
                        Class = p[3].ToString(),
                        Id = (int) (long) p[4],
                        AC = (int) (long) p[5],
                        HP = (int) (long) p[6],
                        Experience = (int) (long) p[7],
                        PassWis = (int) (long) p[8]
                    };
                    group.Players.Add(player);
                }
                groups.Add(group);
            }

            return groups;
        }

        public IEnumerable<Player> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> GetPlayersByGroupId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Player> GetPlayerById(int id)
        {
            throw new NotImplementedException();
        }

        public void AddMonster(ExtendedMonster monster)
        {
            throw new NotImplementedException();
        }

        public void AddItem(ExtendedMagicItem item)
        {
            throw new NotImplementedException();
        }

        public void AddSpell(ExtendedSpell spell)
        {
            throw new NotImplementedException();
        }

        public void AddGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public void AddPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void UpdateMonster(ExtendedMonster monster)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(ExtendedMagicItem item)
        {
            throw new NotImplementedException();
        }

        public void UpdateSpell(ExtendedSpell spell)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void UpdateGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public void DeleteMonster(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteSpell(int id)
        {
            throw new NotImplementedException();
        }

        public void DeletePlayer(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteGroup(int id)
        {
            throw new NotImplementedException();
        }

        private void UpdateTable(Table table, int parentId, string dbTable)
        {

        }

        private void UpdateFeature(Feature feature, int parentId, string dbTable)
        {

        }

        private Table GetTable(int parentId, string dbTable)
        {
            var data = DataAccess.GetData(dbTable, $"ParentId = {parentId}", null, "*");
            if (data.Count == 0) return null;
            return new Table()
            {
                Rows = (int) (long) data[0][1], Columns = (int) (long) data[0][2],
                Fields = data[0][3].ToString().Split("@").ToList()
            };
        }

        private List<Feature> GetFeatures(int parentId, string dbTable)
        {
            List<Feature> features = new List<Feature>();
            foreach (var act in DataAccess.GetData(dbTable, $"_id = {parentId}", null, "Name, Description"))
            {
                features.Add(new Feature()
                {
                    ParentId = parentId,
                    Name = act[0].ToString(),
                    Description = act[1].ToString()
                });
            }

            return features;
        }
    }
}
