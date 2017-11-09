using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class Base: BaseItem
    {
        public Base(int x, int y, Color color) : base(x, y)
        {
            Color = color;
        }
    }
}
