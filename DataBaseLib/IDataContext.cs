﻿using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBaseLib
{
    public interface IDataContext
    {
        #region Get

        IEnumerable<DataItem> GetDataItems();

        IEnumerable<MagicItem> GetMagicItems();

        ExtendedMagicItem GetExtendedMagicById(int id);

        IEnumerable<Monster> GetMonsters();

        ExtendedMonster GetExtendedMonsterById(int id);

        IEnumerable<Spell> GetSpells();

        ExtendedSpell GetExtendedSpellById(int id);

        IEnumerable<Group> GetGroups();

        IEnumerable<Player> GetPlayers();

        IEnumerable<Encounter> GetEncounters();

        IEnumerable<CompletedDataItem> GetCompletedItems();


        #endregion

        #region Add

        Task AddMonster(ExtendedMonster monster);

        Task AddItem(ExtendedMagicItem item);

        Task AddSpell(ExtendedSpell spell);

        Task AddGroup(Group group);

        Task AddPlayer(Player player);

        Task AddEncounter(Encounter enc);

        #endregion

        #region Update

        Task UpdateMonster(ExtendedMonster monster);

        Task UpdateItem(ExtendedMagicItem item);

        Task UpdateSpell(ExtendedSpell spell);

        Task UpdatePlayer(Player player);

        Task UpdateGroup(Group group);

        Task UpdateEncounter(Encounter encounter);

        #endregion

        #region Delete

        Task DeleteMonster(int id);

        Task DeleteItem(int id);

        Task DeleteSpell(int id);

        Task DeletePlayer(int id);

        Task DeleteGroup(int id);
        Task DeleteEncounter(int id);

        #endregion
    }
}
