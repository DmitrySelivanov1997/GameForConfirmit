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
        private Base BaseWhite { get; set; }
        private Base BaseBlack { get; set; }
        private TypesOfObject[,] Array { get; }
        public Map( TypesOfObject[,] array)
        {
            Array = array;
            WhiteArmy = GetArmy(Colors.White);
            BlackArmy = GetArmy(Colors.Black);
        }

    

        public BaseItem GetItem(int i, int j)
        {
            if (i >= 0 && j >= 0 && i < Array.GetLength(0) && j < Array.GetLength(1))
            {
                switch (Array[j,i])
                {
                    case TypesOfObject.Brick:
                        return new Brick(i, j);
                    case TypesOfObject.Food:
                        return new Food(i, j);
                    case TypesOfObject.FreeSpace:
                        return new FreeSpace(i, j);
                    case TypesOfObject.BaseBlack:
                        return new Base(i, j, Colors.Black);
                    case TypesOfObject.BaseWhite:
                        BaseWhite = new Base(i, j, Colors.White);
                        return BaseWhite;
                    case TypesOfObject.UnitBlack:
                        BaseBlack = new Base(i, j, Colors.Black);
                        return BaseBlack;
                    case TypesOfObject.UnitWhite:
                        return new UnitBase(i, j, Colors.White);
                }
                
            }
            return new Border(i,j);
           
        }

        public int GetLength()
        {
            return Array.GetLength(0);
        }

        private List<Unit> GetArmy(Color color)
        {
            var list = new List<Unit>();
            for (var i = 0; i < Array.GetLength(0); i++)
            {
                for (var j = 0; j < Array.GetLength(1); j++)
                {
                    if ((color == Colors.White && Array[i, j] == TypesOfObject.UnitWhite) || (color == Colors.Black && Array[i, j] == TypesOfObject.UnitBlack))
                    {
                        list.Add(new Unit(j,i,Colors.White,this));
                    }
                }
            }
            return list;
        }

        public void SetItem(int x, int y, TypesOfObject obj)
        {
            Array[y, x] = obj;
        }

        public void AddNewUnitNearBase(Color unitColor)
        {
            Base BaseTmp;
            BaseTmp = unitColor == Colors.White ? BaseWhite : BaseBlack;
            for (var i = BaseTmp.Y - 1; i <= BaseTmp.Y + 1; i++)
            {
                for (var j = BaseTmp.X - 1; j <= BaseTmp.X + 1; j++)
                {
                    if (i != j && (Array[i, j] == TypesOfObject.Food || Array[i, j] == TypesOfObject.FreeSpace))
                    {
                        Array[i, j] = TypesOfObject.UnitWhite;
                        WhiteArmy.Add(new Unit(j, i, Colors.White, this));
                    }
                }
            }
        }
    }
}
