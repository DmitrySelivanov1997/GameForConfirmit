using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using CommonClient_WebServiseParts;
using Newtonsoft.Json;

namespace Game.Models
{
    public class WebServiceCaller
    {
        private readonly string _appPath = Properties.Settings.Default.AppPath;

        public async Task<TournamentState> GetTournamentStateFromTheServer(DispatcherTimer timer)
        {
            using (var client = new HttpClient())
            {
                timer.Stop();
                string json = await client.GetStringAsync(_appPath + "api/tournament");
                timer.Start();
                return JsonConvert.DeserializeObject<TournamentState>(json);
            }
        }
        public  async Task PostAlgorithmToTheServer(byte[] asm, string id)
        {
            using (var client = new HttpClient())
            {
                var byteArrayContent = new ByteArrayContent(asm);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/bson"));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/bson");
                await client.PostAsync(_appPath + "api/algorithm/" + id, byteArrayContent);
            }
        }
        public async Task GetAlgoritmnameFromTheServer(TextBlock algorithmName, string id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(_appPath + "api/algorithm/" + id);
                algorithmName.Text = response;
            }
        }

        public async Task CancellTournament()
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync(_appPath + "api/tournament");
            }
        }

        public async Task PostInitializingSettingsToTheServer(TournamentInitializingClass value)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(_appPath + "api/tournament/start/", value);
            }
        }

        public async Task PutNewWaitTimeValueToTheServer(int value)
        {
            using (var client = new HttpClient())
            {
                await client.PutAsJsonAsync(_appPath + "api/tournament/", value);
            }
        }
    }
}
