using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using InterfaceLibrary;

namespace Game.Models.BaseItems
{
    public class Brick:BaseItem
    {
        public static double Probability { get; set; } = Properties.Settings.Default.BrickProbability;
        
        public Brick(int i, int j, TypesOfObject obj): base(i, j, obj)
        {
            Color = Colors.Brown;
        }

      

    }
    
}
