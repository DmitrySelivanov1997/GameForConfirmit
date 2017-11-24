using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Game.Interfaces;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class WpfPrinter:IPrinter
    {
        public Image MainImage { get; }
        public WpfPrinter(Image mainImage )
        {
            MainImage = mainImage;
        }
        public void Print(Map map, WriteableBitmap writeableBitmap)
        {
            using (writeableBitmap.GetBitmapContext())
            {
                for (var i = 0; i < map.GetLength(); i++)
                {
                    for (var j = 0; j < map.GetLength(); j++)
                    {
                            AddObjectOnBmp(i, j, map.GetItem(i, j).Color, writeableBitmap, map);
                    }
                }
            }


        }
        private void AddObjectOnBmp(int j, int i, Color color, WriteableBitmap writeableBitmap, Map map)
        {
            writeableBitmap.FillRectangle(
                (int) (i * MainImage.Height / map.GetLength()),
                (int)(j * MainImage.Width / map.GetLength()),
                (int)((i +1)*( MainImage.Width / map.GetLength())), 
                (int)((j + 1) * ( MainImage.Height / map.GetLength())),
                color
                );
           
        }
    }
}
