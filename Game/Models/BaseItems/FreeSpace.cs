using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Game.Models.BaseItems
{
    class FreeSpace:BaseItem
    {
        public FreeSpace(int x, int y):base(x, y)
        {
            Color = Colors.Bisque;
        }
    }
}
