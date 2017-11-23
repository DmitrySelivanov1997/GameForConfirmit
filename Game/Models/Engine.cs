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
        public MapManager MapManager { get; set; }
        public IAlgoritm FirstAlgoritm { get; set; }
        public IAlgoritm SecondAlgoritm { get; set; }
        public int TurnNumber { get; set; }
        public int WaitTime { get; set; }
        public bool IsCanceled { get; set; } = false;
        public Rules Rules = new Rules();

        public Engine(IAlgoritm firstAlgoritm, IAlgoritm secondAlgoritm, Map map)
        {
            MapManager= new MapManager(map);
            FirstAlgoritm = firstAlgoritm;
            SecondAlgoritm = secondAlgoritm;

        }

        public void Startbattle()
        {
            //for (int i=0; ; i++)
            while (!IsCanceled)
            {
                FirstAlgoritm.MoveAllUnits(MapManager.Map.WhiteArmyReadOnlyCollection);
                UpdateUnits(MapManager.Map.WhiteArmy);
                UnitsAttackFoes(MapManager.Map.BlackArmy);
                UnitsAttackFoes(MapManager.Map.WhiteArmy);
                SecondAlgoritm.MoveAllUnits(MapManager.Map.BlackArmyReadOnlyCollection);
                UpdateUnits(MapManager.Map.BlackArmy);
                UnitsAttackFoes(MapManager.Map.WhiteArmy);
                UnitsAttackFoes(MapManager.Map.BlackArmy);
                TurnNumber++;
                Thread.Sleep(WaitTime);
            }


        }

        private void UnitsAttackFoes(List<IUnitManagable> army)
        {
            foreach (var unit in army)
            {
                if (unit.DieOrSurvive())
                {
                    MapManager.UnitDied(unit);
                }
            }
            MapManager.Map.UpdateArmies();
            MapManager.CheckForGameOver();
            
        }
        public void UpdateUnits(List<IUnitManagable> army)
        {
            foreach (var unit in army)
            {
                var xNew = unit.X;
                var yNew = unit.Y;
                Rules.GetNewPositionAccordingToDirection(ref yNew, ref xNew, unit);
                if (Rules.ShouldUnitsBeUpdated(MapManager.Map.GetItem(yNew, xNew)))
                {
                    if (MapManager.Map.GetItem(yNew, xNew) is Food)
                    {
                        MapManager.RemoveOldUnitAddNewOne(yNew, xNew, unit);
                        MapManager.AddNewUnitNearBase(unit.GetFraction());
                    }
                    else
                        MapManager.RemoveOldUnitAddNewOne(yNew, xNew, unit);

                }

            }
            MapManager.Map.UpdateArmies();
        }

        
    }
}
