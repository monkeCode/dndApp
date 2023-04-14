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
    public class DataBaseContext : IDataContext
    {
        private static DataBaseContext _inst;
        public static DataBaseContext Instance => _inst ??= new DataBaseContext();
        private DataBaseContext() { }

        public IEnumerable<DataItem> GetDataItems()
        {
            var request = DataAccess.Instance.GetData("SELECT _id, Name from Monsters");
            List<DataItem> dataItems = request.Select(it => new DataItem { Id = Convert.ToInt32(it[0]), Name = it[1].ToString(), ItemType = DataItem.DataType.Monster }).ToList();
            request = DataBaseLib.DataAccess.Instance.GetData("SELECT _id, Name from MagicItems");
            dataItems.AddRange(request.Select(it => new DataItem { Id = Convert.ToInt32(it[0]), Name = it[1].ToString(), ItemType = DataItem.DataType.MagicItem }));

            request = DataBaseLib.DataAccess.Instance.GetData("SELECT _id, Name from Spells");
            dataItems.AddRange(request.Select(it => new DataItem { Id = Convert.ToInt32(it[0]), Name = it[1].ToString(), ItemType = DataItem.DataType.Spell }));

            return dataItems;
        }

        public IEnumerable<MagicItem> GetMagicItems()
        {
            var data = DataAccess.Instance.GetData("MagicItems", null, "Name", "*");
            List<MagicItem> items = new List<MagicItem>();
            foreach (var item in data)
            {
                items.Add(new MagicItem()
                {
                    Id = Convert.ToInt32(item[0]),
                    Name = item[1].ToString(),
                    Quality = Convert.ToInt32(item[2]),
                    Type = item[3].ToString(),
                    Attunement = item[4].ToString() != "0" ? "(Настройка)" : "",
                    ItemSource = item[5].ToString(),
                    IsHomebrew = Convert.ToInt32(item[6]) == 1
                });
            }
            return items;
        }

        public ExtendedMagicItem GetExtendedMagicById(int id)
        {
            var data = DataAccess.Instance.GetData("MagicItems", $"_id = {id}", "Name", "*")[0];
            var item = new ExtendedMagicItem()
            {
                Id = Convert.ToInt32(data[0]),
                Name = data[1].ToString(),
                Quality = Convert.ToInt32(data[2]),
                Type = data[3].ToString(),
                ItemSource = data[5].ToString(),
                IsHomebrew = Convert.ToInt32(data[6]) == 1,
            };
            data = DataAccess.Instance.GetData("MagicItems, ExtendedMagicItems", $"MagicItems._id = {id} And MagicItems._Id = ExtendedMagicItems._id", null, "*")[0];
            item.Id = id;
            item.Name = data[1].ToString();
            item.Quality = Convert.ToInt32(data[2]);
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
            var list = DataAccess.Instance.GetData("Monsters", null, null, "*");
            var monsters = new List<Monster>();
            foreach (var monster in list)
            {
                monsters.Add(new Monster()
                {
                    Id = Convert.ToInt32(monster[0]),
                    Name = monster[1].ToString(),
                    Size = Convert.ToInt32(monster[2]),
                    Type = monster[3].ToString(),
                    Habitat = new ObservableCollection<string>(monster[4].ToString().Split("@")),
                    Challenge = monster[5].ToString(),
                    IsLegendary = monster[6].ToString() == "1",
                    Source = monster[7].ToString(),
                    IsHomebrew = Convert.ToInt32(monster[8]) == 1
                });
            }

            return monsters;
        }

        public ExtendedMonster GetExtendedMonsterById(int id)
        {
            var list = DataAccess.Instance.GetData("ExtendedMonsters", $"_id = {id}", null, "*")[0];
            ExtendedMonster monster = new ExtendedMonster()
            {
                AC = Convert.ToInt32(list[1]),
                HP = list[2].ToString(),
                Speed = list[3].ToString(),
                Str = Convert.ToInt32(list[4]),
                Dex = Convert.ToInt32(list[5]),
                Con = Convert.ToInt32(list[6]),
                Intel = Convert.ToInt32(list[7]),
                Wis = Convert.ToInt32(list[8]),
                Cha = Convert.ToInt32(list[9]),
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

            list = DataAccess.Instance.GetData("Monsters", $"_id = {id}", null, "*")[0];
            monster.Id = id;
            monster.Name = list[1].ToString();
            monster.Size = Convert.ToInt32(list[2]);
            monster.Type = list[3].ToString();
            monster.Habitat = new ObservableCollection<string>(list[4].ToString().Split("@"));
            monster.Challenge = list[5].ToString();
            monster.IsLegendary = list[6].ToString() == "1";
            monster.Source = list[7].ToString();
            monster.IsHomebrew = Convert.ToInt32(list[8]) == 1;

            monster.Features = new ObservableCollection<Feature>();
            monster.Actions = new ObservableCollection<Feature>();
            monster.ReciprocalActions = new ObservableCollection<Feature>();
            monster.LegendaryActions = new ObservableCollection<Feature>();

            monster.Table = GetTable(monster.Id, "TablesMonsters");

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
            var data = DataAccess.Instance.GetData("Spells", null, null, "*");
            var spells = new List<Spell>();
            foreach (var spell in data)
            {
                var s = new Spell()
                {
                    Id = Convert.ToInt32(spell[0]),
                    Name = spell[1].ToString(),
                    Lvl = Convert.ToInt32(spell[2]),
                    School = spell[3].ToString(),
                    Concentration = Convert.ToInt32(spell[5]) == 1,
                    Source = spell[6].ToString(),
                    IsHomebrew = Convert.ToInt32(spell[7]) == 1,
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
            var data = DataAccess.Instance.GetData("Parties", null, null, "*");
            List<Group> groups = new List<Group>();
            foreach (var gr in data)
            {

                var group = new Model.Group()
                {
                    Id = Convert.ToInt32(gr[0]),
                    Name = gr[1].ToString()
                };
                var players = DataAccess.Instance.GetData("Players", $"Group_id = {group.Id}", null, "*");
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
                GroupId = Convert.ToInt32(playerData[0]),
                Name = playerData[1].ToString(),
                PlayerName = playerData[2].ToString(),
                Class = playerData[3].ToString(),
                Id = Convert.ToInt32(playerData[4]),
                AC = Convert.ToInt32(playerData[5]),
                HP = Convert.ToInt32(playerData[6]),
                Experience = Convert.ToInt32(playerData[7]),
                PassWis = Convert.ToInt32(playerData[8]),
                Initiative = Convert.ToInt32(playerData[9]),
                Race = playerData[10].ToString()
            };
            return player;
        }

        public IEnumerable<Player> GetPlayers()
        {
            List<Player> players = new List<Player>();
            var data = DataAccess.Instance.GetData("Players", null, null, "*");
            foreach (var singleData in data)
            {
                players.Add(GetPlayerFromData(singleData));
            }

            return players;
        }

        public IEnumerable<Encounter> GetEncounters()
        {
            var encData = DataAccess.Instance.GetData("Select * From Encounters");
            List<Encounter> encounters = new List<Encounter>();

            foreach (var en in encData)
            {
                encounters.Add(new Encounter()
                {
                    GroupId = Convert.ToInt32(en[0]),
                    Name = en[1].ToString(),
                    Id = Convert.ToInt32(en[2])
                });
            }

            var monsterList = GetMonsters().ToList();
            foreach (var enc in encounters)
            {
                var monsterData = DataAccess.Instance.GetData("SELECT * FROM EncountersToMonsters " +
                                                           $"where Encounter_Id = {enc.Id}");

                foreach (var monsterd in monsterData)
                {
                    enc.Monsters.Add(new EncountingMonster()
                    {
                        Quantity = Convert.ToInt32(monsterd[2]),
                        Monster = monsterList.First(m => m.Id == Convert.ToInt32(monsterd[1]))
                    });
                }
            }

            return encounters;
        }

        public IEnumerable<CompletedDataItem> GetCompletedItems()
        {
            List<CompletedDataItem> completedDataItems = new List<CompletedDataItem>();
            var data = DataAccess.Instance.GetData(
                "select _id, Name, (select Description from ExtendedMagicItems where MagicItems._id = ExtendedMagicItems._id) is not null as Completed from MagicItems");
            AddCompletedItems(completedDataItems, data, DataItem.DataType.MagicItem);
            data = DataAccess.Instance.GetData(
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
            await DataAccess.Instance.RawRequestAsync($"INSERT INTO Monsters(Name, Size, Type, ChallengeRate) " +
                                                      $"values ('{monster.Name.Replace("'", "''")}', " +
                                                      $"{monster.Size}, " +
                                                      $"'{monster.Type}', " +
                                                      $" '{monster.Challenge}');");
            var id = Convert.ToInt32(DataAccess.Instance.GetData("select _id from Monsters Last where _id = (select max(_id) from Monsters)")[0][0]);
            await DataAccess.Instance.RawRequestAsync($"Insert into ExtendedMonsters (_id) values ({id})");
            monster.Id = id;
            await UpdateMonster(monster);
        }

        public async Task AddItem(ExtendedMagicItem item)
        {
            await DataAccess.Instance.RawRequestAsync(
                    "INSERT INTO MagicItems (Name, Quality, Type, Source)" +
                    $"values('{item.Name.Replace("'", "''")}',{item.Quality}, '{item.Type}', '{item.ItemSource.Replace("'", "''")}')");
            //var id = Convert.ToInt32(DataAccess.Instance.GetData($"Select _id from MagicItems where Name = '{item.Name.Replace("'", "''")}'")[0][0]);
            var id = Convert.ToInt32(DataAccess.Instance.GetData("select _id from MagicItems Last where _id = (select max(_id) from MagicItems)")[0][0]);
            await DataAccess.Instance.RawRequestAsync($"Insert into ExtendedMagicItems (_id) values ({id})");
            item.Id = id;
            await UpdateItem(item);
        }

        public async Task AddSpell(ExtendedSpell spell)
        {
            throw new NotImplementedException();
        }

        public async Task AddGroup(Group group)
        {
            await DataAccess.Instance.RawRequestAsync("Insert into Parties (Name) values ('" + group.Name.Replace("'", "''") + "')");
        }

        public async Task AddPlayer(Player player)
        {
            await DataAccess.Instance.RawRequestAsync(
                "INSERT into Players (Group_Id, Name, PlayerName, Class, AC, HP, Exp, PassWis, Initiative, Race) " +
                $"values ({player.GroupId}, '{player.Name.Replace("'", "''")}', " +
                $"'{player.PlayerName.Replace("'", "''")}', '{player.Class}', " +
                $"{player.AC}, {player.HP}, {player.Experience}, " +
                $"{player.PassWis}, {player.Initiative}, " +
                $"'{player.Race.Replace("'", "''")}')");
        }

        public async Task AddEncounter(Encounter enc)
        {
            await DataAccess.Instance.RawRequestAsync($"insert into Encounters(group_id, name) values ({enc.GroupId}, '{enc.Name?.Replace("'", "''")}')");
            var id = DataAccess.Instance.GetLastId();
            foreach (var monster in enc.Monsters)
            {
                await AddBattleMonster(monster, id);
            }
        }

        private async Task AddBattleMonster(EncountingMonster monster, int encId)
        {
            await DataAccess.Instance.RawRequestAsync($"insert into EncountersToMonsters(Monster_Quantity, Monster_id, Encounter_id) " +
                                             $"values ({monster.Quantity}, {monster.Monster.Id}, {encId})");
        }

        public async Task UpdateMonster(ExtendedMonster monster)
        {
            await DataAccess.Instance.RawRequestAsync("UPDATE  Monsters set " +
                                                      $"Name = '{monster.Name.Replace("'","''")}', " +
                                                      $"Size = {monster.Size}, " +
                                                      $"Type = '{monster.Type.Replace("'", "''")}', " +
                                                      $"Habitat = '{string.Join("@",monster.Habitat)}', " +
                                                      $"ChallengeRate = '{monster.Challenge}', " +
                                                      $"IsLegendary = {monster.IsLegendary}, " +
                                                      $"Source = '{monster.Source.Replace("'", "''")}', " +
                                                      $"IsHomebrew = 0 " +
                                                      $"where _id = {monster.Id}");
            await DataAccess.Instance.RawRequestAsync("UPDATE ExtendedMonsters set " +
                                                      $"AC = {monster.AC}, " +
                                                      $"HP = '{monster.HP.Replace("'", "''")}', " +
                                                      $"speed = '{monster.Speed.Replace("'", "''")}', " +
                                                      $"STR = {monster.Str}, " +
                                                      $"Dex = {monster.Dex}, " +
                                                      $"CON = {monster.Con}, " +
                                                      $"INTEL = {monster.Intel}, " +
                                                      $"WIS = {monster.Wis}, " +
                                                      $"CHA = {monster.Cha}, " +
                                                      $"SavingThrows = '{monster.SavingThrows.Replace("'", "''")}', " +
                                                      $"Skills = '{monster.Skills.Replace("'", "''")}', " +
                                                      $"Senses = '{monster.Senses.Replace("'", "''")}', " +
                                                      $"Languages = '{monster.Languages.Replace("'", "''")}', " +
                                                      $"Description = '{monster.Description.Replace("'", "''")}', " +
                                                      $"LairActions = '{monster.LairActions.Replace("'", "''")}', " +
                                                      $"RegionalEffect ='{monster.RegionalEf.Replace("'", "''")}', " +
                                                      $"UnderType = '{monster.UnderType.Replace("'", "''")}', " +
                                                      $"WorldView = '{monster.WorldView}', " +
                                                      $"ACType = '{monster.ACType}', " +
                                                      $"Immunity = '{monster.Immunity}', " +
                                                      $"Resistance = '{monster.Resistance}', " +
                                                      $"Vulnerability = '{monster.Vulnerability}', " +
                                                      $"ImmunityState = '{monster.ImmunityState}' " +
                                                      $"where _id = {monster.Id}");

            await Task.Run(() => UpdateTable(monster.Table, monster.Id, "TablesMonsters"));
            await Task.Run(() =>
            {
                DeleteFeatures(monster.Id, "MonsterFeatures");
                foreach (var feature in monster.Features)
                {
                    UpdateFeature(feature, monster.Id, "MonsterFeatures");
                }

            });
            await Task.Run(() =>
            {
                DeleteFeatures(monster.Id, "MonsterActions");
                foreach (var feature in monster.Actions)
                {
                    UpdateFeature(feature, monster.Id, "MonsterActions");
                }

            }); 
            await Task.Run(() =>
            {
                DeleteFeatures(monster.Id, "MonsterLegendaryActions");
                foreach (var feature in monster.LegendaryActions)
                {
                    UpdateFeature(feature, monster.Id, "MonsterLegendaryActions");
                }

            });
            await Task.Run(() =>
            {
                DeleteFeatures(monster.Id, "MonsterReciprocalActions");
                foreach (var feature in monster.ReciprocalActions)
                {
                    UpdateFeature(feature, monster.Id, "MonsterReciprocalActions");
                }

            });

        }

        public async Task UpdateItem(ExtendedMagicItem item)
        {
            await DataAccess.Instance.RawRequestAsync($"UPDATE MagicItems " +
                                                          $"SET Name = \'{item.Name.Replace("'", "''")}\', " +
                                                          $"Quality = {item.Quality}, " +
                                                          $"Type = \'{item.Type}\', " +
                                                          $"Attunement = \'{((item.Attunement != string.Empty) ? 1 : 0)}\', " +
                                                          $"Source = \'{item.ItemSource.Replace("'", "''")}\', " +
                                                          $"isHomeBrew = 0 " +
                                                          $"Where _id = {item.Id}");

            await DataAccess.Instance.RawRequestAsync("UPDATE ExtendedMagicItems SET " +
                 $"Description = \'{item.Description.Replace("'", "''")}\', " +
                 $"Undertype = \'{item.UnderType.Replace("'", "''")}\'," +
                 $"UnderQuality = \'{item.UnderQuality.Replace("'", "''")}\', " +
                 $"Attunement = \'{item.Attunement.Replace("'", "''")}\', " +
                 $"OptionalText = \'{item.OptionableText.Replace("'", "''")}\' " +
                 $"Where _id = {item.Id}");
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
            await DataAccess.Instance.RawRequestAsync("Update Players SET" +
                                  $"Group_Id = {player.GroupId}, " +
                                  $"Name = '{player.Name.Replace("'", "''")}', " +
                                  $"PlayerName = '{player.PlayerName.Replace("'", "''")}', " +
                                  $"Class = '{player.Class}', AC = {player.AC}, HP = {player.HP}, " +
                                  $"Exp = {player.Experience}, PassWis = {player.PassWis}, " +
                                  $"Initiative = {player.Initiative}, " +
                                  $"Race = '{player.Race.Replace("'", "''")}' " +
                                  $"WHERE _id = {player.Id}");
        }

        public async Task UpdateGroup(Group group)
        {
            await DataAccess.Instance.RawRequestAsync("Update Parties SET" +
                                   $"Name = '{group.Name.Replace("'", "''")}' " +
                                   $"WHERE _id = {group.Id}");
        }

        public async Task UpdateEncounter(Encounter encounter)
        {
            await DataAccess.Instance.RawRequestAsync("Update Encounters SET " +
                                  $"Name = '{encounter.Name.Replace("'", "''")}' " +
                                  $"WHERE _id = {encounter.Id}");
            await DataAccess.Instance.RawRequestAsync("Delete from EncountersToMonsters where Encounter_id = " + encounter.Id);
            foreach (var monster in encounter.Monsters)
            {
                await AddBattleMonster(monster, encounter.Id);
            }
        }

        public async Task DeleteMonster(int id)
        {
            await DataAccess.Instance.RawRequestAsync($"Delete from Monsters where _id = {id}");
        }

        public async Task DeleteItem(int id)
        {
            await DataAccess.Instance.RawRequestAsync($"Delete from MagicItems where _id = {id}");

        }

        public async Task DeleteSpell(int id)
        {
            await DataAccess.Instance.RawRequestAsync($"Delete from Spells where _id = {id}");
        }

        public async Task DeletePlayer(int id)
        {
            await DataAccess.Instance.RawRequestAsync($"Delete from Players WHERE _id = {id}");
        }

        public async Task DeleteGroup(int id)
        {
            await DataAccess.Instance.RawRequestAsync($"Delete from Parties WHERE _id = {id}");
        }

        public async Task DeleteEncounter(int id)
        {
            await DataAccess.Instance.RawRequestAsync("DELEte from Encounters WHERE _id = " + id);
        }

        private void UpdateTable(Table table, int parentId, string dbTable)
        {
            DeleteTable(parentId, dbTable);
            if (table == null) return;
            string data = string.Join("@", table.Fields.Select(it => it.Replace("'", "''")));

            DataAccess.Instance.RawRequest(
                $"INSERT INTO {dbTable} (ParentId, Rows, Columns, Data) " +
                $"values ({parentId},{table.Rows}, {table.Columns}, '{data}')");
        }

        private static void DeleteTable(int parentId, string dbTable)
        {
            DataAccess.Instance.RawRequest($"Delete from {dbTable} where ParentId = {parentId}");
        }
        private void DeleteFeatures(int parentId, string dbTable)
        {
            DataAccess.Instance.RawRequest($"Delete from {dbTable} where _id = {parentId}");
        }
        private void UpdateFeature(Feature feature, int parentId, string dbTable)
        {
            if (feature == null) return;
            DataBaseLib.DataAccess.Instance.RawRequest($"INSERT INTO {dbTable} (_id, Name, Description) " +
                                              $"values ({parentId},'{feature.Name.Replace("'", "''")}', '{feature.Description.Replace("'", "''")}')");
        }

        private static Table GetTable(int parentId, string dbTable)
        {
            var data = DataAccess.Instance.GetData(dbTable, $"ParentId = {parentId}", null, "*");
            if (data.Count == 0) return null;
            return new Table()
            {
                Rows = Convert.ToInt32(data[0][1]),
                Columns = Convert.ToInt32(data[0][2]),
                Fields = data[0][3].ToString().Split("@").ToList()
            };
        }

        private static List<Feature> GetFeatures(int parentId, string dbTable)
        {
            List<Feature> features = new List<Feature>();
            foreach (var act in DataAccess.Instance.GetData(dbTable, $"_id = {parentId}", null, "Name, Description"))
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
