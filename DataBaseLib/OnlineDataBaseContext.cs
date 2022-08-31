using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace DataBaseLib
{
    public class OnlineDataBaseContext : IDataContext
    {
        private static OnlineDataBaseContext _inst;
        public static OnlineDataBaseContext Instance => _inst ??= new OnlineDataBaseContext();
        private OnlineDataBaseContext() { }

        private static string HandleString(string str)
        {
           return str.Replace("'", "''").Replace("\\","\\\\");
        }

        public IEnumerable<DataItem> GetDataItems()
        {
            var request = OnlineDataAccess.GetData("SELECT _id, Name from Monsters");
            List<DataItem> dataItems = request.Select(it => new DataItem { Id = (int)it[0], Name = it[1].ToString(), ItemType = DataItem.DataType.Monster }).ToList();
            request = DataBaseLib.OnlineDataAccess.GetData("SELECT _id, Name from MagicItems");
            dataItems.AddRange(request.Select(it => new DataItem { Id = (int)it[0], Name = it[1].ToString(), ItemType = DataItem.DataType.MagicItem }));

            request = DataBaseLib.OnlineDataAccess.GetData("SELECT _id, Name from Spells");
            dataItems.AddRange(request.Select(it => new DataItem { Id = (int)it[0], Name = it[1].ToString(), ItemType = DataItem.DataType.Spell }));

            return dataItems;
        }

        public IEnumerable<MagicItem> GetMagicItems()
        {
            var data = OnlineDataAccess.GetData("MagicItems", null, "Name", "*");
            List<MagicItem> items = new List<MagicItem>();
            foreach (var item in data)
            {
                items.Add(new MagicItem()
                {
                    Id = (int)item[0],
                    Name = item[1].ToString(),
                    Quality = (int)item[2],
                    Type = item[3].ToString(),
                    Attunement = item[4].ToString() != "0" ? "(Настройка)" : "",
                    ItemSource = item[5].ToString(),
                });
            }
            return items;
        }

        public ExtendedMagicItem GetExtendedMagicById(int id)
        {
            var data = OnlineDataAccess.GetData("MagicItems", $"_id = {id}", "Name", "*")[0];
            var item = new ExtendedMagicItem()
            {
                Id = (int)data[0],
                Name = data[1].ToString(),
                Quality = (int)data[2],
                Type = data[3].ToString(),
                ItemSource = data[5].ToString(),
            };
            data = OnlineDataAccess.GetData("MagicItems, ExtendedMagicItems", $"MagicItems._id = {id} And MagicItems._Id = ExtendedMagicItems._id", null, "*")[0];
            item.Id = id;
            item.Name = data[1].ToString();
            item.Quality = (int)data[2];
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
            var list = OnlineDataAccess.GetData("Monsters", null, null, "*");
            var monsters = new List<Monster>();
            foreach (var monster in list)
            {
                monsters.Add(new Monster()
                {
                    Id = (int)monster[0],
                    Name = monster[1].ToString(),
                    Size = (int)monster[2],
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
            var list = OnlineDataAccess.GetData("ExtendedMonsters", $"_id = {id}", null, "*")[0];
            ExtendedMonster monster = new ExtendedMonster()
            {
                AC = (int)list[1],
                HP = list[2].ToString(),
                Speed = list[3].ToString(),
                Str = (int)list[4],
                Dex = (int)list[5],
                Con = (int)list[6],
                Intel = (int)list[7],
                Wis = (int)list[8],
                Cha = (int)list[9],
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

            list = OnlineDataAccess.GetData("Monsters", $"_id = {id}", null, "*")[0];
            monster.Id = id;
            monster.Name = list[1].ToString();
            monster.Size = (int)list[2];
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
            var data = OnlineDataAccess.GetData("Spells", null, null, "*");
            var spells = new List<Spell>();
            foreach (var spell in data)
            {
                var s = new Spell()
                {
                    Id = (int)spell[0],
                    Name = spell[1].ToString(),
                    Lvl = (int)spell[2],
                    School = spell[3].ToString(),
                    Concentration = (int)spell[5] != 0,
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
            var data = OnlineDataAccess.GetData("Parties", null, null, "*");
            List<Group> groups = new List<Group>();
            foreach (var gr in data)
            {

                var group = new Model.Group()
                {
                    Id = (int)gr[0],
                    Name = gr[1].ToString()
                };
                var players = OnlineDataAccess.GetData("Players", $"Group_id = {group.Id}", null, "*");
                foreach (var p in players)
                {
                    var player = GetPlayerFromData(p);
                    group.Players.Add(player);
                }
                groups.Add(group);
            }

            return groups;
        }

        private static Player GetPlayerFromData(object[] playerData)
        {
            var player = new Player()
            {
                GroupId = (int)playerData[0],
                Name = playerData[1].ToString(),
                PlayerName = playerData[2].ToString(),
                Class = playerData[3].ToString(),
                Id = (int)playerData[4],
                AC = (int)playerData[5],
                HP = (int)playerData[6],
                Experience = (int)playerData[7],
                PassWis = (int)playerData[8],
                Initiative = (int)playerData[9],
                Race = playerData[10].ToString()
            };
            return player;
        }

        public IEnumerable<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            var data = OnlineDataAccess.GetData("Players", null, null, "*");
            foreach (var singleData in data)
            {
                players.Add(GetPlayerFromData(singleData));
            }

            return players;
        }

        public IEnumerable<Encounter> GetEncounters()
        {
            var encData = OnlineDataAccess.GetData("Select * From Encounters");
            List<Encounter> encounters = new List<Encounter>();

            foreach (var en in encData)
            {
                encounters.Add(new Encounter()
                {
                    GroupId = (int)en[0],
                    Name = en[1].ToString(),
                    Id = (int)en[2]
                });
            }

            var monsterList = GetMonsters().ToList();
            foreach (var enc in encounters)
            {
                var monsterData = OnlineDataAccess.GetData("SELECT * FROM EncountersToMonsters " +
                                                        $"where Encounter_Id = {enc.Id}");
                
                foreach (var monsterd in monsterData)
                {
                    enc.Monsters.Add(new BattleMonster()
                    {
                        Quantity = (int)monsterd[2],
                        Monster =  monsterList.First(m => m.Id == Convert.ToInt32(monsterd[1]))
                    });
                }
            }

            return encounters;
        }

        public IEnumerable<CompletedDataItem> GetCompletedItems()
        {
            List<CompletedDataItem> completedDataItems = new List<CompletedDataItem>();
            var data = OnlineDataAccess.GetData(
                 "select _id, Name, (select Description from ExtendedMagicItems where MagicItems._id = ExtendedMagicItems._id) is not null as Completed from MagicItems");
            AddCompletedItems(completedDataItems, data, DataItem.DataType.MagicItem);
             data = OnlineDataAccess.GetData(
                "select _id, Name, (select Description from ExtendedMonsters where Monsters._id = ExtendedMonsters._id) is not null as Completed from Monsters");
            AddCompletedItems(completedDataItems, data, DataItem.DataType.Monster);
            return completedDataItems;
        }

        private static void AddCompletedItems(List<CompletedDataItem> completedDataItems, List<object[]> data, DataItem.DataType type)
        {
            foreach (var item in data)
            {
                completedDataItems.Add(new CompletedDataItem()
                {
                    Id = Convert.ToInt32(item[0]),
                    Name = item[1].ToString(),
                    ItemType = type,
                    Color = new SolidColorBrush(
                        Convert.ToInt32(item[2]) == 1
                            ? Windows.UI.Color.FromArgb(Color.YellowGreen.A, Color.YellowGreen.R,
                                Color.YellowGreen.G, Color.YellowGreen.B)
                            : Windows.UI.Color.FromArgb(0, 0, 0, 0))
                });
            }
        }

        public async Task AddMonster(ExtendedMonster monster)
        {
            throw new NotImplementedException();
        }

        public async Task AddItem(ExtendedMagicItem item)
        {
            await OnlineDataAccess.RawRequestAsync(
                    "INSERT INTO MagicItems (Name, Quality, Type, Source)" +
                    $"values('{HandleString(item.Name)}',{item.Quality}, '{item.Type}', '{HandleString(item.ItemSource)}')");
            var id = (int)OnlineDataAccess.GetData($"Select _id from MagicItems where Name = '{HandleString(item.Name)}'")[0][0];
            await OnlineDataAccess.RawRequestAsync($"Insert into ExtendedMagicItems (_id) values ({id})");
            item.Id = id;
            await UpdateItem(item);
        }

        public async Task AddSpell(ExtendedSpell spell)
        {
            throw new NotImplementedException();
        }

        public async Task AddGroup(Group group)
        {
            await OnlineDataAccess.RawRequestAsync("Insert into Parties (Name) values ('" + HandleString(group.Name) + "')");
        }
        
        public async Task AddPlayer(Player player)
        {
            await OnlineDataAccess.RawRequestAsync(
                "INSERT into Players (Group_Id, Name, PlayerName, Class, AC, HP, Exp, PassWis, Initiative, Race) " +
                $"values ({player.GroupId}, '{HandleString(player.Name)}', " +
                $"'{HandleString(player.PlayerName)}', '{player.Class}', " +
                $"{player.AC}, {player.HP}, {player.Experience}, " +
                $"{player.PassWis}, {player.Initiative}, " +
                $"'{HandleString(player.Race)}')");
        }

        public async Task AddEncounter(Encounter enc)
        {
            await OnlineDataAccess.RawRequestAsync($"insert into Encounters(group_id, name) values ({enc.GroupId}, '{HandleString(enc.Name)}')");
            var id = (int)OnlineDataAccess.GetData(" SELECT MAX(_id) FROM Encounters")[0][0];
            foreach (var monster in enc.Monsters)
            {
                await AddBattleMonster(monster, id);
            }
        }

        private async Task AddBattleMonster(BattleMonster monster, int encId)
        {
            await OnlineDataAccess.RawRequestAsync($"insert into EncountersToMonsters(Monster_Quantity, Monster_id, Encounter_id) " +
                                             $"values ({monster.Quantity}, {monster.Monster.Id}, {encId})");
        }

        public async Task UpdateMonster(ExtendedMonster monster)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateItem(ExtendedMagicItem item)
        {
            (await OnlineDataAccess.RawRequestAsync($"UPDATE MagicItems " +
                                                          $"SET Name = \'{HandleString(item.Name)}\', " +
                                                          $"Quality = {item.Quality}, " +
                                                          $"Type = \'{item.Type}\', " +
                                                          $"Attunement = \'{((item.Attunement != string.Empty) ? 1 : 0)}\', " +
                                                          $"Source = \'{HandleString(item.ItemSource)}\', " +
                                                          $"isHomeBrew = 0 " +
                                                          $"Where _id = {item.Id}")).Close();

            (await OnlineDataAccess.RawRequestAsync("UPDATE ExtendedMagicItems SET " +
                 $"Description = \'{HandleString(item.Description)}\', " +
                 $"Undertype = \'{HandleString(item.UnderType)}\'," +
                 $"UnderQuality = \'{HandleString(item.UnderQuality)}\', " +
                 $"Attunement = \'{HandleString(item.Attunement)}\', " +
                 $"OptionalText = \'{HandleString(item.OptionableText)}\' " +
                 $"Where _id = {item.Id}")).Close();
            await Task.Run(() => UpdateTable(item.Table, item.Id, "TablesMagicItems"));
            await Task.Run(() =>
            {
                DeleteFeatures(item.Id, "FeaturesOfMagicItem");
                foreach (var feature in item.Features)
                {
                    UpdateFeature(feature, item.Id, "FeaturesOfMagicItem");
                }

            });
        }

        public async Task UpdateSpell(ExtendedSpell spell)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePlayer(Player player)
        {
            await OnlineDataAccess.RawRequestAsync("Update Players SET" +
                                  $"Group_Id = {player.GroupId}, " +
                                  $"Name = '{HandleString(player.Name)}', " +
                                  $"PlayerName = '{HandleString(player.PlayerName)}', " +
                                  $"Class = '{player.Class}', AC = {player.AC}, HP = {player.HP}, " +
                                  $"Exp = {player.Experience}, PassWis = {player.PassWis}, " +
                                  $"Initiative = {player.Initiative}, " +
                                  $"Race = '{HandleString(player.Race)}' " +
                                  $"WHERE _id = {player.Id}");
        }

        public async Task UpdateGroup(Group group)
        {
            await OnlineDataAccess.RawRequestAsync("Update Parties SET" +
                                   $"Name = '{HandleString(group.Name)}' " +
                                   $"WHERE _id = {group.Id}");
        }

        public async Task UpdateEncounter(Encounter encounter)
        {
            await OnlineDataAccess.RawRequestAsync("Update Encounters SET " +
                                  $"Name = '{HandleString(encounter.Name)}' " +
                                  $"WHERE _id = {encounter.Id}");
            await OnlineDataAccess.RawRequestAsync("Delete from EncountersToMonsters where Encounter_id = " + encounter.Id);
            foreach (var monster in encounter.Monsters)
            {
                await AddBattleMonster(monster, encounter.Id);
            }
        }

        public async Task DeleteMonster(int id)
        {
            await OnlineDataAccess.RawRequestAsync($"Delete from Monsters where _id = {id}");
        }

        public async Task DeleteItem(int id)
        {
            await OnlineDataAccess.RawRequestAsync($"Delete from MagicItems where _id = {id}");

        }

        public async Task DeleteSpell(int id)
        {
            await OnlineDataAccess.RawRequestAsync($"Delete from Spells where _id = {id}");
        }

        public async Task DeletePlayer(int id)
        {
            await OnlineDataAccess.RawRequestAsync($"Delete from Players WHERE _id = {id}");
        }

        public async Task DeleteGroup(int id)
        {
            await OnlineDataAccess.RawRequestAsync($"Delete from Parties WHERE _id = {id}");
        }

        public async Task DeleteEncounter(int id)
        {
            await OnlineDataAccess.RawRequestAsync("DELEte from Encounters WHERE _id = " + id);
        }

        private void UpdateTable(Table table, int parentId, string dbTable)
        {
            DeleteTable(parentId, dbTable);
            if (table == null) return;
            string data = string.Join("@", table.Fields.Select(HandleString));

            OnlineDataAccess.RawRequest(
                $"INSERT INTO {dbTable} (ParentId, Rows, Columns, Data) " +
                $"values ({parentId},{table.Rows}, {table.Columns}, '{data}')");
        }

        private static void DeleteTable(int parentId, string dbTable)
        {
            OnlineDataAccess.RawRequest($"Delete from {dbTable} where ParentId = {parentId}");
        }
        private void DeleteFeatures(int parentId, string dbTable)
        {
            OnlineDataAccess.RawRequest($"Delete from {dbTable} where _id = {parentId}");
        }
        private void UpdateFeature(Feature feature, int parentId, string dbTable)
        {
            if (feature == null) return;
            DataBaseLib.OnlineDataAccess.RawRequest($"INSERT INTO {dbTable} (_id, Name, Description) " +
                                              $"values ({parentId},'{HandleString(feature.Name)}', '{HandleString(feature.Description)}')");
        }

        private static Table GetTable(int parentId, string dbTable)
        {
            var data = OnlineDataAccess.GetData(dbTable, $"ParentId = {parentId}", null, "*");
            if (data.Count == 0) return null;
            return new Table()
            {
                Rows = (int)data[0][1],
                Columns = (int)data[0][2],
                Fields = data[0][3].ToString().Split("@").ToList()
            };
        }

        private static List<Feature> GetFeatures(int parentId, string dbTable)
        {
            List<Feature> features = new List<Feature>();
            foreach (var act in OnlineDataAccess.GetData(dbTable, $"_id = {parentId}", null, "Name, Description"))
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
