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
            MyGrid.Width = Width;
            MyGrid.Height = Height;
            MainImage.Height = (Width<Height? Width:Height) * 4 / 5;
            MainImage.Width = MainImage.Height; 
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Brick.Probability = 0.15;
            Food.Probability = 0.0;

            _mapSize = Convert.ToInt32(MapSize.Text);
            MapGenerator mapGenerator = new MapGenerator();
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            MainImage.Source = WriteableBitmap;
            Printer = new WpfPrinter(MainImage);
            ButtonStartFight.IsEnabled = true;
            MyMap = mapGenerator.GenerateMap(_mapSize, Printer);
            Printer.Print(MyMap, WriteableBitmap);
            Engine = new Engine(new Algoritm1(), new Algoritm2(), MyMap) {IsCanceled = false};
            Engine.GameOver += Show_Message;


        }

        private void PrintMap(object sender, EventArgs e)
        {
            Printer?.Print(MyMap, WriteableBitmap);

            NumberOfTurns.Content = Convert.ToString(Engine.TurnNumber);
        }

        

        private void ButtonStartFight_Click(object sender, RoutedEventArgs e)
        {
            PrintTimer.Stop();
            PrintTimer.Start();
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
            if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) == 0)
            {
                ButtonStart.IsEnabled = false;
                ButtonStartFight.IsEnabled = false;
            }
            else
            {
                ButtonStart.IsEnabled = true;
            }
        }

        private void MapSize_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            

        }
    }
}

