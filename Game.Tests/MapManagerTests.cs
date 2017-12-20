using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommonClient_WebServiseParts;
using InterfaceLibrary;
using WebGameService.Models.EngineLogic;

namespace Game.Tests
{
    /// <summary>
    /// Summary description for MapManagerTests
    /// </summary>
    [TestClass]
    public class MapManagerTests
    {
        public MapManager MapManager = new MapManager(new Map(10));

        [TestMethod]
        public void MoveUnit_ChangesTheCoordinatesOfUnit()
        {
            bool result;
            var unit = new Unit(2,2,TypesOfObject.UnitBlack,MapManager.Map);
            MapManager.MoveUnit(4,4,unit);
            if (unit.Y == 4 && unit.X == 4)
                result = true;
            else result = false;
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void MoveUnit_SetsUnitInTheCorrectSpotOnMap()
        {
            MapManager.Map = new Map(10);
            var unit = new Unit(2, 2, TypesOfObject.UnitBlack, MapManager.Map);
            MapManager.MoveUnit(4, 4, unit);
            var result = MapManager.Map.Array[4,4] ==TypesOfObject.UnitBlack;
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void MoveUnit_LeavesFreeSpaceOnTheFormerUnitPozition()
        {
            MapManager.Map = new Map(10);
            var unit = new Unit(2, 2, TypesOfObject.UnitBlack, MapManager.Map);
            MapManager.MoveUnit(4, 4, unit);
            var result = MapManager.Map.Array[2, 2] == TypesOfObject.FreeSpace;
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CheckForGameOver_ReturnsBlackBaseDestroyed()
        {
            var map = new Map(5);
            map.SetItem(0,0,TypesOfObject.BaseBlack);
            map.SetItem(0,1,TypesOfObject.UnitWhite);
            map.AddUnitToArmy(new Unit(2,2,TypesOfObject.UnitBlack,map));
            map.AddUnitToArmy(new Unit(0, 1, TypesOfObject.UnitWhite, map));
            MapManager.Map = map;
            MapManager.UpdateArmies();
            var result = MapManager.CheckForGameOver();
            Assert.AreEqual(GameResult.BlackBaseDestroyed,result);
        }
        [TestMethod]
        public void CheckForGameOver_ReturnsBlackArmyDestroyed()
        {
            var map = new Map(5);
            map.AddUnitToArmy(new Unit(0, 1, TypesOfObject.UnitWhite, map));
            MapManager.Map = map;
            MapManager.UpdateArmies();
            var result = MapManager.CheckForGameOver();
            Assert.AreEqual(GameResult.BlackArmyDestroyed, result);
        }

        [TestMethod]
        public void UpdateArmies_AddUnitToArmyFromBuffer()
        {
            MapManager.Map = new Map(10);
            var unit = new Unit(0,0,TypesOfObject.UnitBlack,MapManager.Map);
            MapManager.Map.AddUnitToArmy(unit);
            MapManager.UpdateArmies();
            Assert.IsTrue(MapManager.Map.Army.Contains(unit));
        }
        [TestMethod]
        public void UpdateArmies_RemoveUnitFromArmy()
        {
            MapManager.Map = new Map(10);
            var unit = new Unit(0, 0, TypesOfObject.UnitBlack, MapManager.Map);
            MapManager.Map.AddUnitToArmy(unit);
            MapManager.UpdateArmies();
            MapManager.Map.RemoveUnitFromArmy(unit);
            MapManager.UpdateArmies();
            Assert.IsFalse(MapManager.Map.Army.Contains(unit));
        }

    }
}
