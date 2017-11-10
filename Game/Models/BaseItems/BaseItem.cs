using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class BaseItem
    {
        public Color Color { get; set; }
        public int X { get; }
        public int Y { get; }

        public BaseItem(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
