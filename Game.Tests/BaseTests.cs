﻿using System;
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
    public class BaseTests
    {
        [TestMethod]
        public void BaseDiesIfNoAlliesInRangeAndFoeIsInRange()
        {
            var map = CommonTestPart.GenerateMap10X10WithFreeSpace();
            map.SetItem(0,0,TypesOfObject.BaseBlack);
            map.SetItem(0,3,TypesOfObject.UnitWhite);
            map.Army.Add(new Unit(0,3,TypesOfObject.UnitWhite,map));
            var result = new Base(0,0,TypesOfObject.BaseBlack,map).GetIsAlive();
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void BaseDoesNotDieIfThereIsAnAllyInRangeAndFoeIsInRange()
        {
            var map = CommonTestPart.GenerateMap10X10WithFreeSpace();
            map.SetItem(0, 0, TypesOfObject.BaseBlack);
            CommonTestPart.AddUnitOnMap(0,3,map,TypesOfObject.UnitWhite);
            CommonTestPart.AddUnitOnMap(0, 2, map, TypesOfObject.UnitWhite);
            CommonTestPart.AddUnitOnMap(1, 1, map, TypesOfObject.UnitBlack);
            var result = new Base(0, 0, TypesOfObject.BaseBlack, map).GetIsAlive();
            Assert.IsTrue(result);
        }

    }
}
