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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public int WhiteArmyWins { get; set; }
        public int BlackArmyWins { get; set; }
        public IAlgorithm WhiteArmyAlgorithm = new Algoritm2();
        public IAlgorithm BlackArmyAlgorithm = new Algoritm2();
        public Engine Engine ;
        public DispatcherTimer PrintTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 80) };
        public WpfPrinter Printer;
        public TypesOfObject[,] MyMap;
        public WriteableBitmap WriteableBitmap;
        public int _mapSize;
        public CancellationTokenSource Ts;
        public int NumberOfTournamentsGame;
        public const string AppPath = "http://localhost:62109/";

        public MainWindow()
        {
            InitializeComponent();
            PrintTimer.Tick += PrintMap;
            Height = SystemParameters.PrimaryScreenHeight - 100;  
            Width = SystemParameters.PrimaryScreenHeight -100;
            WhiteAlgorithmName.Text = WhiteArmyAlgorithm.GetType().FullName;
            BlackAlgorithmName.Text = BlackArmyAlgorithm.GetType().FullName;
            Printer = new WpfPrinter(MainImage);
            PrintTimer.Start();
        }
        
        private async void PrintMap(object sender, EventArgs e)
        {
            await GetMapFromTheWebServiceAndSetIt();
            Printer.Print(MyMap, WriteableBitmap);

            //NumberOfTurns.Content = Convert.ToString(Engine.TurnNumber);
            //WhiteArmyEnemiesKilled.Content = Engine.WhiteArmyStatistics.EnemiesKilled.ToString();
            //WhiteArmyFoodEaten.Content = Engine.WhiteArmyStatistics.FoodEaten.ToString();
            //WhiteArmyCurrentUnits.Content = Engine.WhiteArmyStatistics.CurrentArmyNumber.ToString();
            //BlackArmyEnemiesKilled.Content = Engine.BlackArmyStatistics.EnemiesKilled.ToString();
            //BlackArmyFoodEaten.Content = Engine.BlackArmyStatistics.FoodEaten.ToString();
            //BlackArmyCurrentUnits.Content = Engine.BlackArmyStatistics.CurrentArmyNumber.ToString();
        }

        private async Task GetMapFromTheWebServiceAndSetIt()
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync(AppPath + "api/tournament");
                MyMap = JsonConvert.DeserializeObject<TypesOfObject[,]>(json);
            }
        }

        private void UpdateButtonsStatusAfterTournamentEnded()
        {
            MapSize.IsEnabled = true;
            ButtonStartTournament.IsEnabled = true;
            PauseOn.IsEnabled = false;
            TextBoxNumberOfMatches.IsEnabled = true;
            AlgoritmN1.IsEnabled = true;
            AlgoritmN2.IsEnabled = true;
        }


        private void Show_Message(GameResult result)
        {
            if (result == GameResult.BlackArmyDestroyed || result == GameResult.BlackBaseDestroyed)
                {
                    WhiteArmyWins++;
                }
            if (result == GameResult.WhiteArmyDestroyed || result == GameResult.WhiteBaseDestroyed)
                {
                    BlackArmyWins++;
                }
        }

        #region Changeable UI parts
            


            private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            PrintTimer.Stop();

            Engine.IsCanceled = true;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {

            Engine.IsCanceled = false;
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MyGrid.Width = Width;
            MyGrid.Height = Height;
            MainImage.Height = (Width<Height? Width:Height) * 4 / 6;
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
            PrintTimer.Interval=new TimeSpan(0,0,0,time);

        }

        private void TurnsTimeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(Engine!=null)
            Engine.WaitTime = (int)TurnsTimeSlider.Value;

        }
        #endregion

        private async void AlgoritmN1_OnClick(object sender, RoutedEventArgs e)
        {

            await GetAlgoritmnameFromTheServer(WhiteAlgorithmName, "white");
            //LoadDllAndCheckForInterface(ref WhiteArmyAlgorithm, (Button)sender);
        }

        private async Task GetAlgoritmnameFromTheServer(TextBlock algorithmName,string id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(AppPath+"api/algorithm/" + id);
                algorithmName.Text = response;
            }
        }

        private async void AlgoritmN2_OnClick(object sender, RoutedEventArgs e)
        {
            var asm = LoadDll();
            if(asm.Length==0)
                return;
            await PostAlgorithmToTheServer(asm, "black");
            await GetAlgoritmnameFromTheServer(BlackAlgorithmName, "black");
        }

        private async Task PostAlgorithmToTheServer(byte[] asm, string id)
        {
            using (var client = new HttpClient())
            {
                var byteArrayContent = new ByteArrayContent(asm);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/bson"));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/bson");
                var result = await client.PostAsync(AppPath+"api/algorithm/"+id, byteArrayContent);
            }
        }

        private byte[] LoadDll()
        {
            string filename = null;
            var fd = new OpenFileDialog
            {
                Title = "Выберите библиотеку",
                Filter = "Dll files | *.dll"
            };
            var result = fd.ShowDialog();
            if (result == true)
            {
                filename = fd.FileName;
                return File.ReadAllBytes(filename);
            }
            return new byte[0];
        }

        private async void ButtonStartTournamemt_Click(object sender, RoutedEventArgs e)
        {
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
                var response = await client.PostAsJsonAsync<ClassForTournamentControllerPost>(AppPath + "api/tournament", value);
                if (!response.IsSuccessStatusCode)
                {
                   MessageBox.Show(" Tournamen information has not been sent. \r\nERROR:" + response.ReasonPhrase);
                }

            }
            ButtonCancellTournament.IsEnabled = true;
        }

        private async void ButtonCancellTournament_OnClick(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(AppPath + "api/tournament");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(" Tournamen has not been stopped. \r\nERROR:" + response.ReasonPhrase);
                }

            }
        }
    }
}

