using System.Windows.Media;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class FreeSpace:BaseItem
    {
        public FreeSpace(int i, int j, TypesOfObject obj):base(i, j,obj)
        {
           Color = Colors.Bisque;
        }
    }
}
