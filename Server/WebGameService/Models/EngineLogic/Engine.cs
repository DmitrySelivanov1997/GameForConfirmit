using System;
using System.Threading;
using CommonClient_WebServiseParts;
using InterfaceLibrary;


namespace WebGameService.Models.EngineLogic
{
    public class Engine
    {
        public static bool Ct;
        public static Statistics WhiteArmyStatistics { get; set; } = new Statistics();
        public static Statistics BlackArmyStatistics { get; set; } = new Statistics();
        public static int WaitTime { get; set; }
        public Rules Rules = new Rules();
        public MapManager MapManager { get; set; }

        public Engine()
        {
            MapManager= new MapManager(GameSessionManager.Map);
            WhiteArmyStatistics = new Statistics();
            BlackArmyStatistics = new Statistics();
        }

        public void Startbattle()
        {
            WhiteArmyStatistics = new Statistics{NumberOfWins = WhiteArmyStatistics.NumberOfWins};
            BlackArmyStatistics = new Statistics{NumberOfWins = BlackArmyStatistics.NumberOfWins};
            while (true)
            {
                MakeATurn(AlgorithmContainer.AlgorithmWhite, TypesOfObject.UnitWhite, WhiteArmyStatistics);
                WhiteArmyStatistics.TurnNumber++;
                if (Ct)
                    return;
                MakeATurn(AlgorithmContainer.AlgorithmBlack, TypesOfObject.UnitBlack, BlackArmyStatistics);
                BlackArmyStatistics.TurnNumber++;
                if (Ct)
                    return;
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
                string gameResult = "";
                var result = MapManager.CheckForGameOver();
                Ct = true;
                if (result == GameResult.BlackArmyDestroyed || result == GameResult.BlackBaseDestroyed)
                {
                    if (result == GameResult.BlackArmyDestroyed)
                        gameResult = "Армия черных разбита";
                    else
                        gameResult = "База черных разбита";
                    GameSessionManager.WriteEndingDataForStatistic(WhiteArmyStatistics.TurnNumber,gameResult);
                    WhiteArmyStatistics.NumberOfWins++;
                }
                if (result == GameResult.WhiteArmyDestroyed || result == GameResult.WhiteBaseDestroyed)
                {
                    if (result == GameResult.WhiteArmyDestroyed)
                        gameResult = "Армия белых разбита";
                    else
                        gameResult = "База белых разбита";
                    GameSessionManager.WriteEndingDataForStatistic(BlackArmyStatistics.TurnNumber, gameResult);
                    BlackArmyStatistics.NumberOfWins++;
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
