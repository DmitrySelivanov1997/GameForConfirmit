using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonClient_WebServiseParts;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationTest
{
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
        private async Task StartTournament(int numberOfGames = 20)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(_appPath + "api/tournament/start/", new TournamentInitializingClass()
                {
                    MapSize = 100,
                    NumberOfGames = numberOfGames,
                    WaitTime = 1000
                });
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
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync(_appPath + "api/tournament");
                var map = JsonConvert.DeserializeObject<TournamentState>(json);
                Assert.IsNotNull(map.Map);
            }
            await CancellTournament();
        }
        [TestMethod]
        public async Task DatabaseUpdatedAfterGameFinished()
        {
            using (var client = new HttpClient())
            {
                await StartTournament(1);
                var response = await client.GetAsync(_appPath + "/api/statistic/count");
                var expected = response.Content.ReadAsAsync<int>().Result;
                await CancellTournament();
                response = await client.GetAsync(_appPath + "/api/statistic/count");
                var actual = response.Content.ReadAsAsync<int>().Result;
                Assert.AreEqual(expected+1,actual);
            }
        }
    }
}
