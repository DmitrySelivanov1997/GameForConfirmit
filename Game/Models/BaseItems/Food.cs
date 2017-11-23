using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class Food:BaseItem
    {
        public static double Probability { get; set; }
        public Food(int i, int j): base(i, j)
        {
            Color = Colors.Green;
        }
        
    }
}
