using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static Map Map { get; set; }
        public static int NumberOfGames { get; set; }
        public static int WaitTime { get; set; }
        public static Engine Engine { get; set; }
        public static GameSessionStatistic GameSessionStatistic { get; set; }

        public GameSessionManager()
        {
            Engine = new Engine();
            GameSessionStatistic = new GameSessionStatistic();
        }
        public void Startfight()
        {
            WriteStartingDataForStatistic();
            try
            {
                while (Map != null)
                {
                    AlgorithmContainer.AlgorithmWhite =
                        (IAlgorithm)Activator.CreateInstance(AlgorithmContainer.AlgorithmWhite.GetType());
                    AlgorithmContainer.AlgorithmBlack =
                        (IAlgorithm)Activator.CreateInstance(AlgorithmContainer.AlgorithmBlack.GetType());
                    Engine.Startbattle();
                    NumberOfGames--;
                    AddDataToDB();
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

        private void AddDataToDB()
        {
        }

        private void WriteStartingDataForStatistic()
        {
            GameSessionStatistic.WhiteAlgorithmName = AlgorithmContainer.AlgorithmWhite.GetType().FullName;
            GameSessionStatistic.BlackAlgorithmName = AlgorithmContainer.AlgorithmBlack.GetType().FullName;
            GameSessionStatistic.GameStartTime = DateTime.Now;
            GameSessionStatistic.MapSize = Map.Array.GetLength(0);
        }

        public static void WriteEndingDataForStatistic(int turnNumber, string gameResult)
        {
            GameSessionStatistic.TurnsNumber = turnNumber;
            GameSessionStatistic.GameResult = gameResult;
            GameSessionStatistic.GameDuration = DateTime.Now.Subtract(DateTime.Now.TimeOfDay);
        }
    }
}