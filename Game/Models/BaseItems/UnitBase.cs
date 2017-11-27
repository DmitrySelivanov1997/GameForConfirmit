using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using InterfaceLibrary;

namespace Game.Models.BaseItems
{
    public class UnitBase:BaseItem
    {

        public TypesOfObject Fraction { get; }
        public UnitBase(int i, int j, TypesOfObject obj) : base(i, j)
        {
            Fraction = obj;
            Color = obj == TypesOfObject.UnitBlack ? Colors.Black : Colors.White;
        }
       
    }
}
