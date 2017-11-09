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
        
        public int Size { get; set; }

        private TypesOfObject[,] _array;
        public Map(int size, TypesOfObject[,] array)
        {
            Size = size;
            _array = array;
        }

        public BaseItem GetItem(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < Size && j < Size)
            {
                switch (_array[i, j])
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
                        return new Unit(i, j, Colors.Black,this);
                    case TypesOfObject.UnitWhite:
                        return new Unit(i, j, Colors.White,this);
                }
                
            }
            return new Border(i,j);
           
        }
        
        
    }
}
