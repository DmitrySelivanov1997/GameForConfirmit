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
        public void BricksAndFoodCreatedSuccesfullyWith100Chance()
        {
            int sum = 0;
            Brick.Probability = 100;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            Brick.Probability = 0;
            Food.Probability = 100;
            var mg2 = new MapGenerator(10);
            var map2 = mg2.GenerateMap();
            for (var i = 0; i < map1.GetLength(); i++)
            {
                for (var j = 0; j < map1.GetLength(); j++)
                {
                    if (map1.Array[i, j] is TypesOfObject.Brick)
                        sum++;
                }
            }
            for (var i = 0; i < map2.GetLength(); i++)
            {
                for (var j = 0; j < map2.GetLength(); j++)
                {
                    if (map2.Array[i, j] is TypesOfObject.Food)
                        sum++;
                }
            }
            Assert.AreEqual(2*10*10-8,sum);
        }

        [TestMethod]
        public void FoodCanNotOverrideBrick()
        {
            var sum = 0;
            Brick.Probability = 100;Food.Probability = 100;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            for (var i = 0; i < map1.GetLength(); i++)
            {
                for (var j = 0; j < map1.GetLength(); j++)
                {
                    if (map1.Array[i, j] is TypesOfObject.Food)
                        sum++;
                }
            }

            Assert.AreEqual(0, sum);
            
        }

        [TestMethod]
        public void BrickAndFoodWereNotCreatedWith0Chance()
        {
            var sum = 0;
            Brick.Probability = 0;
            Food.Probability = 0;
            var mg1 = new MapGenerator(10);
            var map1 = mg1.GenerateMap();
            for (var i = 0; i < map1.GetLength(); i++)
            {
                for (var j = 0; j < map1.GetLength(); j++)
                {
                    if (map1.Array[i, j] is TypesOfObject.Brick)
                        sum++;
                }
            }
            for (var i = 0; i < map1.GetLength(); i++)
            {
                for (var j = 0; j < map1.GetLength(); j++)
                {
                    if (map1.Array[i, j] is TypesOfObject.Food)
                        sum++;
                }
            }
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
            for (var i = 0; i < map1.GetLength(); i++)
            {
                for (var j = 0; j < map1.GetLength(); j++)
                {
                    if (map1.Array[i, j] is TypesOfObject.UnitWhite || map1.Array[i, j] is TypesOfObject.BaseWhite || map1.Array[i, j] is TypesOfObject.UnitBlack || map1.Array[i, j] is TypesOfObject.BaseBlack)
                        sum++;
                }
            }
            Assert.AreEqual(4, sum);
        }
    }
}
