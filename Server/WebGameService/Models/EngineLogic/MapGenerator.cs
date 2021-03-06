﻿using System;
using System.IO;
using CommonClient_WebServiseParts;
using InterfaceLibrary;

namespace WebGameService.Models.EngineLogic
{
    public class MapGenerator
    {
        public Map Map { get; set; }
        public int Size { get; set; }
        private static readonly Random Rnd=new Random();

        public MapGenerator(int s)
        {
            Size = s;
            Map = new Map(Size);
        }
        public Map GenerateMap()
        {
            GenerateBricks();
            GenerateFood();
            GenerateBaseAndUnit(TypesOfObject.UnitWhite, TypesOfObject.BaseWhite);
            GenerateBaseAndUnit(TypesOfObject.UnitBlack, TypesOfObject.BaseBlack);
            return Map;
        }


        private void GenerateBricks()
        {
            for (var i = 0; i < Map.GetLength(); i++)
            {
                for (var j = 0; j < Map.GetLength(); j++)
                {
                    if (ShouldIGenerateItem(Properties.Settings.Default.BrickProbability))
                        Map.SetItem(i,j,TypesOfObject.Brick);
                }
            }
        }
        private void GenerateFood()
        {
            for (var i = 0; i < Map.GetLength(); i++)
            {
                for (var j = 0; j < Map.GetLength(); j++)
                {
                    if (ShouldIGenerateItem(Properties.Settings.Default.FoodProbability))
                        Map.SetItem(i, j, TypesOfObject.Food);

                }
            }
        }

        private void GenerateBaseAndUnit(TypesOfObject unit, TypesOfObject Base)
        {
            while (true)
            {
                var x = Rnd.Next(1, Map.GetLength() - 1);
                var y = Rnd.Next(1, Map.GetLength() - 1);
                if ((Map.Array[y, x] != TypesOfObject.FreeSpace && Map.Array[y, x] != TypesOfObject.Brick && Map.Array[y, x] != TypesOfObject.Food)
                   || (Map.Array[y-1, x] != TypesOfObject.FreeSpace && Map.Array[y-1, x] != TypesOfObject.Brick && Map.Array[y-1, x] != TypesOfObject.Food) )
                {
                    continue;
                }
                Map.SetItem(y,x,Base);
                Map.SetItem(y-1, x, unit);
                Map.Army.Add(new Unit(y-1, x , unit, Map));
                break;
            }
        }

        private static bool ShouldIGenerateItem(double probability)
        {
            return !(Rnd.NextDouble() >= probability);
        }

    }
}