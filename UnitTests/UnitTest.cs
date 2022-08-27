using App.Generator;
using App;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public partial class UnitTest1
    {
        [TestMethod]
        public void GemValidate()
        {
            const int GEM_COUNT = 1000;
            var gems = GemGenerator.GenerateGemsBy10(GEM_COUNT);
            int count = 0;
            int price = 0;
            foreach (var ge in gems)
            {
                count += ge.Count;
                price += ge.Price * ge.Count;
            }
            Assert.AreEqual(GEM_COUNT, count);
            Assert.AreEqual(GEM_COUNT * 10, price);
        }

        [TestMethod]
        public void MonsterSortValidate()
        {
            for (int i = 0; i < StaticValues.MonsterRate.Count; i++)
            {

                Assert.AreEqual(i.ToString(), StaticValues.MonsterRate.IndexOf(StaticValues.MonsterRate[i]).ToString());
            }
        }
    }
}
