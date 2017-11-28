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
    class CommonTestPart
    {

        public static void AddUnitOnMap(int y, int x, Map map, TypesOfObject obj)
        {
            map.SetItem(y, x, obj);
            map.Army.Add(new Unit(y, x, obj, map));
        }

        public static Map GenerateMap10X10WithFreeSpace()
        {
            var map = new Map(10);
            for (int i = 0; i < map.GetLength(); i++)
            {
                for (int j = 0; j < map.GetLength(); j++)
                {
                    map.SetItem(i, j, TypesOfObject.FreeSpace);
                }
            }
            return map;
        }
    }
}
