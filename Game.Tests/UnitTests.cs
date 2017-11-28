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
    public class UnitTests
    {
        [TestMethod]
        public void DieOrSurvive_ReturnsTrueIfUnitHasMoreFoesThanAlliesInAttackRange()
        {
            var map = CommonTestPart.GenerateMap10X10WithFreeSpace();
            var baseTest = new BaseTests();
            CommonTestPart.AddUnitOnMap(0, 0,map,TypesOfObject.UnitBlack);
            CommonTestPart.AddUnitOnMap(0, 1, map, TypesOfObject.UnitWhite);
            CommonTestPart.AddUnitOnMap(1, 0, map, TypesOfObject.UnitWhite);
            var unit = map.Army.Find(x => x.Fraction == TypesOfObject.UnitBlack);
            var result = unit.DieOrSurvive();
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void DieOrSurvive_ReturnsFalseIfUnitHasEqualFoesAndAlliesInAttackRange()
        {
            var map = CommonTestPart.GenerateMap10X10WithFreeSpace();
            map.SetItem(0, 0, TypesOfObject.UnitBlack);
            map.SetItem(0, 1, TypesOfObject.UnitWhite);
            map.SetItem(1, 0, TypesOfObject.UnitWhite);
            map.SetItem(0, 2, TypesOfObject.UnitBlack);
            map.Army.Add(new Unit(0, 2, TypesOfObject.UnitBlack, map));
            map.Army.Add(new Unit(0, 0, TypesOfObject.UnitBlack, map));
            map.Army.Add(new Unit(0, 1, TypesOfObject.UnitWhite, map));
            map.Army.Add(new Unit(1, 0, TypesOfObject.UnitWhite, map));
            var unit = map.Army.Find(x => x.Fraction == TypesOfObject.UnitBlack);
            var result = unit.DieOrSurvive();
            Assert.IsFalse(result);
        }
    }
}
