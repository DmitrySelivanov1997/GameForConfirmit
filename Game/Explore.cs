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
    public class Explore:CommonStrategyPart
    {

        public Explore(int size) :base(size)
        {
        }

        public Explore(IItem[,] map) : base(map)
        {
        }


        public override Point GetNewDestinationPoint(IUnit unit)
        {
            Point point = new Point();
            Point food = new Point();
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    switch (Map[i, j])
                    {
                        case Food _:
                            if (IsPointAccessibleAndClosereThanOther(new Point(unit.Y, unit.X), new Point(i, j), food))
                                food = new Point(i, j);
                            break;
                        case null:
                            if (IsPointAccessibleAndClosereThanOther(new Point(unit.Y, unit.X), new Point(i, j), point ))
                                point = new Point(i, j);
                            break;
                    }
                }
            }
            return food.Y == -1 ? point : new Point(food.Y, food.X);
        }



    }
}
