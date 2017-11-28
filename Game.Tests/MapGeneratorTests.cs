using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game.Models;
using Game.Models.BaseItems;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using InterfaceLibrary;

namespace Game.Tests
{
    [TestClass]
    public class MapGeneratorTests
    {

        private static int GetNumberOfObjectsInArray(Map map1, TypesOfObject obj)
        {
            int sum = 0;
            for (var i = 0; i < map1.GetLength(); i++)
            {
                for (var j = 0; j < map1.GetLength(); j++)
                {
                    if (map1.Array[i, j] == obj)
                        sum++;
                }
            }
            return sum;
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "Map too small")]
        public void CanNotCreateMapLessThan2X2()
        {
            MapGenerator mg = new MapGenerator(2);
        }

        [TestMethod]
        public void ArraySizeCreatedIsCorrect()
        {
            int size = 100;
            MapGenerator mg = new MapGenerator(size);
            var map = mg.GenerateMap();
            Assert.AreEqual(size,map.GetLength());
        }

        [TestMethod]
        public void BricksCreatedSuccesfullyWith100Chance()
        {
            Brick.Probability = 100;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            var sum = GetNumberOfObjectsInArray(map1, TypesOfObject.Brick);
            Assert.AreEqual(sum,10*10-4);
        }
        [TestMethod]
        public void FoodCreatedSuccesfullyWith100Chance()
        {
            Brick.Probability = 0;
            Food.Probability = 100;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            var sum = GetNumberOfObjectsInArray(map1, TypesOfObject.Food);
            Assert.AreEqual(sum, 10 * 10 - 4);
        }
        [TestMethod]
        public void BricksWereNotCreatedWith0Chance()
        {
            Brick.Probability = 0;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            var sum = GetNumberOfObjectsInArray(map1, TypesOfObject.Brick);
            Assert.AreEqual(0, sum);
        }
        public void FoodWasNotCreatedWith0Chance()
        {
            Food.Probability = 0;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            var sum = GetNumberOfObjectsInArray(map1, TypesOfObject.Food);
            Assert.AreEqual(0, sum);
        }
        [TestMethod]
        public void UnitAndBaseSuccesfullyCreatedAndItsNumberIsOk()
        {
            var sum = 0;
            Brick.Probability = 100;
            Food.Probability = 100;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            sum = GetNumberOfObjectsInArray(map1, TypesOfObject.BaseBlack) +
                  GetNumberOfObjectsInArray(map1, TypesOfObject.BaseWhite) +
                  GetNumberOfObjectsInArray(map1, TypesOfObject.UnitBlack) +
                  GetNumberOfObjectsInArray(map1, TypesOfObject.UnitWhite);
            Assert.AreEqual(4, sum);
        }
    }
}
