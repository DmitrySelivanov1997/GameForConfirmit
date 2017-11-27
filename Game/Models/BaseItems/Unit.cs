﻿using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using InterfaceLibrary;

namespace Game.Models.BaseItems
{
    public class Unit:UnitBase, IUnitManagable
    {
        public Direction Direction { get; set; }
        public IItem[,] ScopeArray { get; set; }
        private Map Map { get; }
        public Unit(int i, int j, TypesOfObject obj, Map map) : base(i, j, obj)
        {
            Map = map;
            ScopeArray = GetAllObjectsInScopeArray();

        }
        
        

        private IItem[,] GetAllObjectsInScopeArray()
        {
            int x = 0;
            int y = 0;
            var array = new IItem[13,13];
            for (var i = Y - 6; i <= Y + 6; i++)
            {
                for (var j = X - 6; j <= X + 6; j++)
                {
                    if ((Math.Abs(i - Y )+Math.Abs(j - X)) <= 6)
                    {
                        if (x != 6 || y != 6)
                            array[y,x] = Map.GetItem(i, j);
                    }

                    x++;
                }
                x = 0;
                y++;

            }
            return array;
        }

        public void Move(Direction direction)
        {
            Direction = direction;
        }


        public bool DieOrSurvive()
        {
            ScopeArray = GetAllObjectsInScopeArray();
            var allies=1;
            var foes = 0;
            foreach (var item in ScopeArray)
            {
                if (item is UnitBase)
                {
                    if (Math.Abs(item.X - X) + Math.Abs(item.Y - Y) <= 3)
                    {
                        if (Color == item.Color)
                            allies++;
                        else
                            foes++;
                    }
                }
            }
            return foes > allies;
        }
    }
}
