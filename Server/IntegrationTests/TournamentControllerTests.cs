using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonClient_WebServiseParts;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Threading;

namespace IntegrationTest
{    /// <summary>
    /// Testing of tournament controller functionality
    /// like the ability to start/cancell tournament, update database etc
    /// </summary>
    [TestClass]
    public class TournamentControllerTests
    {
        private readonly string _appPath = "http://co-yar-ws100:8080/";

        private async Task CancellTournament()
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync(_appPath + "api/tournament");
            }
        }
        private async Task StartTournament(int numberOfGames = 20, int mapSize = 100, int waitTime =1000)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(_appPath + "api/tournament/start/", new TournamentInitializingClass()
                {
                    MapSize = mapSize,
                    NumberOfGames = numberOfGames,
                    WaitTime = waitTime
                });
            }
        }
        private async Task<TournamentState> GetTournamentState()
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync(_appPath + "api/tournament");
                return JsonConvert.DeserializeObject<TournamentState>(json);
            }
        }
        [TestMethod]
        public async Task GetMapWhichIsNullWhenTournamentCancelled()
        {
            await CancellTournament();
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync(_appPath + "api/tournament");
                var map = JsonConvert.DeserializeObject<TournamentState>(json);
                Assert.IsNull(map.Map);
            }
        }
        [TestMethod]
        public async Task GetMapWhichIsNotNullWhenTournamentStarted()
        {
            await StartTournament();
            var map = GetTournamentState().Result;
            Assert.IsNotNull(map.Map);
            await CancellTournament();
        }
        [TestMethod]
        public async Task DatabaseUpdatedAfterGameFinished()
        {
            using (var client = new HttpClient())
            {
                var n = 1;
                await StartTournament(n);
                var response = await client.GetAsync(_appPath + "/api/statistic/count");
                var expected = response.Content.ReadAsAsync<int>().Result;
                await CancellTournament();
                var result = WhaitUntilDbChanged(expected, expected+n);
                Assert.IsTrue(result.Result);
            }
        }
        [TestMethod]
        public async Task NumberOfTurnsChangedAfterTime()
        {
            await StartTournament(1,50,0);
            var stats = GetTournamentState().Result;
            var start = stats.BlackStatistics.TurnNumber;
            var end = GetTournamentState().Result.BlackStatistics.TurnNumber;
            Assert.AreNotEqual(start,end);
        }

        private async Task<bool> WhaitUntilDbChanged(int expected, int real)
        {
            using (var client = new HttpClient())
            {
                var stopWatch = new Stopwatch();
                int actual;
                stopWatch.Start();
                do
                {
                    var response = await client.GetAsync(_appPath + "/api/statistic/count");
                    actual = response.Content.ReadAsAsync<int>().Result;
                    Thread.Sleep(100);
                } while (actual != real || stopWatch.ElapsedMilliseconds>1000);

                if (actual == real)
                    return true;
                return false;
            }
        }
    }
}
