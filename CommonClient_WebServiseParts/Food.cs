using System.Windows.Media;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class Food:BaseItem
    {
        public Food(int i, int j, TypesOfObject obj): base(i, j,obj)
        {
            Color = Colors.Green;
        }
        
    }
}
