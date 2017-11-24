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
        public List<IUnitManagable> BufferArmy{ get; set; }
        public IReadOnlyCollection<IUnit> WhiteArmyReadOnlyCollection { get; set; }
        public IReadOnlyCollection<IUnit> BlackArmyReadOnlyCollection { get; set; }
        public List<IUnitManagable> WhiteArmy;
        public List<IUnitManagable> BlackArmy;
        public Base BaseWhite { get; set; }
        public Base BaseBlack { get; set; }
        public TypesOfObject[,] Array { get; }
        public Map( TypesOfObject[,] array)
        {
            Array = array;
            WhiteArmy = GetArmy(TypesOfObject.UnitWhite);
            BlackArmy = GetArmy(TypesOfObject.UnitBlack);
            BufferArmy = new List<IUnitManagable>();
            WhiteArmyReadOnlyCollection = WhiteArmy;
            BlackArmyReadOnlyCollection = BlackArmy;
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

        private List<IUnitManagable> GetArmy(TypesOfObject unitColor)
        {
            var list = new List<IUnitManagable>();
            for (var i = 0; i < Array.GetLength(0); i++)
            {
                for (var j = 0; j < Array.GetLength(1); j++)
                {
                    if (Array[i,j] == unitColor)
                    {
                        list.Add(new Unit(i, j, unitColor == TypesOfObject.UnitWhite? Colors.White: Colors.Black,this));
                    }
                }
            }
            return list;
        }

        public void SetItem( int y, int x, TypesOfObject obj)
        {
            Array[y, x] = obj;
        }

        public void RemoveUnitFromArmy(IUnitManagable unit)
        {
            BufferArmy.Add(unit);
        }

        public void AddUnitToArmy(TypesOfObject unit, int i, int j)
        {
            BufferArmy.Add(new Unit(i, j, Colors.White, this));
        }

        public void UpdateArmies()
        {
            foreach (var unitManagable in BufferArmy)
            {
                if (unitManagable.GetFraction() == TypesOfObject.UnitWhite)
                {
                    if (!WhiteArmy.Contains(unitManagable))
                        WhiteArmy.Add(unitManagable);
                    else
                        WhiteArmy.Remove(unitManagable);
                }
                else
                {
                    if (!BlackArmy.Contains(unitManagable))
                        BlackArmy.Add(unitManagable);
                    else
                        BlackArmy.Remove(unitManagable);
                }
            }
            BufferArmy.Clear();
        }
    }
}
