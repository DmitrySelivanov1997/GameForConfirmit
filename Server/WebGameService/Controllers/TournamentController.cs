﻿using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using CommonClient_WebServiseParts;
using WebGameService.Models.EngineLogic;

namespace WebGameService.Controllers
{
    public class TournamentController : ApiController
    {
        [System.Web.Http.HttpPost]
        public HttpStatusCodeResult StartAndLaunchNewTournament([FromBody] TournamentInitializingClass classForTournament)
        {
            GameSessionManager.MapSize = classForTournament.MapSize;
            Engine.WaitTime = classForTournament.WaitTime;
            GameSessionManager.NumberOfGames = classForTournament.NumberOfGames;
            Task.Factory.StartNew(() =>
            {
                new GameSessionManager().Startfight();
            });
            while (GameSessionManager.Map == null)
            {
                //не возвращать результат, пока не создастся карта
            }
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
                    WhiteStatistics = StatisticManager.WhiteArmyStatistics,
                    BlackStatistics = StatisticManager.BlackArmyStatistics
                };
            return new TournamentState
            {
                Map = GameSessionManager.Map.Array,
                WhiteStatistics = StatisticManager.WhiteArmyStatistics,
                BlackStatistics = StatisticManager.BlackArmyStatistics
            };
        }

        [System.Web.Http.HttpDelete]
        public void StopTournament()
        {
            GameSessionManager.NumberOfGames = 0;
            Engine.IsGameAlive = false;
        }
    }
}
