using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class Engine
    {
        public delegate void GameOverMessager(string message);
        public event GameOverMessager GameOver = delegate { };
        public Map Map { get; set; }
        public IReadOnlyCollection<IUnit> WhiteArmy { get; set; }
        public IReadOnlyCollection<IUnit> BlackArmy { get; set; }
        public IAlgoritm FirstAlgoritm { get; set; }
        public IAlgoritm SecondAlgoritm { get; set; }
        public int TurnNumber { get; set; }
        public int WaitTime { get; set; }
        public bool IsCanceled { get; set; } = false;
        public Rules Rules = new Rules();

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
            //for (int i=0; ; i++)
            while (!IsCanceled)
            {
                FirstAlgoritm.MoveAllUnits(WhiteArmy);
                UpdateUnits(WhiteArmy);
                UnitsAttackFoes(BlackArmy);
                UnitsAttackFoes(WhiteArmy);
                Thread.Sleep(WaitTime);
                SecondAlgoritm.MoveAllUnits(BlackArmy);
                UpdateUnits(BlackArmy);
                UnitsAttackFoes(WhiteArmy);
                UnitsAttackFoes(BlackArmy);
                TurnNumber++;
            }


        }

        private void UnitsAttackFoes(IReadOnlyCollection<IUnit> army)
        {
            var color = army.Last().Color;
            foreach (var unit in army)
            {
                if (unit.DieOrSurvive())
                {
                    Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
                    Map.RemoveUnitFromArmy(unit);
                }
            }
            UpdateArmy(army);
            var armyString = color == Colors.White ? "белых" : "черных";
            if (BlackArmy.Count == 0 || WhiteArmy.Count == 0)
                {
                    GameOver($"Армия {armyString} разбита");
                return;
                }
            if (!Map.BaseBlack.GetIsAlive())
                {
                    GameOver("База черных разбита");
                    return;
                }
            if (!Map.BaseWhite.GetIsAlive())
            {
                GameOver("База белых разбита");
            }
        }

        public void UpdateUnits(IReadOnlyCollection<IUnit> army)
        {
            foreach (var unit in army)
            {
                var xNew = unit.X;
                var yNew = unit.Y;
                Rules.GetNewPositionAccordingToDirection(ref yNew, ref xNew, unit);
                if (Rules.ShouldUnitsBeUpdated(Map.GetItem(yNew, xNew)))
                {
                    RemoveOldUnitAddNewOne(yNew, xNew, unit);
                }
                if (Map.GetItem(yNew, xNew) is Food)
                    Map.AddNewUnitNearBase(unit.Color);
                
            }
            UpdateArmy(army);
        }

        private void UpdateArmy(IReadOnlyCollection<IUnit> army)
        {
            if (army.Last().Color == Colors.White)
            {
                WhiteArmy = Map.WhiteArmy.ToArray();
                return;
            }
            BlackArmy = Map.BlackArmy.ToArray();
        }

        private void RemoveOldUnitAddNewOne( int y, int x, IUnit unit)
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
            Map.SetItem(unit.Y, unit.X, TypesOfObject.FreeSpace);
        }
    }
}
