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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Engine Engine ;
        public DispatcherTimer PrintTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 30) };
        public WpfPrinter Printer;
        public Map MyMap;
        public WriteableBitmap WriteableBitmap;
        public int _mapSize ;

        public MainWindow()
        {
            InitializeComponent();
            PrintTimer.Tick += PrintMap;
            Height = SystemParameters.PrimaryScreenHeight - 100;  
            Width = SystemParameters.PrimaryScreenHeight -100;
        }

        private void ButtonGenerateMap_Click(object sender, RoutedEventArgs e)
        {
            ButtonStartFight.IsEnabled = true;
            _mapSize = Convert.ToInt32(MapSize.Text);
            MapGenerator mapGenerator = new MapGenerator(_mapSize);
            MyMap = mapGenerator.GenerateMap();
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            MainImage.Source = WriteableBitmap;
            Printer = new WpfPrinter(MainImage);
            Printer.Print(MyMap, WriteableBitmap);
            Engine = new Engine(new Algoritm1(), new Algoritm2(), MyMap) {IsCanceled = false, WaitTime = (int)TurnsTimeSlider.Value};
            Engine.MapManager.GameOver += Show_Message;
            
        }

        private void PrintMap(object sender, EventArgs e)
        {
            Printer.Print(MyMap, WriteableBitmap);

            NumberOfTurns.Content = Convert.ToString(Engine.TurnNumber);
        }

        

        private void ButtonStartFight_Click(object sender, RoutedEventArgs e)
        {
            PauseOn.IsEnabled = true;
            PrintTimer.Start();
            ButtonStartFight.IsEnabled = false;
            var task = Task.Factory.StartNew(StartEngine);
            
        }

        private void StartEngine()
        {
            for (;;)
            {
                Engine.Startbattle();
            }
        }
        private static void Show_Message(string message)
        {
            MessageBox.Show(message);
            Environment.Exit(0);
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
            MainImage.Height = (Width<Height? Width:Height) * 4 / 5;
            MainImage.Width = MainImage.Height;
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            MainImage.Source = WriteableBitmap;
            Printer?.Print(MyMap,WriteableBitmap);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) < 10)
            {
                ButtonGenerateMap.IsEnabled = false;
                ButtonStartFight.IsEnabled = false;
            }
            else
            {
                ButtonGenerateMap.IsEnabled = true;
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
    }
}

