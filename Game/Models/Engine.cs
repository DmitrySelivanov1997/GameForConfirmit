using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Game.Interfaces;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class Engine
    {
        public IPrinter Printer { get; set; }
        private Map Map { get; }
        public IReadOnlyCollection<Unit> WhiteArmy { get; set; }
        public IReadOnlyCollection<Unit> BlackArmy { get; set; }
        public IAlgoritm FirstAlgoritm { get; set; }
        public IAlgoritm SecondAlgoritm { get; set; }

        public Engine(IAlgoritm firstAlgoritm, IAlgoritm secondAlgoritm, Map map)
        {
            Map = map;
            FirstAlgoritm = firstAlgoritm;
            SecondAlgoritm = secondAlgoritm;
            WhiteArmy = map.WhiteArmy.ToArray();
            BlackArmy = map.BlackArmy.ToArray();
        }

        public void Startbattle()
        {
            for (var i=0;i<100;i++)
            {
                FirstAlgoritm.MoveAllUnits(WhiteArmy);
                UpdateUnits(WhiteArmy);
                SecondAlgoritm.MoveAllUnits(BlackArmy);
                UpdateUnits(Map.BlackArmy);

            }
        }

        private void UpdateUnits(IReadOnlyCollection<Unit> army)
        {
            int xNew=0, yNew=0;
            foreach (var unit in army)
            {
                switch (unit.Direction)
                {
                    case Direction.Down:
                        xNew = unit.X;
                        yNew = unit.Y - 1;
                        break;
                    case Direction.Left:
                        xNew = unit.X - 1;
                        yNew = unit.Y;
                        break;
                    case Direction.Right:
                        xNew = unit.X + 1;
                        yNew = unit.Y;
                        break;
                    case Direction.Up:
                        xNew = unit.X;
                        yNew = unit.Y + 1;
                        break;
                    case Direction.Stay:
                        continue;
                }
                if (Map.GetItem(xNew, yNew) is FreeSpace) // if there is free space below: replace unit with freespace, and move unit one cell down 
                {
                    RemoveOldUnitAddNewOne(xNew, yNew, unit);
                    Map.SetItem(unit.X, unit.Y, TypesOfObject.FreeSpace);
                    continue;
                }
                if (Map.GetItem(xNew, yNew) is Brick || Map.GetItem(xNew, yNew) is Border ||
                    Map.GetItem(xNew, yNew) is Base || Map.GetItem(xNew, yNew) is UnitBase)
                {
                    unit.Direction = Direction.Stay;
                    continue;
                }
                if (Map.GetItem(xNew, yNew) is Food)
                {
                    RemoveOldUnitAddNewOne(xNew, yNew, unit);
                    Map.SetItem(unit.X, unit.Y, TypesOfObject.FreeSpace);
                    Map.AddNewUnitNearBase(unit.Color);
                }
            }
            WhiteArmy = (army.Last().Color == Colors.White ? Map.WhiteArmy : Map.BlackArmy).ToArray();
        }

        private void RemoveOldUnitAddNewOne(int x, int y, Unit unit)
        {
            Map.SetItem(x, y, unit.Color == Colors.White ? TypesOfObject.UnitWhite : TypesOfObject.UnitBlack );
            Map.WhiteArmy.Remove(unit);
            Map.WhiteArmy.Add(new Unit(x, y, unit.Color, Map));
        }
    }
}
