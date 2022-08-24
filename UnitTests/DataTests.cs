using DataBaseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void LoadAllItems()
        {
            var data = DataBaseContext.Instance.GetDataItems();
            Assert.IsNotNull(data);
        }

        [TestMethod]
        public void LoadAllCategory()
        {
            var magicItems = DataBaseContext.Instance.GetMagicItems();
            var monsters = DataBaseContext.Instance.GetMonsters();
            var spells = DataBaseContext.Instance.GetSpells();
            Assert.IsNotNull(magicItems);
            Assert.IsNotNull(monsters);
            Assert.IsNotNull(spells);
        }

        [TestMethod]
        public void LoadExtendedItems()
        {
            for (int i = 0; i < 10; i++)
            {
                DataBaseContext.Instance.GetExtendedMagicById(i);
            }
        }

        [TestMethod]
        public void LoadExtendedMonsters()
        {
            for (int i = 1; i <= 1; i++)
            {
                DataBaseContext.Instance.GetExtendedMonsterById(i);
            }
        }

        [TestMethod]
        public void GetGroups()
        {
            var groups = DataBaseContext.Instance.GetGroups();
        }
    }
}