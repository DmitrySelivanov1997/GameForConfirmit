using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Game.Interfaces;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int y, int x)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !Equals(left, right);
        }
    }
    class WpfPrinter:IPrinter
    {
        private ConcurrentDictionary<Point, Button> ButtonsDictionary { get; set; }
        public Image MainImage { get; }
        public WpfPrinter(Image mainImage )
        {
            ButtonsDictionary = new ConcurrentDictionary<Point, Button>();
            MainImage = mainImage;
        }


        public void Print(Map map, WriteableBitmap writeableBitmap)
        {
            

            using (writeableBitmap.GetBitmapContext())
            {
                var timer = Stopwatch.StartNew();
                for (var i = 0; i < map.GetLength(); i++)
                {
                   
                    for (var j = 0; j < map.GetLength(); j++)
                    {
                        if (map.GetItem(i, j) != null)
                            AddObjectOnBmp(i, j, map.GetItem(i, j).Color, writeableBitmap, map);
                    }
                    
                }
                Debug.WriteLine(timer.ElapsedMilliseconds);
            }
          

        }

        private void AddObjectOnBmp(int j, int i, Color color, WriteableBitmap writeableBitmap, Map map)
        {
            writeableBitmap.FillRectangle(
                (int) (i * MainImage.Height / map.GetLength()),
                (int)(j * MainImage.Width / map.GetLength()),
                (int)((i +1)*( MainImage.Width / map.GetLength())), 
                (int)((j + 1) * ( MainImage.Height / map.GetLength())),
                color
                );
           
        }

        public void UpdateItem( BaseItem item)
        {
            //Grid.Dispatcher.Invoke(
            //    ()=>AddButtonAndColorIt(item.Y, item.X, item.Color));
        }

        private void AddButtonAndColorIt(int i, int j, Color color)
        {
            //var point = new Point(i, j);
            //if (ButtonsDictionary.ContainsKey(point))
            //    Grid.Children.Remove(ButtonsDictionary[point]);
            //var button = new Button { Background = new SolidColorBrush(color) };
            //Grid.SetRow(button, i);
            //Grid.SetColumn(button, j);
            //ButtonsDictionary[point] = button; 
            //Grid.Children.Add(button);
        }
    }
}
