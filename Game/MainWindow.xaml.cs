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
        public CancellationTokenSource ts;
        public int NumberOfTournamentsGame;

        public MainWindow()
        {
            InitializeComponent();
            PrintTimer.Tick += PrintMap;
            Height = SystemParameters.PrimaryScreenHeight - 100;  
            Width = SystemParameters.PrimaryScreenHeight -100;
            
        }

        private void ButtonGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            Brick.Probability = 0.2;
            Food.Probability = 0.01;
            ButtonStartFight.IsEnabled = true;
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

        

        private void ButtonStartFight_Click(object sender, RoutedEventArgs e)
        {
            WhiteArmyAlgorithm = (IAlgorithm) Activator.CreateInstance(WhiteArmyAlgorithm.GetType());
            BlackArmyAlgorithm = (IAlgorithm)Activator.CreateInstance(BlackArmyAlgorithm.GetType());

            WhiteAlgorithmName.Text = WhiteArmyAlgorithm.GetType().FullName;
            BlackArmyAlgorithmName.Text = BlackArmyAlgorithm.GetType().FullName;

            PauseOn.IsEnabled = true;
            PrintTimer.Start();
            ButtonStartFight.IsEnabled = false;
            Engine = new Engine(WhiteArmyAlgorithm,BlackArmyAlgorithm, MyMap)
            {
                IsCanceled = false,
                WaitTime = (int)TurnsTimeSlider.Value 
            };
            Engine.GameOver += Show_Message;
            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            Task.Factory.StartNew(() =>
            {
                while (!ct.IsCancellationRequested)
                {
                    Engine.Startbattle(ct);
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart) delegate
                    {
                        if(NumberOfTournamentsGame!=0)
                        {
                            ButtonGenerateMap_Click(sender, e);
                            ButtonStartFight_Click(sender, e);
                            NumberOfTournamentsGame = Convert.ToInt32(TextBoxNumberOfMatches.Text);
                            NumberOfTournamentsGame--;
                            TextBoxNumberOfMatches.Text = NumberOfTournamentsGame.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"Счет команды белых {WhiteArmyAlgorithm.GetType().FullName}: {WhiteArmyWins} \r\nСчет команды черных {BlackArmyAlgorithm.GetType().FullName}: {BlackArmyWins}");
                            ButtonGenerateMap.IsEnabled = true;
                            ButtonStartFight.IsEnabled = true;
                            MapSize.IsEnabled = true;
                            ButtonStartTournament.IsEnabled = true;
                        }
                    });
            }, ct);

        }
        private  void Show_Message(GameResult result)
        {
            string whiteArmyLeader = WhiteArmyAlgorithm.GetType().FullName,blackArmyLeader = BlackArmyAlgorithm.GetType().FullName;
            if (result == GameResult.BlackArmyDestroyed)
            {
                MessageBox.Show($"Черная армия, под управлением {blackArmyLeader} разбита");
                WhiteArmyWins++;
            }
            if (result == GameResult.WhiteArmyDestroyed)
            {
                MessageBox.Show($"Белая армия, под управлением {whiteArmyLeader} разбита");
                BlackArmyWins++;
            }

            if (result == GameResult.BlackBaseDestroyed)
            {
                MessageBox.Show($"Черная база, игрока {blackArmyLeader} разбита");
                WhiteArmyWins++;
            }
            if (result == GameResult.WhiteBaseDestroyed)
            {
                MessageBox.Show($"Белая база, игрока {whiteArmyLeader} разбита");
                BlackArmyWins++;
            }
            ts.Cancel();


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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) < 4)
            {
                ButtonGenerateMap.IsEnabled = false;
                ButtonStartFight.IsEnabled = false;
                ButtonStartTournament.IsEnabled = false;
            }
            else
            {
                ButtonGenerateMap.IsEnabled = true;
            }
        }

        private void TextBoxNumberOfMatches_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MapSize.Text.Length == 0 || Convert.ToInt32(TextBoxNumberOfMatches.Text) < 2)
            {
                ButtonStartTournament.IsEnabled = false;
            }
            else
            {
                ButtonStartTournament.IsEnabled = true;
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

        private void LoadDllAndCheckForInterface(ref IAlgorithm algorithm, Button sender)
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
                    return;
                }
            }
            MessageBox.Show("Данная библиотека не содержит определения для IAlgoritm");


        }

        private void ButtonStartTournamemt_Click(object sender, RoutedEventArgs e)
        {
            BlackArmyWins = 0;
            WhiteArmyWins = 0;
            ts?.Cancel();
            ButtonGenerateMap.IsEnabled = false;
            ButtonStartFight.IsEnabled = false;
            NumberOfTournamentsGame = Convert.ToInt32(TextBoxNumberOfMatches.Text);
            ButtonGenerateMap_Click(sender,e);
            ButtonStartFight_Click(sender,e);
        }
        
    }
}

