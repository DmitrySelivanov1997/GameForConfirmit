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
        
        public Brick(int i, int j): base(i, j)
        {
            Color = Colors.Brown;
        }

      

    }
    
}
