using Model;
using System.Collections.Generic;

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

        IEnumerable<Player> GetPlayersByGroupId(int id);
        IEnumerable<Player> GetPlayerById(int id);

        #endregion

        #region Add

        void AddMonster(ExtendedMonster monster);

        void AddItem(ExtendedMagicItem item);

        void AddSpell(ExtendedSpell spell);

        void AddGroup(Group group);

        void AddPlayer(Player player);

        #endregion

        #region Update

        void UpdateMonster(ExtendedMonster monster);

        void UpdateItem(ExtendedMagicItem item);

        void UpdateSpell(ExtendedSpell spell);

        void UpdatePlayer(Player player);

        void UpdateGroup(Group group);

        #endregion

        #region Delete

        void DeleteMonster(int id);

        void DeleteItem(int id);

        void DeleteSpell(int id);

        void DeletePlayer(int id);

        void DeleteGroup(int id);

        #endregion
    }
}
