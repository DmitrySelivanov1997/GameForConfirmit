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

        public Task Startbattle()
        {
            return Task.Factory.StartNew(() =>
            {
                FirstAlgoritm.MoveAllUnits(WhiteArmy);
                UpdateUnits(WhiteArmy);
                SecondAlgoritm.MoveAllUnits(BlackArmy);
                UpdateUnits(BlackArmy);
            });
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
                        yNew = unit.Y + 1;
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
                        yNew = unit.Y - 1;
                        break;
                    case Direction.Stay:
                        continue;
                }
                if (Map.GetItem(yNew, xNew) is FreeSpace) // if there is free space below: replace unit with freespace, and move unit one cell down 
                {
                    RemoveOldUnitAddNewOne(yNew, xNew, unit);
                    Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
                    continue;
                }
                if (Map.GetItem(yNew, xNew) is Brick || Map.GetItem(yNew, xNew) is Border ||
                    Map.GetItem(yNew, xNew ) is Base || Map.GetItem(yNew, xNew) is UnitBase)
                {
                    unit.Direction = Direction.Stay;
                    continue;
                }
                if (Map.GetItem(yNew, xNew) is Food)
                {
                    RemoveOldUnitAddNewOne(yNew, xNew, unit);
                    Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
                    Map.AddNewUnitNearBase(unit.Color);
                }
            }
            if (army.Last().Color == Colors.White)
            {
                WhiteArmy = Map.WhiteArmy.ToArray();
                return;
            }
            BlackArmy = Map.BlackArmy.ToArray();

        }

        private void RemoveOldUnitAddNewOne( int y, int x, Unit unit)
        {
            Map.SetItem(y, x,  unit.Color == Colors.White ? TypesOfObject.UnitWhite : TypesOfObject.UnitBlack );
            if (unit.Color == Colors.White)
            {
                Map.WhiteArmy.Remove(unit);
                Map.WhiteArmy.Add(new Unit(y, x, unit.Color, Map));
                return;
            }
            Map.BlackArmy.Remove(unit);
            Map.BlackArmy.Add(new Unit(y, x, unit.Color, Map));
        }
    }
}
