using System.Windows.Media;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class UnitBase:BaseItem
    {
        public UnitBase(int i, int j, TypesOfObject obj) : base(i, j,obj)
        {
            Color = obj == TypesOfObject.UnitBlack ? Colors.Black : Colors.White;
        }
       
    }
}
