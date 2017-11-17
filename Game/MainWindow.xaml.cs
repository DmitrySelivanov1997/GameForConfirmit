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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Engine Engine ;
        public DispatcherTimer PrintTimer;
        public WpfPrinter Printer;
        public Map MyMap;
        public WriteableBitmap WriteableBitmap;
        public int _mapSize ;

        public MainWindow()
        {
            InitializeComponent();
            Height = SystemParameters.PrimaryScreenHeight - 100;  
            Width = SystemParameters.PrimaryScreenHeight -100;
            MyGrid.Width = Width;
            MyGrid.Height = Height;
            MainImage.Height = Width *3/4 ;
            MainImage.Width = Width * 3 / 4; 
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Brick.Probability = 0.15;
            Food.Probability = 0.5;
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            Printer = new WpfPrinter(MainImage);
            MainImage.Source = WriteableBitmap;
            MapGenerator mapGenerator = new MapGenerator();
            MyMap = mapGenerator.GenerateMap(_mapSize, Printer);
            Printer.Print(MyMap, WriteableBitmap);
            PrintTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 30)};
            PrintTimer.Tick += PrintMap;
            Engine = new Engine(new Algoritm1(), new Algoritm2(), MyMap);
            Engine.GameOver += Show_Message;

        }

        private void PrintMap(object sender, EventArgs e)
        {
            Printer.Print(MyMap, WriteableBitmap);
        }

        

        private void ButtonStartFight_Click(object sender, RoutedEventArgs e)
        {
            _mapSize = Convert.ToInt32(MapSize.Text);
            PrintTimer.Stop();
            PrintTimer.Start();
            var task = Task.Factory.StartNew(StartEngine);
            

        }

        private void StartEngine()
        {

            Engine.Startbattle();
            
        }
        private static void Show_Message(string message)
        {
            MessageBox.Show(message);
            Environment.Exit(0);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MyGrid.Width = Width;
            MyGrid.Height = Height;
            MainImage.Height = Width * 3 / 4;
            MainImage.Width = Width * 3 / 4;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) == 0)
            {
                ButtonStart.IsEnabled = false;
                ButtonStartFight.IsEnabled = false;
            }
            else
            {
                ButtonStart.IsEnabled = true;
                ButtonStartFight.IsEnabled = true;
            }
        }

        private void MapSize_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}

