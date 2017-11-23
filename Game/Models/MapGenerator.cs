using System;
using Game.Interfaces;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class MapGenerator
    {
        public int Size { get; set; }
        private static readonly Random Rnd=new Random();

        public MapGenerator(int s)
        {
            if(s <= 2) throw new IndexOutOfRangeException("Map is too small");
            Size = s;
        }
        public Map GenerateMap()
        {
            TypesOfObject[,] array = new TypesOfObject[Size, Size];
            GenerateBricks(array);
            GenerateFood(array);
            GenerateBaseAndUnit(array, TypesOfObject.UnitWhite, TypesOfObject.BaseWhite);
            GenerateBaseAndUnit(array, TypesOfObject.UnitBlack, TypesOfObject.BaseBlack);

            return new Map(array);
        }


        private static void GenerateBricks(TypesOfObject[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (ShouldIGenerateItem(Brick.Probability))
                        array[i, j] = TypesOfObject.Brick;
                }
            }
        }
        private static void GenerateFood(TypesOfObject[,] array)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    if (ShouldIGenerateItem(Food.Probability) && array[i, j] != TypesOfObject.Brick)
                        array[i, j] = TypesOfObject.Food;

                }
            }
        }

        private static void GenerateWhiteBaseAndUnit(TypesOfObject[,] array)
        {
            var x = Rnd.Next(1, array.GetLength(1)-1);
            var y = Rnd.Next(1, array.GetLength(0)-1);
            array[y, x] = TypesOfObject.BaseWhite;
            array[y+1, x] = TypesOfObject.UnitWhite;
           
        }

        private void GenerateBaseAndUnit(TypesOfObject[,] array, TypesOfObject unit, TypesOfObject Base)
        {
            while (true)
            {
                var x = Rnd.Next(1, array.GetLength(1) - 1);
                var y = Rnd.Next(1, array.GetLength(0) - 1);
                if (array[y, x] != TypesOfObject.FreeSpace && array[y, x] != TypesOfObject.Brick && array[y - 1, x] != TypesOfObject.Food )
                {
                    continue;
                }
                array[y, x] = Base;
                array[y - 1, x] = unit;
                break;
            }
        }

        private static bool ShouldIGenerateItem(double probability)
        {
            return !(Rnd.NextDouble() >= probability);
        }
    }
}