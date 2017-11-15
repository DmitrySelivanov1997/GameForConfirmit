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
using Game.Models;
using Game.Models.BaseItems;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Map MyMap;
        private const int _mapSize = 100;

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
            Food.Probability = 0.05;
            //GetGridCleared();
            //AddRowsAndColomns();
            var writeableBmp = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            WpfPrinter printer = new WpfPrinter(MainImage);
            MainImage.Source = writeableBmp;
            MapGenerator mapGenerator = new MapGenerator();
            MyMap = mapGenerator.GenerateMap(_mapSize, printer);
            printer.Print(MyMap, writeableBmp);
        }

        private void GetGridCleared()
        {
            
            
            
            
        }

        


        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainImage.Width = Width - 100;

            MainImage.Height = Height - 100;
        }

        private void ButtonStartFight_Click(object sender, RoutedEventArgs e)
        {
            var task = Task.Factory.StartNew(StartEngine, TaskCreationOptions.LongRunning);

        }

        private void StartEngine()
        {
            Engine eng = new Engine(new Algoritm1(), new Algoritm2(), MyMap);
            for (int i = 0;; i++)
            {
                eng.Startbattle();
                //Thread.Sleep(1);
                //await Task.Delay(1);
                //Debug.WriteLine(i);
            }
        }
    }
}

