using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Game.Models;
using Game.Models.BaseItems;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using InterfaceLibrary;
using Microsoft.Win32;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using WebGameService.Models;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public int WhiteArmyWins { get; set; }
        public int BlackArmyWins { get; set; }
        public Engine Engine ;
        public DispatcherTimer PrintTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 75) };
        public WpfPrinter Printer;
        public TypesOfObject[,] MyMap;
        public WriteableBitmap WriteableBitmap;
        public int _mapSize;
        public CancellationTokenSource Ts;
        public int NumberOfTournamentsGame;
        public const string AppPath = "http://localhost:62109/";

        public  MainWindow()
        {
            InitializeComponent();
            PrintTimer.Tick += PrintMap;
            Height = SystemParameters.PrimaryScreenHeight - 100;  
            Width = SystemParameters.PrimaryScreenHeight -100;
            Printer = new WpfPrinter(MainImage);
            PrintTimer.Start();
        }
        
        private async void PrintMap(object sender, EventArgs e)
        {
            var state = await GetTournamentState();
            MyMap = state.Map;
            Printer.Print(MyMap, WriteableBitmap);
            SetStatisticInformation(state.WhiteStatistics, state.BlackStatistics);
        }
        async Task<TournamentState> GetTournamentState()
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync(AppPath + "api/tournament/");
                return JsonConvert.DeserializeObject<TournamentState>(json);
            }
        }
        private void SetStatisticInformation(Statistics statisticsWhite, Statistics statisticsBlack)
        {
            NumberOfTurns.Content = Convert.ToString(statisticsWhite.TurnNumber);
            WhiteArmyEnemiesKilled.Content = statisticsWhite.EnemiesKilled.ToString();
            WhiteArmyFoodEaten.Content = statisticsWhite.FoodEaten.ToString();
            WhiteArmyCurrentUnits.Content = statisticsWhite.CurrentArmyNumber.ToString();
            BlackArmyEnemiesKilled.Content = statisticsBlack.EnemiesKilled.ToString();
            BlackArmyFoodEaten.Content = statisticsBlack.FoodEaten.ToString();
            BlackArmyCurrentUnits.Content = statisticsBlack.CurrentArmyNumber.ToString();
            TextBlockCurrentScore.Text = $"{statisticsWhite.NumberOfWins}:{statisticsBlack.NumberOfWins}";
        }
        private async void AlgoritmN1_OnClick(object sender, RoutedEventArgs e)
        {
            var asm = LoadDll();
            if (asm.Length == 0)
                return;
            await PostAlgorithmToTheServer(asm, "white");
            await GetAlgoritmnameFromTheServer(WhiteAlgorithmName, "white");
        }
        private async void AlgoritmN2_OnClick(object sender, RoutedEventArgs e)
        {
            var asm = LoadDll();
            if(asm.Length==0)
                return;
            await PostAlgorithmToTheServer(asm, "black");
            await GetAlgoritmnameFromTheServer(BlackAlgorithmName, "black");
        }
        private static byte[] LoadDll()
        {
            var fd = new OpenFileDialog
            {
                Title = "Выберите библиотеку",
                Filter = "Dll files | *.dll"
            };
            var result = fd.ShowDialog();
            if (result == true)
            {
                var filename = fd.FileName;
                return File.ReadAllBytes(filename);
            }
            return new byte[0];
        }
        private static async Task PostAlgorithmToTheServer(byte[] asm, string id)
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
        private async Task GetAlgoritmnameFromTheServer(TextBlock algorithmName, string id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(AppPath + "api/algorithm/" + id);
                algorithmName.Text = response;
            }
        }

        private async void ButtonStartTournamemt_Click(object sender, RoutedEventArgs e)
        {
            await GetAlgoritmnameFromTheServer(WhiteAlgorithmName, "white");
            await GetAlgoritmnameFromTheServer(BlackAlgorithmName, "black");
            _mapSize = Convert.ToInt32(MapSize.Text);
            NumberOfTournamentsGame = Convert.ToInt32(TextBoxNumberOfMatches.Text);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppPath);
                var value = new ClassForTournamentControllerPost
                {
                    MapSize = _mapSize,
                    NumberOfGames = NumberOfTournamentsGame,
                    WaitTime = (int)TurnsTimeSlider.Value
                };
                await client.PostAsJsonAsync(AppPath + "api/tournament/start", value);
            }
            UpdateButtonStatusAftertournamentStarts();
        }

        private async void ButtonCancellTournament_OnClick(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
               await client.DeleteAsync(AppPath + "api/tournament");
            }
            UpdateButtonsStatusAfterTournamentEnded();
        }
        private void UpdateButtonsStatusAfterTournamentEnded()
        {
            MapSize.IsEnabled = true;
            ButtonCancellTournament.IsEnabled = false;
            ButtonStartTournament.IsEnabled = true;
            TextBoxNumberOfMatches.IsEnabled = true;
            AlgoritmN1.IsEnabled = true;
            AlgoritmN2.IsEnabled = true;
        }
        private void UpdateButtonStatusAftertournamentStarts()
        {
            ButtonCancellTournament.IsEnabled = true;
            ButtonStartTournament.IsEnabled = false;
            MapSize.IsEnabled = false;
            TextBoxNumberOfMatches.IsEnabled = false;
            AlgoritmN1.IsEnabled = false;
            AlgoritmN2.IsEnabled = false;
        }

        #region Changeable UI parts
        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MyGrid.Width = Width;
            MyGrid.Height = Height;
            MainImage.Height = (Width < Height ? Width : Height) * 4 / 6;
            MainImage.Width = MainImage.Height;
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            MainImage.Source = WriteableBitmap;
        }

        private void MapSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonStartTournament != null)
            {
                if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) < 4)
                {
                    ButtonStartTournament.IsEnabled = false;
                }
                else if (Convert.ToInt32(TextBoxNumberOfMatches.Text) >= 1)
                    ButtonStartTournament.IsEnabled = true;
            }
        }

        private void TextBoxNumberOfMatches_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonStartTournament != null)
            {
                if (TextBoxNumberOfMatches.Text.Length == 0 || MapSize.Text.Length == 0 || Convert.ToInt32(TextBoxNumberOfMatches.Text) < 1)
                {
                    ButtonStartTournament.IsEnabled = false;
                }
                else
                {
                    ButtonStartTournament.IsEnabled = true;
                }
            }
        }

        private void MapSize_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void DrawTimeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var time = (int)DrawTimeSlider.Value;
            PrintTimer.Interval = new TimeSpan(0, 0, 0, time);

        }

        private async void TurnsTimeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppPath);
                await client.PostAsJsonAsync(AppPath + "api/tournament/delay", (int)TurnsTimeSlider.Value);
            }

        }
        #endregion
    }
}

