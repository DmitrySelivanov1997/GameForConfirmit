using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class UnitBase:BaseItem
    {
        public UnitBase(int i, int j, Color color) : base(i, j)
        {
            Color = color;
        }
       
    }
}
