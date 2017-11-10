using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Game.Interfaces;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class Map
    {
        public List<Unit> WhiteArmy = new List<Unit>();
        public List<Unit> BlackArmy = new List<Unit>();
        private TypesOfObject[,] Array { get; }
        public Map( TypesOfObject[,] array)
        {
            Array = array;
        }

        public BaseItem GetItem(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < Array.GetLength(0) && j < Array.GetLength(1))
            {
                switch (Array[i, j])
                {
                    case TypesOfObject.Brick:
                        return new Brick(i,j);
                    case TypesOfObject.Food:
                        return new Food(i, j);
                    case TypesOfObject.FreeSpace:
                        return new FreeSpace(i, j);
                    case TypesOfObject.BaseBlack:
                        return new Base(i, j, Colors.Black);
                    case TypesOfObject.BaseWhite:
                        return new Base(i, j, Colors.White);
                    case TypesOfObject.UnitBlack:
                        BlackArmy.Add(new Unit(i, j, Colors.Black, this));
                        return new UnitBase(i, j, Colors.Black);
                    case TypesOfObject.UnitWhite:
                        WhiteArmy.Add(new Unit(i,j, Colors.White,this));
                        return new UnitBase(i, j, Colors.White);
                }
                
            }
            return new Border(i,j);
           
        }

        public int GetLength()
        {
            return Array.GetLength(0);
        }
        
        
    }
}
