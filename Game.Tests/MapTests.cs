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
    /// <summary>
    /// Summary description for MapTests
    /// </summary>
    [TestClass]
    public class MapTests
    {
        public Map Map = new Map(10);
        [TestMethod]
        public void MapSizeIsCorrectAfterCreation()
        {
            var size = Map.Array.GetLength(0);
            Assert.AreEqual(size, 10);
        }
        [TestMethod]
        public void GetLengthReturnsCorrectLength()
        {
            var size = Map.GetLength();
            Assert.AreEqual(size, 10);
        }

        #region GetItemTests[TestMethod]
        public void GetItemReturnsBricks()
        {
            Map.Array[0, 0] = TypesOfObject.Brick;
            var item = Map.GetItem(0, 0);
            Assert.IsTrue(item is Brick);
        }
        [TestMethod]
        public void GetItemReturnsFood()
        {
            Map.Array[0, 0] = TypesOfObject.Food;
            var item = Map.GetItem(0, 0);
            Assert.IsTrue(item is Food);
        }
        [TestMethod]
        public void GetItemReturnsFreeSpace()
        {
            Map.Array[0, 0] = TypesOfObject.FreeSpace;
            var item = Map.GetItem(0, 0);
            Assert.IsTrue(item is FreeSpace);
        }
        [TestMethod]
        public void GetItemReturnsBase()
        {
            Map.Array[0, 0] = TypesOfObject.BaseBlack;
            var item = Map.GetItem(0, 0);
            Assert.IsTrue(item is Base);
        }
        [TestMethod]
        public void GetItemReturnsUnitBase()
        {
            Map.Array[0, 0] = TypesOfObject.UnitWhite;
            var item = Map.GetItem(0, 0);
            Assert.IsTrue(item is UnitBase);
        }
        [TestMethod]
        public void GetItemReturnsBorder()
        {
            Map.Array[0, 0] = TypesOfObject.UnitWhite;
            var item = Map.GetItem(-1, -1);
            Assert.IsTrue(item is Models.BaseItems.Border);
        }
        #endregion
        [TestMethod]
        public void SetItem_AddsCorrectItemInTheCorrectPlaceOfArray()
        {
            Map.SetItem(2,2,TypesOfObject.Brick);
            var result = Map.Array[2, 2] == TypesOfObject.Brick;
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void SetItem_AddsBaseToTheBaseList()
        {
            Map.BaseList.Clear();
            Map.SetItem(2,1, TypesOfObject.BaseBlack);
            var result = Map.BaseList.Count > 0;
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void RemoveUnit_AddsUnitTiBufferArmy()
        {
            Map.BufferArmy.Clear();
            var unit = new Unit(2,2, TypesOfObject.UnitBlack,Map);
            Map.RemoveUnitFromArmy(unit);
            var result = Map.BufferArmy.Contains(unit);
            Assert.IsTrue(result);
        }
    }
}
