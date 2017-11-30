using System;
using System.Collections.Generic;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Algoritm
{
    public class Algoritm1:IAlgoritm
    {
        private Dictionary<Guid,Stack<Node>> UnitsPaths { get; set; }= new Dictionary<Guid, Stack<Node>>();
        public List<Node> GraphNodes=new List<Node>();
        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            foreach (var unit in army)
            {
                if (!UnitsPaths.ContainsKey(unit.Id))
                {
                    UnitsPaths.Add(unit.Id, new Stack<Node>());
                    UnitsPaths[unit.Id].Push(new Node(unit.Y, unit.X){Condition = Condition.Grey});
                }
                else if (UnitsPaths[unit.Id].Peek() != new Node(unit.Y, unit.X))
                {
                    UnitsPaths[unit.Id].Push(new Node(unit.Y, unit.X) {Condition = Condition.Grey});
                }
                if(!GraphNodes.Contains(UnitsPaths[unit.Id].Peek()))
                    GraphNodes.Add(UnitsPaths[unit.Id].Peek());
                MakeDfsStep(unit);
            }
        }

        private void MakeDfsStep(IUnit unit)
        {
           unit.Direction = FindDirection(unit);
        }

        private Direction FindDirection(IUnit unit)
        {


            if (IsPositionFreeAndNotGrey(unit.ScopeArray[6, 6 - 1], new Node(unit.Y, unit.X - 1)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y, unit.X - 1)))
                {
                    return Direction.Left;
                }
            if (IsPositionFreeAndNotGrey(unit.ScopeArray[6, 6 + 1], new Node(unit.Y, unit.X + 1)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y, unit.X + 1)))
                {
                    return Direction.Right;
                }
            if (IsPositionFreeAndNotGrey(unit.ScopeArray[6 - 1, 6], new Node(unit.Y - 1, unit.X)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y - 1, unit.X)))
                {
                    return Direction.Up;
                }
            if (IsPositionFreeAndNotGrey(unit.ScopeArray[6 + 1, 6], new Node(unit.Y + 1, unit.X)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y + 1, unit.X)))
                {
                    return Direction.Down;
                }

            if (IsPositionFree(unit.ScopeArray[6, 6 - 1], new Node(unit.Y, unit.X - 1)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y, unit.X - 1)))
                {
                    return Direction.Left;
                }
            if (IsPositionFree(unit.ScopeArray[6, 6 + 1], new Node(unit.Y, unit.X + 1)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y, unit.X + 1)))
                {
                    return Direction.Right;
                }
            if (IsPositionFree(unit.ScopeArray[6 - 1, 6], new Node(unit.Y - 1, unit.X)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y - 1, unit.X)))
                {
                    return Direction.Up;
                }
            if (IsPositionFree(unit.ScopeArray[6 + 1, 6], new Node(unit.Y + 1, unit.X)))
                if (!UnitsPaths[unit.Id].Contains(new Node(unit.Y + 1, unit.X)))
                {
                    return Direction.Down;
                }
            if (!AreOtherUnitsTheReasonOfStuck(unit))
            {

                GraphNodes.Remove(new Node(unit.Y, unit.X));
                GraphNodes.Add(new Node(unit.Y, unit.X) { Condition = Condition.Black });
            }
            return GetReturnDirection(UnitsPaths[unit.Id]);
        }

        private bool AreOtherUnitsTheReasonOfStuck(IUnit unit)
        {
            if ((unit.X-1>=0 && unit.ScopeArray[unit.X - 1, unit.Y] is Unit) ||( unit.Y - 1 >= 0 && unit.ScopeArray[unit.X, unit.Y - 1] is Unit) || unit.Y + 1 < unit.ScopeArray.GetLength(0)&&
                unit.ScopeArray[unit.X + 1, unit.Y] is Unit || unit.Y + 1 <unit.ScopeArray.GetLength(0) && unit.ScopeArray[unit.X, unit.Y + 1] is Unit)
                return true;
            return false;
        }

        private bool IsPositionFreeAndNotGrey(IItem item, Node node)
        {
            if ((item is FreeSpace || item is Food)
                && !GraphNodes.Contains(node)
                && !GraphNodes.Exists(x => x.Y == node.Y && x.X == node.X && x.Condition == Condition.Black))
            {
                return true;
            }
            return false;
        }

        private Direction GetReturnDirection(Stack<Node> unitsPath)
        {
            var currentLocation = unitsPath.Pop();
            if (unitsPath.Count != 0)
            {
                if (unitsPath.Peek().Y < currentLocation.Y)
                    return Direction.Up;
                if (unitsPath.Peek().Y > currentLocation.Y)
                    return Direction.Down;
                if (unitsPath.Peek().X > currentLocation.X)
                    return Direction.Right;
                return Direction.Left;
            }
            unitsPath.Push(currentLocation);
            return Direction.Stay;
        }

        private bool IsPositionFree(IItem item, Node node)
        {
            if ((item is FreeSpace || item is Food)
                && !GraphNodes.Exists(x => x.Y == node.Y && x.X == node.X && x.Condition == Condition.Black))
            {
                return true;
            }
            return false;
        }
    }
}
