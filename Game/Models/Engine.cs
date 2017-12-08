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
        public Statistics WhiteArmyStatistics = new Statistics();
        public Statistics BlackArmyStatistics = new Statistics();
        public IAlgorithm WhiteArmyAlgorithm;
        public IAlgorithm BlackArmyAlgorithm;
        public event Action<GameResult> GameOver;
        public int TurnNumber { get; set; }
        public int WaitTime { get; set; }
        public bool IsCanceled { get; set; } = false;
        public Rules Rules = new Rules();
        public MapManager MapManager { get; set; }

        public Engine(IAlgorithm whiteArmy, IAlgorithm blackArmy, Map map)
        {
            WhiteArmyAlgorithm = whiteArmy;
            BlackArmyAlgorithm = blackArmy;
            MapManager= new MapManager(map);
        }

        public void Startbattle(CancellationToken ct)
        {
            while (!IsCanceled)
            {
                MakeATurn(WhiteArmyAlgorithm, TypesOfObject.UnitWhite, WhiteArmyStatistics);
                if (ct.IsCancellationRequested)
                    return;
                MakeATurn(BlackArmyAlgorithm, TypesOfObject.UnitBlack, BlackArmyStatistics);
                if (ct.IsCancellationRequested)
                    return;
                TurnNumber++;
                Thread.Sleep(WaitTime);
            }
            
        }

        private void MakeATurn(IAlgorithm armyType, TypesOfObject unit, Statistics stats)
        {
            var army = MapManager.Map.Army.FindAll(x => x.TypeOfObject == unit);
            armyType.MoveAllUnits(army, MapManager.Map.GetLength());
            UpdateUnits(unit,stats);
            UnitsAttackFoes();
            stats.SetArmyNumber(army);
        }

        private void UnitsAttackFoes()
        {
            foreach (var unit in MapManager.Map.Army)
            {
                if (unit.DieOrSurvive())
                {
                    MapManager.UnitDied(unit);
                    if (unit.TypeOfObject == TypesOfObject.UnitBlack)
                        WhiteArmyStatistics.EnemyGotKilled();
                    else
                    {
                        BlackArmyStatistics.EnemyGotKilled();
                    }
                }
            }
            MapManager.UpdateArmies();
            if (MapManager.CheckForGameOver() != GameResult.NotAGameOver)
            {
                GameOver?.Invoke(MapManager.CheckForGameOver());
            }

        }
        public void UpdateUnits(TypesOfObject unit,Statistics stats)
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
                        stats.IncFood();
                    }
                    else
                        MapManager.MoveUnit(yNew, xNew, unitToUpdate);
                }
            }
            MapManager.UpdateArmies();
        }

        
    }
}
