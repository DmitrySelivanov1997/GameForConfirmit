using System;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class MapGenerator
    {

        private static readonly Random Rnd=new Random();
        public Map GenerateMap(int size)
        {
            var rnd=new Random();
            TypesOfObject[,] array = new TypesOfObject[size, size];
            GenerateBricks(array);
            GenerateFood(array);
            GenerateWhiteBaseAndUnit(array);
            GenerateBlackBaseAndUnit(array);
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
                    if (ShouldIGenerateItem(Food.Probability) && array[i, j] == TypesOfObject.Brick)
                        array[i, j] = TypesOfObject.Food;

                }
            }
        }

        private static void GenerateWhiteBaseAndUnit(TypesOfObject[,] array)
        {
            var x = Rnd.Next(1, array.GetLength(1));
            var y = Rnd.Next(1, array.GetLength(0) / 10);
            array[x, y] = TypesOfObject.BaseWhite;
            array[x, y+1] = TypesOfObject.UnitWhite;
           
        }
        private void GenerateBlackBaseAndUnit(TypesOfObject[,] array)
        {
            var x = Rnd.Next(1, array.GetLength(1));
            var y = Rnd.Next(array.GetLength(0) - array.GetLength(1) / 10, array.GetLength(0)-1);
            array[x, y] = TypesOfObject.BaseBlack;
            array[x, y-1] = TypesOfObject.UnitBlack;
        }

        private static bool ShouldIGenerateItem(double probability)
        {
            return !(Rnd.NextDouble() >= probability);
        }
    }
}