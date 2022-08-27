using DataBaseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Linq;

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

        [TestMethod]
        public void UpdateItem()
        {
            var item = DataBaseContext.Instance.GetExtendedMagicById(3);
            item.Description = "Описание обновлено";
            item.OptionableText = "Текст опционального ' '' '''' поведения обновлен";
            item.Attunement = "Требует настроенности test";
            item.UnderType = "Тип подчиненного обновлен";
            item.UnderQuality = "Качество подчиненного обновлено";
            DataBaseContext.Instance.UpdateItem(item).Wait();
            var newItem = DataBaseContext.Instance.GetExtendedMagicById(3);
            Assert.AreEqual(item.Description, newItem.Description);
            Assert.AreEqual(item.OptionableText, newItem.OptionableText);
            Assert.AreEqual(item.Attunement, newItem.Attunement);
            Assert.AreEqual(item.UnderType, newItem.UnderType);
            Assert.AreEqual(item.UnderQuality, newItem.UnderQuality);

        }

        [TestMethod]
        public void AddNewItem()
        {
            var item = new ExtendedMagicItem();
            item.Attunement = "test";
            item.Description = "test";
            item.OptionableText = "test";
            item.UnderType = "test";
            item.UnderQuality = "test";
            item.Name = "test";
            DataBaseContext.Instance.AddItem(item).Wait();
            var id = DataBaseContext.Instance.GetMagicItems().First(it => it.Name == item.Name).Id;
            var newItem = DataBaseContext.Instance.GetExtendedMagicById(id);
            Assert.AreEqual(item.Attunement, newItem.Attunement);
            Assert.AreEqual(item.Description, newItem.Description);
            Assert.AreEqual(item.OptionableText, newItem.OptionableText);
            Assert.AreEqual(item.UnderType, newItem.UnderType);
            Assert.AreEqual(item.UnderQuality, newItem.UnderQuality);
            Assert.AreEqual(item.Name, newItem.Name);
            

        }

    }
}