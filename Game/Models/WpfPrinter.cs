using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Game.Interfaces;

namespace Game.Models
{
    class WpfPrinter:IPrinter
    {
        
        public Grid Grid { get; }
        public WpfPrinter(Grid grid )
        {
            Grid = grid;
        }


        public void Print(Map map)
        {
            for (var i = 0; i < map.Size; i++)
            {
                for (var j = 0; j < map.Size; j++)
                {
                    if (map.GetItem(i, j) != null)
                        AddButtonAndColorIt(i, j, map.GetItem(i, j).Color);
                }
            }
        }
        private void AddButtonAndColorIt(int i, int j, Color color)
        {
            var button = new Button { Background = new SolidColorBrush(color) };
            Grid.SetRow(button, i);
            Grid.SetColumn(button, j);
            Grid.Children.Add(button);
        }
    }
}
