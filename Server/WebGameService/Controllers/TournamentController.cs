using System.Threading.Tasks;
using System.Web.Http;
using CommonClient_WebServiseParts;
using WebGameService.Models.EngineLogic;

namespace WebGameService.Controllers
{
    public class TournamentController : ApiController
    {
        [HttpPost]
        public void StartAndLaunchNewTournament([FromBody] TournamentInitializingClass classForTournament)
        {
            if(Engine.IsGameAlive)
                return;

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
        }
        [HttpPut]
        public void PostDelayForEachTurn([FromBody]int delay)
        {
            Engine.WaitTime = delay; 
        }

        [HttpGet]
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

        [HttpDelete]
        public void StopTournament()
        {
            GameSessionManager.NumberOfGames = 0;
            Engine.IsGameAlive = false;
            while (GameSessionManager.Map != null)
            {
                //не возвращать результат, пока не удалится карта
            }
        }
    }
}
