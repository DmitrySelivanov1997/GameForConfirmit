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
        public List<IUnitManagable> Army;
        public List<Base> BaseList;
        public TypesOfObject[,] Array { get; }
        public Map(int size)
        {
            Array = new TypesOfObject[size,size];
            Army = new List<IUnitManagable>();
            BufferArmy = new List<IUnitManagable>();
            BaseList = new List<Base>();
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
                        return new Base(y, x, TypesOfObject.BaseBlack, this);
                    case TypesOfObject.BaseWhite:
                        return new Base(y, x, TypesOfObject.BaseWhite, this); ;
                    case TypesOfObject.UnitBlack:
                        return new UnitBase(y, x, TypesOfObject.UnitBlack); 
                    case TypesOfObject.UnitWhite:
                        return new UnitBase(y, x, TypesOfObject.UnitWhite);
                }
                
            }
            return new Border(y,x);
           
        }

        public int GetLength()
        {
            return Array.GetLength(0);
        }
        public void SetItem( int y, int x, TypesOfObject obj)
        {
            Array[y, x] = obj;
            if(obj == TypesOfObject.BaseBlack || obj == TypesOfObject.BaseWhite) // adds base to list if needed
                BaseList.Add(new Base(y, x, obj,this));
        }

        public void RemoveUnitFromArmy(IUnitManagable unit)
        {
            BufferArmy.Add(unit);
        }

        public void AddUnitToArmy(IUnitManagable unit)
        {
            RemoveUnitFromArmy(unit);
        }

    }
}
