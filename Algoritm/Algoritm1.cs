using System;
using System.Collections.Generic;
using InterfaceLibrary;

namespace Algoritm
{
    public class Algoritm1:IAlgoritm
    {
        public List<Node> GraphNodes=new List<Node>();
        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            foreach (var unit in army)
            {
                if (!GraphNodes.Contains(new Node(unit.Y, unit.X)))
                {
                    GraphNodes.Add(new Node(unit.Y, unit.X));
                }
                MakeDfsStep(unit);
            }
        }

        private void MakeDfsStep(IUnit unit)
        {
            GraphNodes.Find(x => x.X == unit.X && x.Y == unit.Y).Condition = Condition.Grey;
            FindDirection(ref unit);
        }

        private void FindDirection(ref IUnit unit)
        {
            if (unit.ScopeArray[6, 6-1].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6, 6-1].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(unit.Y, unit.X-1)))
                {
                    unit.Direction = Direction.Left;
                    return;
                }
            }
            if (unit.ScopeArray[6, 6+1].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6, 6+1].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(unit.Y, unit.X + 1)))
                {
                    unit.Direction = Direction.Right;
                    return;
                }
            }
            if (unit.ScopeArray[6-1, 6].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6-1, 6].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(6-1, 6)))
                {
                    unit.Direction = Direction.Up;
                    return;
                }
            }
            if (unit.ScopeArray[6+1, 6].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6+1, 6].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(6,6)))
                {
                    unit.Direction = Direction.Down;
                    return;
                }
            }
            if (unit.ScopeArray[6, 6 - 1].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6, 6 - 1].TypeOfObject == TypesOfObject.Food)
            {
                if (GraphNodes.Contains(new Node(unit.Y, unit.X - 1)))
                {
                    unit.Direction = Direction.Left;
                    return;
                }
            }
            if (unit.ScopeArray[6, 6 + 1].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6, 6 + 1].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(unit.Y, unit.X + 1)))
                {
                    unit.Direction = Direction.Right;
                    return;
                }
            }
            if (unit.ScopeArray[6 - 1, 6].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6 - 1, 6].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(unit.Y - 1, unit.X)))
                {
                    unit.Direction = Direction.Up;
                    return;
                }
            }
            if (unit.ScopeArray[6, 6].TypeOfObject == TypesOfObject.FreeSpace ||
                unit.ScopeArray[6, 6].TypeOfObject == TypesOfObject.Food)
            {
                if (!GraphNodes.Contains(new Node(unit.Y + 1, unit.X)))
                {
                    unit.Direction = Direction.Down;
                    return;
                }
            }
        }

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army, int mapLength)
        {
            throw new NotImplementedException();
        }
    }
}
