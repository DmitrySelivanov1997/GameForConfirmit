using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Windows.Threading;
using CommonClient_WebServiseParts;
using InterfaceLibrary;
using WebGameService.Controllers;

namespace WebGameService.Models.EngineLogic
{
    public class GameSessionManager
    {
        private readonly ISessionRepository _repository;
        public static Map Map { get; set; }
        public static int NumberOfGames { get; set; }
        public static int WaitTime { get; set; }
        private static Engine Engine { get; set; }
        private static GameSessionStatistic GameSessionStatistic { get; set; }

        public GameSessionManager(ISessionRepository repository)
        {
            _repository = repository;
            Engine = new Engine();
            GameSessionStatistic = new GameSessionStatistic();
        }
        public void Startfight()
        {
            try
            {
                while (Map != null)
                {
                    AlgorithmContainer.AlgorithmWhite =
                        (IAlgorithm)Activator.CreateInstance(AlgorithmContainer.AlgorithmWhite.GetType());
                    AlgorithmContainer.AlgorithmBlack =
                        (IAlgorithm)Activator.CreateInstance(AlgorithmContainer.AlgorithmBlack.GetType());
                    WriteStartingDataForStatistic();
                    Engine.Startbattle();
                    NumberOfGames--;
                    AddDataToDb();
                    if (NumberOfGames > 0)
                    {
                        var mapGenerator = new MapGenerator(Map.GetLength());
                        Map = mapGenerator.GenerateMap();
                        Engine.MapManager = new MapManager(Map);
                        Engine.Ct = false;
                    }
                    else
                    {
                        Map = null;
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(DateTime.Now + $", task error:{e}");
            }
        }

        private void AddDataToDb()
        {
            _repository.Add(GameSessionStatistic);
        }

        private void WriteStartingDataForStatistic()
        {
            GameSessionStatistic.WhiteAlgorithmName = AlgorithmContainer.AlgorithmWhite.GetType().FullName;
            GameSessionStatistic.BlackAlgorithmName = AlgorithmContainer.AlgorithmBlack.GetType().FullName;
            GameSessionStatistic.GameStartTime = DateTime.Now;
            GameSessionStatistic.MapSize = Map.Array.GetLength(0);
        }

        public static void WriteEndingDataForStatistic(int turnNumber, GameResult gameResult)
        {
            GameSessionStatistic.TurnsNumber = turnNumber;
            GameSessionStatistic.GameResult = GetGameResult(gameResult);
            GameSessionStatistic.GameDuration = DateTime.Now.Subtract(GameSessionStatistic.GameStartTime.TimeOfDay).ToString("mm:ss,fffff");
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