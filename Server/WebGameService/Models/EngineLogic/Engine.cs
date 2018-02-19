using System;
using System.Threading;
using CommonClient_WebServiseParts;
using InterfaceLibrary;

namespace WebGameService.Models.EngineLogic
{
    public class Engine
    {
        public static bool IsGameAlive;
        private StatisticManager StatisticManager { get; set; }
        public static int WaitTime { private get; set; }
        private readonly Rules _rules = new Rules();
        public MapManager MapManager { private get; set; }

        public Engine()
        {
            MapManager= new MapManager(GameSessionManager.Map);
            StatisticManager = new StatisticManager();
        }

        public void Startbattle()
        {
            IsGameAlive = true;
            StatisticManager = new StatisticManager(StatisticManager);
            StatisticManager.WriteStartingDataForStatistic();
            while (true)
            {
                MakeATurn(AlgorithmContainer.AlgorithmWhite, TypesOfObject.UnitWhite, StatisticManager.WhiteArmyStatistics);
                StatisticManager.WhiteArmyStatistics.TurnNumber++;
                if (!IsGameAlive)
                {
                    StatisticManager.WriteEndingDataForStatistic(MapManager.CheckForGameOver());
                    return;
                }
                MakeATurn(AlgorithmContainer.AlgorithmBlack, TypesOfObject.UnitBlack, StatisticManager.BlackArmyStatistics);
                StatisticManager.BlackArmyStatistics.TurnNumber++;
                if (!IsGameAlive)
                {
                    StatisticManager.WriteEndingDataForStatistic(MapManager.CheckForGameOver());
                    return;
                }
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
                        StatisticManager.WhiteArmyStatistics.EnemyGotKilled();
                    else
                    {
                        StatisticManager.BlackArmyStatistics.EnemyGotKilled();
                    }
                }
            }
            MapManager.UpdateArmies();
            if (MapManager.CheckForGameOver() != GameResult.NotAGameOver)
            {
                var result = MapManager.CheckForGameOver();
                IsGameAlive = false;
                if (result == GameResult.BlackArmyDestroyed || result == GameResult.BlackBaseDestroyed)
                {
                    StatisticManager.WhiteArmyStatistics.NumberOfWins++;
                }
                if (result == GameResult.WhiteArmyDestroyed || result == GameResult.WhiteBaseDestroyed)
                {
                    StatisticManager.BlackArmyStatistics.NumberOfWins++;
                }
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
                    _rules.GetNewPositionAccordingToDirection(ref yNew, ref xNew, unitToUpdate);
                    if (!_rules.ShouldUnitsBeUpdated(MapManager.Map.GetItem(yNew, xNew))) continue;
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
