using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class Unit:UnitBase
    {
        public Direction Direction { get; set; }
        public BaseItem[,] ScopeArray { get; }
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
            for (int i = X - 6; i <= X + 6; i++)
            {
                for (int j = Y - 6; j <= Y + 6; j++)
                {
                    if(i!=X || j!=Y)
                    array[x,y]=Map.GetItem(i, j);
                    y++;
                }
                y = 0;
                x++;

            }
            return array;
        }

        public void Move(Direction direction)
        {
            Direction = direction;
        }

        
    }
}
