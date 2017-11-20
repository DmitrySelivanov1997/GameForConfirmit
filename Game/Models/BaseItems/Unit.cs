using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class Unit:UnitBase
    {
        public Direction Direction { get; set; }
        public BaseItem[,] ScopeArray { get; set; }
        private Map Map { get; }
        public Unit(int x, int y, Color color, Map map) : base(x, y, color)
        {
            Map = map;
            ScopeArray = GetAllObjectsInScopeArray();
            
        }
        
        

        private BaseItem[,] GetAllObjectsInScopeArray()
        {
            int x = 0;
            int y = 0;
            var array = new BaseItem[13,13];
            for (int i = Y - 6; i <= Y + 6; i++)
            {
                for (int j = X - 6; j <= X + 6; j++)
                {
                    if ((Math.Abs(i - Y )+Math.Abs(j-X)) <= 6)
                    {
                        if (i != X || j != Y)
                            array[y,x] = Map.GetItem(i, j);
                    }

                    x++;
                }
                x = 0;
                y++;

            }
            return array;
        }

        public void Move(Direction direction)
        {
            Direction = direction;
        }


        public bool DieOrSurvive()
        {
            ScopeArray = GetAllObjectsInScopeArray();
            var allies=1;
            var foes = 0;
            foreach (var item in ScopeArray)
            {
                if (item!=null && item is UnitBase)
                {
                    if (Math.Abs(item.X - X) + Math.Abs(item.Y - Y) <= 3)
                    {
                        if (Color == item.Color)
                            allies++;
                        else
                            foes++;
                    }
                }
            }
            return foes > allies;
        }
    }
}
