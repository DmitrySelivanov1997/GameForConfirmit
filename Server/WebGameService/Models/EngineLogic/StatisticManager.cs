using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonClient_WebServiseParts;

namespace WebGameService.Models.EngineLogic
{
    public class StatisticManager
    {
        public static Statistics WhiteArmyStatistics { get; set; } = new Statistics();
        public static Statistics BlackArmyStatistics { get; set; } = new Statistics();
        public static GameSessionStatistic GameSessionStatistic { get; set; }
        private static ISessionRepository _repository = new SessionRepository(new GameSessionStatisticContext());
        public StatisticManager()
        {
            WhiteArmyStatistics = new Statistics();
            BlackArmyStatistics = new Statistics();
            GameSessionStatistic = new GameSessionStatistic();
        }

        public StatisticManager(StatisticManager statisticManager)
        {
            WhiteArmyStatistics = new Statistics(){NumberOfWins = WhiteArmyStatistics.NumberOfWins};
            BlackArmyStatistics = new Statistics(){NumberOfWins = BlackArmyStatistics.NumberOfWins};
            GameSessionStatistic = new GameSessionStatistic();
        }


        public static void WriteEndingDataForStatistic( GameResult gameResult)
        {
            GameSessionStatistic.TurnsNumber = WhiteArmyStatistics.TurnNumber;
            GameSessionStatistic.GameResult = GetGameResult(gameResult);
            GameSessionStatistic.GameDuration = DateTime.Now.Subtract(GameSessionStatistic.GameStartTime.TimeOfDay).ToString("mm:ss,fffff");
            AddToDataBase();
        }

        private static void AddToDataBase()
        {
            _repository.Add(GameSessionStatistic);
        }

        public void WriteStartingDataForStatistic()
        {
            GameSessionStatistic.WhiteAlgorithmName = AlgorithmContainer.AlgorithmWhite.GetType().FullName;
            GameSessionStatistic.BlackAlgorithmName = AlgorithmContainer.AlgorithmBlack.GetType().FullName;
            GameSessionStatistic.GameStartTime = DateTime.Now;
            GameSessionStatistic.MapSize = GameSessionManager.MapSize;
        }
        private static string GetGameResult(GameResult gameResult)
        {
            switch (gameResult)
            {
                case GameResult.BlackArmyDestroyed:
                    return "Армия черных разбита";
                case GameResult.BlackBaseDestroyed:
                    return "База черных разбита";
                case GameResult.WhiteArmyDestroyed:
                    return "Армия белых разбита";
                case GameResult.WhiteBaseDestroyed:
                    return "База белых разбита";
                default:
                    return "Игра отменена";
            }
        }
    }
}