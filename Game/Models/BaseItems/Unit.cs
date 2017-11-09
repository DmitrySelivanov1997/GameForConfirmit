using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class Unit:BaseItem
    {
        public TypesOfObject[,] ScopeArray { get; set; }
        public Map Map { get; }
        public Unit(int x, int y,  Color color, Map map) : base(x, y)
        {
            Map = map;
            Color = color;
        }

        public void Move(Direction direction)
        {
            
        }
    }
}
