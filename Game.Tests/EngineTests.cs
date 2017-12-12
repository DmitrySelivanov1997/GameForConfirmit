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
    public class EngineTests
    {
        [TestMethod]
        public void UpdateUnit_MovesUnitToNewPositionIfFreeSpace()
        {
            bool result;
            var map = CommonTestPart.GenerateMap10X10WithFreeSpace();
            CommonTestPart.AddUnitOnMap(0,0,map,TypesOfObject.UnitBlack);
            var unit = map.Army.Find(x => x.TypeOfObject == TypesOfObject.UnitBlack);
            unit.Direction = Direction.Down;
            var engine = new Engine(new Algoritm2(), new Algoritm2(),map );
            engine.UpdateUnits(TypesOfObject.UnitBlack,new Statistics());
            if (unit.X == 0 && unit.Y == 1 && map.Array[1, 0] == TypesOfObject.UnitBlack)
                result = true;
            else result = false;
            Assert.IsTrue(result);
            
        }
    }
}
