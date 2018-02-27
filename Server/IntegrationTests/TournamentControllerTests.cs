using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonClient_WebServiseParts;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Threading;
using IntegrationTests;

namespace IntegrationTest
{    /// <summary>
    /// Testing of tournament controller functionality
    /// like the ability to start/cancell tournament, update database etc
    /// </summary>
    [TestClass]
    public class TournamentControllerTests
    {
        private TestWebCaller WebCaller = new TestWebCaller();

        [TestMethod]
        public async Task GetMapWhichIsNullWhenTournamentCancelled()
        {
            await WebCaller.CancellTournament();
            var map = await WebCaller.GetTournamentState();
            Assert.IsNull(map.Map);
        }
        [TestMethod]
        public async Task GetMapWhichIsNotNullWhenTournamentStarted()
        {
            await WebCaller.StartTournament();
            var map = WebCaller.GetTournamentState().Result;
            Assert.IsNotNull(map.Map);
            await WebCaller.CancellTournament();
        }
        [TestMethod]
        public async Task DatabaseUpdatedAfterGameFinished()
        {
            await WebCaller.CancellTournament();
            var n = 1;
                var expected = WebCaller.GetNumberOfStatistics().Result;
                await WebCaller.StartTournament(n);
                await WebCaller.CancellTournament();
                var result = WhaitUntilDbChanged(expected+n);
                Assert.IsTrue(result.Result);
        }
        [TestMethod]
        public async Task NumberOfTurnsChangedAfterTime()
        {
            await WebCaller.StartTournament(1,100,40);
            var stats = WebCaller.GetTournamentState().Result;
            var start = stats.BlackStatistics.TurnNumber;
            Thread.Sleep(80);
            var end = WebCaller.GetTournamentState().Result.BlackStatistics.TurnNumber;
            await WebCaller.CancellTournament();
            Assert.AreNotEqual(start,end);
        }

        private async Task<bool> WhaitUntilDbChanged(int real)
        {
            using (var client = new HttpClient())
            {
                var stopWatch = new Stopwatch();
                int actual;
                stopWatch.Start();
                do
                {
                    var response = await client.GetAsync(WebCaller.AppPath + "/api/statistic/count");
                    actual = response.Content.ReadAsAsync<int>().Result;
                    Thread.Sleep(100);
                } while (actual != real && stopWatch.ElapsedMilliseconds<5000);

                if (actual == real)
                    return true;
                return false;
            }
        }
    }
}
