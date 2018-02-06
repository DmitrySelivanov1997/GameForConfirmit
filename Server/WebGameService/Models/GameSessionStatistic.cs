using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceLibrary;
using WebGameService.Models.EngineLogic;

namespace WebGameService.Models
{
    public class GameSessionStatistic
    {
        public int Id { get; set; }
        public string GameStartTime { get; set; }
        public string GameDuration { get; set; }
        public int TurnsNumber { get; set; }
        public int MapSize { get; set; }
        public string GameResult { get; set; }
        public string WhiteAlgorithmName { get; set; }
        public string BlackAlgorithmName { get; set; }
    }
}