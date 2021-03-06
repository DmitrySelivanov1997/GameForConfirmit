﻿using System.Collections.Generic;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class Map
    {
        public List<IUnitManagable> BufferArmy{ get; set; }
        public List<IUnitManagable> Army;
        public List<Base> BaseList;
        public TypesOfObject[,] Array { get; set; }
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
                        return new Brick(y, x, TypesOfObject.Brick);
                    case TypesOfObject.Food:
                        return new Food(y, x, TypesOfObject.Food);
                    case TypesOfObject.FreeSpace:
                        return new FreeSpace(y, x, TypesOfObject.FreeSpace);
                    case TypesOfObject.BaseBlack:
                        return new Base(y, x, TypesOfObject.BaseBlack, this);
                    case TypesOfObject.BaseWhite:
                        return new Base(y, x, TypesOfObject.BaseWhite, this); 
                    case TypesOfObject.UnitBlack:
                        return new UnitBase(y, x, TypesOfObject.UnitBlack); 
                    case TypesOfObject.UnitWhite:
                        return new UnitBase(y, x, TypesOfObject.UnitWhite);
                }
                
            }
            return new Border(y,x, TypesOfObject.Border);
           
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
