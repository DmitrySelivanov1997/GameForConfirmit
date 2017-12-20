using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using InterfaceLibrary;
using CommonClient_WebServiseParts;
using WebGameService.Models.EngineLogic;

namespace Game.Tests
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
