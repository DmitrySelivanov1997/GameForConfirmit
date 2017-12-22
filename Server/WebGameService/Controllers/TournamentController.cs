using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CommonClient_WebServiseParts;
using InterfaceLibrary;
using WebGameService.Models;
using WebGameService.Models.EngineLogic;

namespace WebGameService.Controllers
{
    public class TournamentController : ApiController
    {
        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult StartAndLaunchNewTournament([FromBody] TournamentInitializingClass classForTournament)
        {
            Engine.Ct = false;
            var mapGenerator = new MapGenerator(classForTournament.MapSize);
            GameSessionManager.Map = mapGenerator.GenerateMap();
            Engine.WaitTime = classForTournament.WaitTime;
            GameSessionManager.NumberOfGames = classForTournament.NumberOfGames;
            new GameSessionManager().Startfight();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [System.Web.Http.HttpPut]
        public void PostDelayForEachTurn([FromBody]int delay)
        {
            Engine.WaitTime = delay;
        }

        [System.Web.Http.HttpGet]
        public TournamentState GetTournamentState()
        {
            if(GameSessionManager.Map == null)
                return new TournamentState
                {
                    WhiteStatistics = Engine.WhiteArmyStatistics,
                    BlackStatistics = Engine.BlackArmyStatistics
                };
            return new TournamentState
            {
                Map = GameSessionManager.Map.Array,
                WhiteStatistics = Engine.WhiteArmyStatistics,
                BlackStatistics = Engine.BlackArmyStatistics
            };
        }

        [System.Web.Http.HttpDelete]
        public void StopTournament()
        {
            GameSessionManager.NumberOfGames = 0;
            Engine.Ct = true;
        }
    }
}
