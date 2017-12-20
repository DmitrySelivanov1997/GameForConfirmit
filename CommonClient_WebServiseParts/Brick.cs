using System.Windows.Media;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class Brick:BaseItem
    {
        public Brick(int i, int j, TypesOfObject obj): base(i, j, obj)
        {
            Color = Colors.Brown;
        }

      

    }
    
}
