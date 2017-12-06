using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game
{
    public class Attack:CommonStrategyPart
    {
        public IItem Base { get; set; }

        public Attack(IItem[,] map, IItem baseItem):base(map)
        {
            Base = baseItem;
        }
        public override Point GetNewDestinationPoint(IUnit unit)
        {
            var startingPoint = new Point(Base.Y, Base.X);
            Point point = new Point(unit.Y, unit.X);
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    if(Map[i,j] is FreeSpace || Map[i, j] is Food)
                        if (IsPointAccessibleAndClosereThanOther(startingPoint, new Point(i,j), point))
                           point = new Point(i, j);
                }
            }
            return point;
        }
    }
}
