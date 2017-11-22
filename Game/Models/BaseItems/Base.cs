using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using InterfaceLibrary;

namespace Game.Models.BaseItems
{
    public class Base: BaseItem
    {

        private Map Map { get; }
        private IItem[,] Scope { get; set; }
        public Base(int x, int y, Color color, Map map) : base(x, y)
        {
            Color = color;
            Scope = new IItem[7,7];
            Map = map;
        }

        private void GetScopeAroundBase()
        {
            int x = 0;
            int y = 0;
            var array = new IItem[13, 13];
            for (int i = Y - 3; i <= Y + 3; i++)
            {
                for (int j = X - 3; j <= X + 3; j++)
                {
                    if ((Math.Abs(i - Y) + Math.Abs(j - X)) <= 3)
                    {
                        if (i != X || j != Y)
                            array[y, x] = Map.GetItem(i, j);
                    }

                    x++;
                }
                x = 0;
                y++;

            }
            Scope = array;
        }

        public bool GetIsAlive()
        {
            GetScopeAroundBase();
            var foes = 0;
            foreach (var item in Scope)
            {
                if (item != null && item is UnitBase)
                {
                    if (Math.Abs(item.X - X) + Math.Abs(item.Y - Y) <= 3)
                    {
                        if (Color == item.Color)
                            return true;
                        foes++;
                    }
                }
            }
            return foes == 0;
        }
    }
}
