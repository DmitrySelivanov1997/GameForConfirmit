﻿using System;
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

namespace WebGameService.Models.EngineLogic
{
    public class GameSessionManager
    {
        public static Map Map { get; set; }
        public static int NumberOfGames { get; set; }
        public static int WaitTime { get; set; }
        public static Engine Engine { get; set; }

        public GameSessionManager()
        {
            Engine = new Engine();
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
                    Engine.Startbattle();
                    NumberOfGames--;
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
    }
}