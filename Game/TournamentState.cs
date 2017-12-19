using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Game.Models;
using InterfaceLibrary;

namespace WebGameService.Models
{
    public class TournamentState
    {
        public TypesOfObject[,] Map { get; set; }
        public Statistics WhiteStatistics { get; set; }
        public Statistics BlackStatistics { get; set; }
    }
}