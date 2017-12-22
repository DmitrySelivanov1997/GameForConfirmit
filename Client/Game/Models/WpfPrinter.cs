using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommonClient_WebServiseParts;
using Game.Interfaces;
using InterfaceLibrary;

namespace Game.Models
{
    public class WpfPrinter:IPrinter
    {
        public Image MainImage { get; }
        public WpfPrinter(Image mainImage )
        {
            MainImage = mainImage;
        }
        public void Print(TypesOfObject[,] array, WriteableBitmap writeableBitmap)
        {
            if(array == null)
                return;
            var map=new Map(array.GetLength(0)){Array = array};
            using (writeableBitmap.GetBitmapContext())
            {
                for (var i = 0; i < map.GetLength(); i++)
                {
                    for (var j = 0; j < map.GetLength(); j++)
                    {
                            AddObjectOnBmp(i, j, map.GetItem(i, j), writeableBitmap, map);
                    }
                }
            }


        }
        private void AddObjectOnBmp(int j, int i, IItem item, WriteableBitmap writeableBitmap, Map map)
        {
            writeableBitmap.FillRectangle(
                (int) (i * MainImage.Height / map.GetLength()),
                (int)(j * MainImage.Width / map.GetLength()),
                (int)((i +1)*( MainImage.Width / map.GetLength())), 
                (int)((j + 1) * ( MainImage.Height / map.GetLength())),
                item.Color
                );
            if(item is Base )
                writeableBitmap.FillRectangle(
                    (int)(i * MainImage.Height / map.GetLength() + MainImage.Height / map.GetLength() / 4),
                    (int)(j * MainImage.Width / map.GetLength() + MainImage.Height / map.GetLength() / 4),
                    (int)((i + 1) * (MainImage.Width / map.GetLength()) - MainImage.Height / map.GetLength() / 4),
                    (int)((j + 1) * (MainImage.Height / map.GetLength()) - MainImage.Height / map.GetLength() / 4),
                    Colors.Red
                );

        }
    }
}
