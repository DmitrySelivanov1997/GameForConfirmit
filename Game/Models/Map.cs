using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Game.Interfaces;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class Map
    {
        public List<IUnit> WhiteArmy;
        public List<IUnit> BlackArmy;
        public Base BaseWhite { get; set; }
        public Base BaseBlack { get; set; }
        public TypesOfObject[,] Array { get; }
        public Map( TypesOfObject[,] array)
        {
            Array = array;
            WhiteArmy = GetArmy(Colors.White);
            BlackArmy = GetArmy(Colors.Black);
        }

    

        public IItem GetItem(int y, int x)
        {
            if (y >= 0 && x >= 0 && y < Array.GetLength(0) && x < Array.GetLength(1))
            {
                switch (Array[y,x])
                {
                    case TypesOfObject.Brick:
                        return new Brick(y, x);
                    case TypesOfObject.Food:
                        return new Food(y, x);
                    case TypesOfObject.FreeSpace:
                        return new FreeSpace(y, x);
                    case TypesOfObject.BaseBlack:
                        BaseBlack = new Base(y, x, Colors.Black, this);
                        return BaseBlack;
                    case TypesOfObject.BaseWhite:
                        BaseWhite = new Base(y, x, Colors.White, this);
                        return BaseWhite;
                    case TypesOfObject.UnitBlack:
                        return new UnitBase(y, x, Colors.Black); ;
                    case TypesOfObject.UnitWhite:
                        return new UnitBase(y, x, Colors.White);
                }
                
            }
            return new Border(y,x);
           
        }

        public int GetLength()
        {
            return Array.GetLength(0);
        }

        private List<IUnit> GetArmy(Color color)
        {
            var list = new List<IUnit>();
            for (var i = 0; i < Array.GetLength(0); i++)
            {
                for (var j = 0; j < Array.GetLength(1); j++)
                {
                    if ((color == Colors.White && Array[i, j] == TypesOfObject.UnitWhite) || (color == Colors.Black && Array[i, j] == TypesOfObject.UnitBlack))
                    {
                        list.Add(new Unit(i,j,color,this));
                    }
                }
            }
            return list;
        }

        public void SetItem( int y, int x, TypesOfObject obj)
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
                        if (unitColor == Colors.White)
                        {
                            SetItem(i, j, TypesOfObject.UnitWhite);
                            WhiteArmy.Add(new Unit(i, j, Colors.White, this));
                            return;
                        }
                        SetItem(i, j, TypesOfObject.UnitBlack);
                        BlackArmy.Add(new Unit(i, j, Colors.Black, this));
                        return;
                    }
                }
            }
        }

        public void RemoveUnitFromArmy(IUnit unit)
        {
            if (unit.Color == Colors.White)
            {
                WhiteArmy.Remove(unit);
                return;
            }
            BlackArmy.Remove(unit);
        }
    }
}
