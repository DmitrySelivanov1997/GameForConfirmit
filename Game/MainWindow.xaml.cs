using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const int _mapSize = 100;

        public MainWindow()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenHeight - 100;

            Height = SystemParameters.PrimaryScreenHeight - 100;

            MyGrid.Width = Width - 100;

            MyGrid.Height = Height - 100;
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Brick.Probability = 0.15;
            Food.Probability = 0.02;
            GetGridCleared();
            AddRowsAndColomns();
            WpfPrinter printer = new WpfPrinter(MyGrid);
            MapGenerator mapGenerator = new MapGenerator();
            var myMap = mapGenerator.GenerateMap(_mapSize);
            printer.Print(myMap);
            Console.WriteLine();

        }

        private void GetGridCleared()
        {
            MyGrid.Children.Clear();
            MyGrid.RowDefinitions.Clear();
            MyGrid.ColumnDefinitions.Clear();
        }

        private void AddRowsAndColomns()
        {
            for (int i = 0; i < _mapSize; i++)
            {
                MyGrid.ColumnDefinitions.Add(new ColumnDefinition());
                MyGrid.RowDefinitions.Add(new RowDefinition());
            }
        }


        private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MyGrid.Width = Width - 100;

            MyGrid.Height = Height - 100;
        }
    }
}

