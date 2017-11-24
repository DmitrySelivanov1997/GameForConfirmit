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
        public IAlgoritm FirstAlgoritm { get; set; }
        public IAlgoritm SecondAlgoritm { get; set; }
        public int TurnNumber { get; set; }
        public int WaitTime { get; set; }
        public bool IsCanceled { get; set; } = false;
        public Rules Rules = new Rules();
        public MapManager MapManager { get; set; }

        public Engine(IAlgoritm firstAlgoritm, IAlgoritm secondAlgoritm, Map map)
        {
            MapManager= new MapManager(map);
            FirstAlgoritm = firstAlgoritm;
            SecondAlgoritm = secondAlgoritm;

        }

        public void Startbattle()
        {  
            while (!IsCanceled)
            {
                FirstAlgoritm.MoveAllUnits(MapManager.Map.Army.FindAll(x=>x.Fraction == TypesOfObject.UnitWhite));
                UpdateUnits(TypesOfObject.UnitWhite);
                UnitsAttackFoes();
                SecondAlgoritm.MoveAllUnits(MapManager.Map.Army.FindAll(x => x.Fraction == TypesOfObject.UnitBlack));
                UpdateUnits(TypesOfObject.UnitBlack);
                UnitsAttackFoes();
                TurnNumber++;
                Thread.Sleep(WaitTime);
            }
            
        }

        private void UnitsAttackFoes()
        {
            foreach (var unit in MapManager.Map.Army)
            {
                if (unit.DieOrSurvive())
                {
                    MapManager.UnitDied(unit);
                }
            }
            MapManager.Map.UpdateArmies();
            MapManager.CheckForGameOver();
            
        }
        public void UpdateUnits(TypesOfObject unit)
        {
            foreach (var unitToUpdate in MapManager.Map.Army)
            {
                if (unitToUpdate.Fraction == unit)
                {
                    var xNew = unitToUpdate.X;
                    var yNew = unitToUpdate.Y;
                    Rules.GetNewPositionAccordingToDirection(ref yNew, ref xNew, unitToUpdate);
                    if (!Rules.ShouldUnitsBeUpdated(MapManager.Map.GetItem(yNew, xNew))) continue;
                    if (MapManager.Map.GetItem(yNew, xNew) is Food)
                    {
                        MapManager.RemoveOldUnitAddNewOne(yNew, xNew, unitToUpdate);
                        MapManager.AddNewUnitNearBase(unitToUpdate.Fraction);
                    }
                    else
                        MapManager.RemoveOldUnitAddNewOne(yNew, xNew, unitToUpdate);
                }
            }
            MapManager.Map.UpdateArmies();
        }

        
    }
}
