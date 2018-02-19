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
        public static int MapSize { get; set; }
        public static Map Map { get; set; }
        public static int NumberOfGames { get; set; }
        private static Engine Engine { get; set; }

        public GameSessionManager()
        {
            Engine = new Engine();
        }
        public void Startfight()
        {
            try
            {
                while (NumberOfGames > 0)
                {
                    var mapGenerator = new MapGenerator(MapSize);
                    Map = mapGenerator.GenerateMap();
                    Engine.MapManager = new MapManager(Map);
                    AlgorithmContainer.AlgorithmWhite =
                        (IAlgorithm)Activator.CreateInstance(AlgorithmContainer.AlgorithmWhite.GetType());
                    AlgorithmContainer.AlgorithmBlack =
                        (IAlgorithm)Activator.CreateInstance(AlgorithmContainer.AlgorithmBlack.GetType());
                    Engine.Startbattle();
                    NumberOfGames--;
                }

                Map = null;
            }
            catch (Exception e)
            {
                Trace.WriteLine(DateTime.Now + $", task error:{e}");
            }
        }
    }
}