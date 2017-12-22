using InterfaceLibrary;

namespace WaveAlgorithm
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
                    if (Map[i, j] == null)
                    {
                        if (IsPointAccessibleAndClosereThanOther(new Point(unit.Y, unit.X), new Point(i, j), point))
                            point = new Point(i, j);
                        continue;
                    }
                    if(Map[i, j].TypeOfObject == TypesOfObject.Food)
                    {
                            if (IsPointAccessibleAndClosereThanOther(new Point(unit.Y, unit.X), new Point(i, j), food))
                                food = new Point(i, j);
                    }
                }
            }
            return food.Y == -1 ? point : new Point(food.Y, food.X);
        }



    }
}
