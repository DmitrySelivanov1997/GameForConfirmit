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
        public Dictionary<IAlgoritm, TypesOfObject> Dictionary = new Dictionary<IAlgoritm, TypesOfObject>();
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

            Brick.Probability = 0.2;
            Food.Probability = 0.05;
            ButtonStartFight.IsEnabled = true;
            _mapSize = Convert.ToInt32(MapSize.Text);
            MapGenerator mapGenerator = new MapGenerator(_mapSize);
            MyMap = mapGenerator.GenerateMap();
            WriteableBitmap = BitmapFactory.New((int)MainImage.Width, (int)MainImage.Width);
            MainImage.Source = WriteableBitmap;
            Printer = new WpfPrinter(MainImage);
            Printer.Print(MyMap, WriteableBitmap);
            Engine = new Engine(Dictionary, MyMap) {IsCanceled = false, WaitTime = (int)TurnsTimeSlider.Value};
            Engine.GameOver += Show_Message;
            
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
        private static void Show_Message(GameResult result)
        {
            if (result == GameResult.BlackArmyDestroyed)
                MessageBox.Show("Черная армия разбита");
            if (result == GameResult.WhiteArmyDestroyed)
                MessageBox.Show("Белая армия разбита");
            if (result == GameResult.BlackBaseDestroyed)
                MessageBox.Show("Черная база разбита");
            if (result == GameResult.WhiteBaseDestroyed)
                MessageBox.Show("Белая база разбита");
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
            if (MapSize.Text.Length == 0 || Convert.ToInt32(MapSize.Text) < 4)
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

        private void AlgoritmN1_OnClick(object sender, RoutedEventArgs e)
        {
            Type[] types = null;
            if (LoadDllAndCheckForInterface(ref types)) return;
            foreach (var type in types)
            {
                if (type.GetInterface("IAlgoritm") != null)
                {
                   Dictionary.Add((IAlgoritm)Activator.CreateInstance(type),TypesOfObject.UnitWhite);
                }
            }
            AlgoritmN1.Content = "Алгоритм загружен.";
        }

        private void AlgoritmN2_OnClick(object sender, RoutedEventArgs e)
        {
            Type[] types=null;
            if (LoadDllAndCheckForInterface(ref types)) return;
            foreach (var type in types)
            {
                if (type.GetInterface("IAlgoritm") != null)
                {
                    Dictionary.Add((IAlgoritm)Activator.CreateInstance(type),TypesOfObject.UnitBlack);
                }
            }
            AlgoritmN2.Content = "Алгоритм загружен.";
        }

        private static bool LoadDllAndCheckForInterface(ref Type[] types)
        {
            string filename;
            OpenFileDialog Fd = new OpenFileDialog
            {
                Title = "Выберите библиотеку",
                Filter = "Dll files | *.dll"
            };
            var result = Fd.ShowDialog();
            if (result == true)
            {
                filename = Fd.FileName;
            }
            else return true;
            var asm = Assembly.LoadFrom(filename);
            types = asm.GetTypes();
            if (types.Any(type => type.GetInterface("IAlgoritm") == null))
            {
                MessageBox.Show("Данная библиотека не содержит определения для IAlgoritm");
                return true;
            }
            return false;

        }
    }
}

