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
        public Dictionary<IAlgoritm, TypesOfObject> DictionaryOfAlgoritms;
        public event Action<GameResult> GameOver;
        public int TurnNumber { get; set; }
        public int WaitTime { get; set; }
        public bool IsCanceled { get; set; } = false;
        public Rules Rules = new Rules();
        public MapManager MapManager { get; set; }

        public Engine(Dictionary<IAlgoritm,TypesOfObject>dictionary, Map map)
        {
            MapManager= new MapManager(map);
            DictionaryOfAlgoritms = dictionary;
        }

        public void Startbattle()
        {
            while (!IsCanceled)
            {
                foreach (var algoritm in DictionaryOfAlgoritms)
                {
                    MakeATurn(algoritm);
                }
                TurnNumber++;
                Thread.Sleep(WaitTime);
            }
            
        }

        private void MakeATurn(KeyValuePair<IAlgoritm, TypesOfObject> algoritm)
        {
            algoritm.Key.MoveAllUnits(MapManager.Map.Army.FindAll(x => x.TypeOfObject == algoritm.Value),MapManager.Map.GetLength());
            UpdateUnits(algoritm.Value);
            UnitsAttackFoes();
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
            MapManager.UpdateArmies();
            if(MapManager.CheckForGameOver()!= GameResult.NotAGameOver)
                GameOver?.Invoke(MapManager.CheckForGameOver());
            
        }
        public void UpdateUnits(TypesOfObject unit)
        {
            foreach (var unitToUpdate in MapManager.Map.Army)
            {
                if (unitToUpdate.TypeOfObject == unit)
                {
                    var xNew = unitToUpdate.X;
                    var yNew = unitToUpdate.Y;
                    Rules.GetNewPositionAccordingToDirection(ref yNew, ref xNew, unitToUpdate);
                    if (!Rules.ShouldUnitsBeUpdated(MapManager.Map.GetItem(yNew, xNew))) continue;
                    if (MapManager.Map.GetItem(yNew, xNew) is Food)
                    {
                        MapManager.MoveUnit(yNew, xNew, unitToUpdate);
                        MapManager.SpawnUnitNearBase(unitToUpdate.TypeOfObject);
                    }
                    else
                        MapManager.MoveUnit(yNew, xNew, unitToUpdate);
                }
            }
            MapManager.UpdateArmies();
        }

        
    }
}
