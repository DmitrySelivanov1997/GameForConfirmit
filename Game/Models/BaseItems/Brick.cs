using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class Brick:BaseItem
    {
        public static double Probability { get; set; }
        
        public Brick(int x, int y): base(x, y)
        {
            Color = Colors.Brown;
        }

      

    }
    
}
