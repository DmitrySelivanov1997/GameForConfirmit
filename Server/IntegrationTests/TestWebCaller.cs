using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommonClient_WebServiseParts;
using Newtonsoft.Json;
using WebGameService.Models;

namespace IntegrationTests
{
    public class TestWebCaller
    {

        public readonly string AppPath = "http://co-yar-ws100:8080/";
        public async Task CancellTournament()
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync(AppPath + "api/tournament");
            }
        }
        public async Task StartTournament(int numberOfGames = 20, int mapSize = 100, int waitTime = 1000)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(AppPath + "api/tournament/start/", new TournamentInitializingClass()
                {
                    MapSize = mapSize,
                    NumberOfGames = numberOfGames,
                    WaitTime = waitTime
                });
            }
        }
        public async Task<TournamentState> GetTournamentState()
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync(AppPath + "api/tournament");
                return JsonConvert.DeserializeObject<TournamentState>(json);
            }
        }

        public async Task<List<GameSessionStatistic>> GetSortedList(string querryOption)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(AppPath + "/api/statistic" + querryOption);
                var actual = JsonConvert.DeserializeObject<List<GameSessionStatistic>>(response);
                return actual;
            }

        }
        public async Task PostAlgorithmToTheServer(byte[] asm, string id)
        {
            using (var client = new HttpClient())
            {
                var byteArrayContent = new ByteArrayContent(asm);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/bson"));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/bson");
                await client.PostAsync(AppPath + "api/algorithm/" + id, byteArrayContent);
            }
        }
        public async Task<string> GetAlgoritmnameFromTheServer(string id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(AppPath + "api/algorithm/" + id);
                return response;
            }
        }

        public async Task<int> GetNumberOfStatistics(string querryOptions = "")
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(AppPath + "/api/statistic/count"+querryOptions);
                var expected = response.Content.ReadAsAsync<int>().Result;
                return expected;
            }
        }
    }
}
