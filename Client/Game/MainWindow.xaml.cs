using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Game.Models;
using InterfaceLibrary;
using Microsoft.Win32;
using CommonClient_WebServiseParts;
namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public DispatcherTimer PrintTimer = new DispatcherTimer { Interval = new TimeSpan(0) };
        public WpfPrinter Printer;
        public TypesOfObject[,] MyMap;
        public WriteableBitmap WriteableBitmap;
        public int MapSize;
        public int NumberOfTournamentsGame;
        public WebServiceCaller WebService = new WebServiceCaller();

        public MainWindow()
        {
            InitializeComponent();
            PrintTimer.Tick += PrintMap;
            Height = SystemParameters.PrimaryScreenHeight - 100;
            Width = SystemParameters.PrimaryScreenHeight - 100;
            Printer = new WpfPrinter(MainImage);
            PrintTimer.Start();
        }

        private async void PrintMap(object sender, EventArgs e)
        {
            var state = await WebService.GetTournamentStateFromTheServer(PrintTimer);
            MyMap = state.Map;
            Printer.Print(MyMap, WriteableBitmap);
            SetStatisticInformation(state.WhiteStatistics, state.BlackStatistics);
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
            await WebService.PostAlgorithmToTheServer(asm, "white");
            await WebService.GetAlgoritmnameFromTheServer(WhiteAlgorithmName, "white");
        }
        private async void AlgoritmN2_OnClick(object sender, RoutedEventArgs e)
        {
            var asm = LoadDll();
            if (asm.Length == 0)
                return;
            await WebService.PostAlgorithmToTheServer(asm, "black");
            await WebService.GetAlgoritmnameFromTheServer(BlackAlgorithmName, "black");
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

        private async void ButtonStartTournamemt_Click(object sender, RoutedEventArgs e)
        {
            await WebService.GetAlgoritmnameFromTheServer(WhiteAlgorithmName, "white");
            await WebService.GetAlgoritmnameFromTheServer(BlackAlgorithmName, "black");
            UpdateButtonStatusAftertournamentStarts();
            var value = new TournamentInitializingClass
            {
                MapSize = Convert.ToInt32(MapSizeTextBox.Text),
                NumberOfGames = Convert.ToInt32(TextBoxNumberOfMatches.Text),
                WaitTime = (int)TurnsTimeSlider.Value
            };
            await WebService.PostInitializingSettingsToTheServer(value);
        }

        private async void ButtonCancellTournament_OnClick(object sender, RoutedEventArgs e)
        {
            await WebService.CancellTournament();
            UpdateButtonsStatusAfterTournamentEnded();
        }
        private void UpdateButtonsStatusAfterTournamentEnded()
        {
            MapSizeTextBox.IsEnabled = true;
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
            MapSizeTextBox.IsEnabled = false;
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
                if (MapSizeTextBox.Text.Length == 0 || Convert.ToInt32(MapSizeTextBox.Text) < 4)
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
                if (TextBoxNumberOfMatches.Text.Length == 0 || MapSizeTextBox.Text.Length == 0 || Convert.ToInt32(TextBoxNumberOfMatches.Text) < 1)
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
            await WebService.PutNewWaitTimeValueToTheServer((int)TurnsTimeSlider.Value);
        }
        #endregion
    }
}



