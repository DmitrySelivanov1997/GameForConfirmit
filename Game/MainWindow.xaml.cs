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
        private const int _mapSize = 15;

        public MainWindow()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenHeight - 100;

            Height = SystemParameters.PrimaryScreenHeight - 100;

            MainImage.Width = Width - 100;

            MainImage.Height = Height - 100;

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

        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainImage.Width = Width - 100;

            MainImage.Height = Height - 100;
        }

        private void ButtonStartFight_Click(object sender, RoutedEventArgs e)
        {
            PrintTimer.Stop();
            PrintTimer.Start();
            var task = Task.Factory.StartNew(StartEngine);

        }

        private void StartEngine()
        {

            for (int i = 0;; i++)
            {
                Engine.Startbattle();
            }
        }
        private static void Show_Message(string message)
        {
            MessageBox.Show(message);
        }
    }
}

