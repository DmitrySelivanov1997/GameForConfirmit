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
        public string AppPath = Properties.Settings.Default.AppPath;

        public async Task<TournamentState> GetTournamentStateFromTheServer(DispatcherTimer timer)
        {
            using (var client = new HttpClient())
            {
                timer.Stop();
                string json = await client.GetStringAsync(AppPath + "api/tournament");
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
                await client.PostAsync(AppPath + "api/algorithm/" + id, byteArrayContent);
            }
        }
        public async Task GetAlgoritmnameFromTheServer(TextBlock algorithmName, string id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(AppPath + "api/algorithm/" + id);
                algorithmName.Text = response;
            }
        }

        public async Task CancellTournament()
        {
            using (var client = new HttpClient())
            {
                await client.DeleteAsync(AppPath + "api/tournament");
            }
        }

        public async Task PostInitializingSettingsToTheServer(TournamentInitializingClass value)
        {
            using (var client = new HttpClient())
            {
                await client.PostAsJsonAsync(AppPath + "api/tournament/start/", value);
            }
        }

        public async Task PutNewWaitTimeValueToTheServer(int value)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(int.MaxValue);
                await client.PutAsJsonAsync(AppPath + "api/tournament/", value);
            }
        }
    }
}
