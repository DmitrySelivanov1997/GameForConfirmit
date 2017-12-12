using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using InterfaceLibrary;
using Microsoft.Win32;

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
        public DispatcherTimer PrintTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 30) };
        public WpfPrinter Printer;
        public Map MyMap;
        public WriteableBitmap WriteableBitmap;
        public int _mapSize;
        public CancellationTokenSource Ts;
        public int NumberOfTournamentsGame;

        public MainWindow()
        {
            InitializeComponent();
            PrintTimer.Tick += PrintMap;
            Height = SystemParameters.PrimaryScreenHeight - 100;  
            Width = SystemParameters.PrimaryScreenHeight -100;
            WhiteAlgorithmName.Text = WhiteArmyAlgorithm.GetType().FullName;
            BlackArmyAlgorithmName.Text = BlackArmyAlgorithm.GetType().FullName;

        }

        private void ButtonGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            _mapSize = Convert.ToInt32(MapSize.Text);
            MapGenerator mapGenerator = new MapGenerator(_mapSize);
            MyMap = mapGenerator.GenerateMap();
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            MainImage.Source = WriteableBitmap;
            Printer = new WpfPrinter(MainImage);
            Printer.Print(MyMap, WriteableBitmap);
            
        }

        private void PrintMap(object sender, EventArgs e)
        {
            Printer.Print(MyMap, WriteableBitmap);

            NumberOfTurns.Content = Convert.ToString(Engine.TurnNumber);
            WhiteArmyEnemiesKilled.Content = Engine.WhiteArmyStatistics.EnemiesKilled.ToString();
            WhiteArmyFoodEaten.Content = Engine.WhiteArmyStatistics.FoodEaten.ToString();
            WhiteArmyCurrentUnits.Content = Engine.WhiteArmyStatistics.CurrentArmyNumber.ToString();
            BlackArmyEnemiesKilled.Content = Engine.BlackArmyStatistics.EnemiesKilled.ToString();
            BlackArmyFoodEaten.Content = Engine.BlackArmyStatistics.FoodEaten.ToString();
            BlackArmyCurrentUnits.Content = Engine.BlackArmyStatistics.CurrentArmyNumber.ToString();
        }

        

        private void ButtonStartFight(object sender, RoutedEventArgs e)
        {
            WhiteArmyAlgorithm = (IAlgorithm) Activator.CreateInstance(WhiteArmyAlgorithm.GetType());
            BlackArmyAlgorithm = (IAlgorithm)Activator.CreateInstance(BlackArmyAlgorithm.GetType());

            PauseOn.IsEnabled = true;
            PrintTimer.Start();
            Engine = new Engine(WhiteArmyAlgorithm,BlackArmyAlgorithm, MyMap)
            {
                IsCanceled = false,
                WaitTime = (int)TurnsTimeSlider.Value 
            };
            Engine.GameOver += Show_Message;
            Ts = new CancellationTokenSource();
            CancellationToken ct = Ts.Token;
            Task.Factory.StartNew(() =>
            {
                while (!Engine.Ct)
                {
                    Engine.Startbattle();
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate
                   {
                       if (NumberOfTournamentsGame > 1)
                       {
                           ButtonGenerateMap_Click(sender, e);
                           ButtonStartFight(sender, e);
                           NumberOfTournamentsGame--;
                           TextBlockCurrentScore.Text = $"{WhiteArmyWins}:{BlackArmyWins}";
                       }
                       else
                       {
                           TextBlockCurrentScore.Text = $"{WhiteArmyWins}:{BlackArmyWins}";
                           UpdateButtonsStatusAfterTournamentEnded();
                           PrintTimer.Stop();
                           MyMap = null;

                       }
                   });
            }, ct);
        }

        private void UpdateButtonsStatusAfterTournamentEnded()
        {
            ButtonGenerateMap.IsEnabled = true;
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
            PrintTimer.Start();

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
            Printer?.Print(MyMap,WriteableBitmap);
        }

        private void MapSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ButtonStartTournament != null && ButtonGenerateMap != null)
            {
                if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) < 4)
                {
                    ButtonGenerateMap.IsEnabled = false;
                    ButtonStartTournament.IsEnabled = false;
                }
                else if (Convert.ToInt32(TextBoxNumberOfMatches.Text) >= 1)
                    ButtonStartTournament.IsEnabled = true;
                if(MapSize.Text.Length != 0 && Convert.ToInt32(MapSize.Text) >= 4)
                {
                    ButtonGenerateMap.IsEnabled = true;
                }
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

        private void AlgoritmN1_OnClick(object sender, RoutedEventArgs e)
        {
            
            LoadDllAndCheckForInterface(ref WhiteArmyAlgorithm, (Button)sender);
        }

        private void AlgoritmN2_OnClick(object sender, RoutedEventArgs e)
        {
            LoadDllAndCheckForInterface(ref BlackArmyAlgorithm, (Button)sender);
        }

        private  void LoadDllAndCheckForInterface(ref IAlgorithm algorithm, Button sender)
        {
            string filename;
            var fd = new OpenFileDialog
            {
                Title = "Выберите библиотеку",
                Filter = "Dll files | *.dll"
            };
            var result = fd.ShowDialog();
            if (result == true)
            {
                filename = fd.FileName;
            }
            else return;
            var asm = Assembly.LoadFrom(filename);
            var types = asm.GetTypes();
            if (types.Any(type => type.GetInterface("IAlgorithm") != null))
            {
                foreach (var type in types)
                {
                    if (type.GetInterface("IAlgorithm") == null) continue;
                    algorithm = (IAlgorithm)Activator.CreateInstance(type);
                    sender.Content = type.FullName;
                    break;
                }
            }
            WhiteAlgorithmName.Text = WhiteArmyAlgorithm.GetType().FullName;
            BlackArmyAlgorithmName.Text = BlackArmyAlgorithm.GetType().FullName;
        }

        private void ButtonStartTournamemt_Click(object sender, RoutedEventArgs e)
        {

            TextBlockCurrentScore.Text = "0:0";
            BlackArmyWins = 0;
            WhiteArmyWins = 0;
            ButtonGenerateMap.IsEnabled = false;
            TextBoxNumberOfMatches.IsEnabled = false;
            ButtonStartTournament.IsEnabled = false;
            ButtonCancellTournament.IsEnabled = true;
            AlgoritmN1.IsEnabled = false;
            AlgoritmN2.IsEnabled = false;
            NumberOfTournamentsGame = Convert.ToInt32(TextBoxNumberOfMatches.Text);
            if(MyMap == null)
                ButtonGenerateMap_Click(sender,e);
            ButtonStartFight(sender,e);
        }

        private void ButtonCancellTournament_OnClick(object sender, RoutedEventArgs e)
        {
            NumberOfTournamentsGame = 0;
            Engine.Ct = true;
            UpdateButtonsStatusAfterTournamentEnded();
        }
    }
}

