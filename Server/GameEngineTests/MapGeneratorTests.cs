using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebGameService.Models.EngineLogic;

namespace GameEngineTests
{
    [TestClass]
    public class MapGeneratorTests
    {

        [TestMethod]
        public void ArraySizeCreatedIsCorrect()
        {
            int size = 100;
            MapGenerator mg = new MapGenerator(size);
            var map = mg.GenerateMap();
            Assert.AreEqual(size,map.GetLength());
        }

       
    }
}
