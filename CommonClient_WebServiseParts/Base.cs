using System;
using System.Windows.Media;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class Base: BaseItem
    {
        private Map Map { get; }
        private IItem[,] Scope { get; set; }

        public Base(int i, int j, TypesOfObject obj, Map map) : base(i, j, obj)
        {
            TypeOfObject = obj;
            Color = TypeOfObject == TypesOfObject.BaseBlack ? Colors.Black : Colors.White;
            Scope = new IItem[7, 7];
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
