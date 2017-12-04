using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Algoritm
{
    public class Algoritm1:IAlgoritm
    {
        private Random Rnd = new Random();
        public Base EnemyBase { get; set; }
        private Dictionary<Guid,List<Node>[]> UnitsPaths { get; set; }= new Dictionary<Guid, List<Node>[]>();
        public List<Node> GraphNodes=new List<Node>();

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            while (true)
            {
                foreach (var unit in army)
                {
                    if (!UnitsPaths.ContainsKey(unit.Id))
                    {
                        UnitsPaths.Add(unit.Id, new[] { new List<Node>(), new List<Node>() });
                        UnitsPaths[unit.Id][0].Add(new Node(unit.Y, unit.X) { Condition = Condition.Grey });
                    }
                    else if (UnitsPaths[unit.Id][0].Last() != new Node(unit.Y, unit.X))
                    {
                        UnitsPaths[unit.Id][0].Add(new Node(unit.Y, unit.X) { Condition = Condition.Grey });
                    }
                    if (!GraphNodes.Contains(UnitsPaths[unit.Id][0].Last()))
                        GraphNodes.Add(UnitsPaths[unit.Id][0].Last());
                    MakeDfsStep(unit);
                    for (int i = 0; i < unit.ScopeArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < unit.ScopeArray.GetLength(1); j++)
                        {
                            if (unit.ScopeArray[i, j] is Base @base && unit.Color != unit.ScopeArray[i, j].Color)
                                EnemyBase = @base;
                        }
                    }
                }
                break;
                

            }
            //if (EnemyBase != null)
            //{
            //    foreach (var unit in army)
            //    {
            //        unit.Direction = Direction.Left;
            //    }


            //}

        }

        private void MakeDfsStep(IUnit unit)
        {
           unit.Direction = FindDirection(unit);
        }

        private Direction FindDirection(IUnit unit)
        {
            if (IsPositionFree(unit.ScopeArray[6, 6 - 1], new Node(unit.Y, unit.X - 1), UnitsPaths[unit.Id][1]))
                if (!UnitsPaths[unit.Id][0].Contains(new Node(unit.Y, unit.X - 1)))
                {
                    return Direction.Left;
                }
            if (IsPositionFree(unit.ScopeArray[6, 6 + 1], new Node(unit.Y, unit.X + 1), UnitsPaths[unit.Id][1]))
                if (!UnitsPaths[unit.Id][0].Contains(new Node(unit.Y, unit.X + 1)))
                {
                    return Direction.Right;
                }
            if (IsPositionFree(unit.ScopeArray[6 - 1, 6], new Node(unit.Y - 1, unit.X), UnitsPaths[unit.Id][1]))
                if (!UnitsPaths[unit.Id][0].Contains(new Node(unit.Y - 1, unit.X)))
                {
                    return Direction.Up;
                }
            if (IsPositionFree(unit.ScopeArray[6 + 1, 6], new Node(unit.Y + 1, unit.X), UnitsPaths[unit.Id][1]))
                if (!UnitsPaths[unit.Id][0].Contains(new Node(unit.Y + 1, unit.X)))
                {
                    return Direction.Down;
                }
            if(!UnitsPaths[unit.Id][1].Contains(new Node(unit.Y, unit.X)))
                UnitsPaths[unit.Id][1].Add(new Node(unit.Y, unit.X));
                return GetReturnDirection(UnitsPaths[unit.Id][0], unit);
        }

        private bool IsOtherUnitTheReasonOfStuck(IUnit unit)
        {
            if (unit.ScopeArray[6 - 1, 6] is UnitBase
                || unit.ScopeArray[6 + 1, 6] is UnitBase
                || unit.ScopeArray[6, 6 - 1] is UnitBase
                || unit.ScopeArray[6, 6 + 1] is UnitBase)
                return true;
            return false;
        }

        //private bool AreOtherUnitsTheReasonOfStuck(IUnit unit)
        //{
        //    if ((unit.X-1>=0 && unit.ScopeArray[unit.X - 1, unit.Y] is Unit) ||( unit.Y - 1 >= 0 && unit.ScopeArray[unit.X, unit.Y - 1] is Unit) || unit.Y + 1 < unit.ScopeArray.GetLength(0)&&
        //        unit.ScopeArray[unit.X + 1, unit.Y] is Unit || unit.Y + 1 <unit.ScopeArray.GetLength(0) && unit.ScopeArray[unit.X, unit.Y + 1] is Unit)
        //        return true;
        //    return false;
        //}

        private bool IsPositionFreeAndNotGrey(IItem item, Node node, List<Node> nodeList)
        {
            if ((item is FreeSpace || item is Food)
                && !GraphNodes.Contains(node)
                && !nodeList.Contains(node))
            {
                return true;
            }
            return false;
        }

        private Direction GetReturnDirection(List<Node> unitsPath, IUnit unit)
        {
            var currentLocation = unitsPath.Last();
            unitsPath.Remove(unitsPath.Last());
            if (unitsPath.Count != 0)
            {
                if (unitsPath.Last().Y < currentLocation.Y)
                {
                    if (unit.ScopeArray[5, 6] is UnitBase)
                    {

                        if (IsFree(unit.ScopeArray[6, 6 - 1]))
                            {
                                return Direction.Left;
                            }
                        if (IsFree(unit.ScopeArray[6, 6 + 1]))
                            {
                                return Direction.Right;
                            }
                        if (IsFree(unit.ScopeArray[6 - 1, 6]))
                            {
                                return Direction.Up;
                            }
                        if (IsFree(unit.ScopeArray[6 + 1, 6]))
                            {
                                return Direction.Down;
                            }
                    }
                    return Direction.Up;
                }
                if (unitsPath.Last().Y > currentLocation.Y)
                {
                    if (unit.ScopeArray[7, 6] is UnitBase)
                    {

                        if (IsFree(unit.ScopeArray[6, 6 - 1]))
                        {
                            return Direction.Left;
                        }
                        if (IsFree(unit.ScopeArray[6, 6 + 1]))
                        {
                            return Direction.Right;
                        }
                        if (IsFree(unit.ScopeArray[6 - 1, 6]))
                        {
                            return Direction.Up;
                        }
                        if (IsFree(unit.ScopeArray[6 + 1, 6]))
                        {
                            return Direction.Down;
                        }
                    }
                    return Direction.Down;
                }
                if (unitsPath.Last().X > currentLocation.X)
                {

                    if (unit.ScopeArray[6, 7] is UnitBase)
                    {

                        if (IsFree(unit.ScopeArray[6, 6 - 1]))
                        {
                            return Direction.Left;
                        }
                        if (IsFree(unit.ScopeArray[6, 6 + 1]))
                        {
                            return Direction.Right;
                        }
                        if (IsFree(unit.ScopeArray[6 - 1, 6]))
                        {
                            return Direction.Up;
                        }
                        if (IsFree(unit.ScopeArray[6 + 1, 6]))
                        {
                            return Direction.Down;
                        }
                    }
                    return Direction.Right;
                }

                if (unit.ScopeArray[6, 5] is UnitBase)
                {

                    if (IsFree(unit.ScopeArray[6, 6 - 1]))
                    {
                        return Direction.Left;
                    }
                    if (IsFree(unit.ScopeArray[6, 6 + 1]))
                    {
                        return Direction.Right;
                    }
                    if (IsFree(unit.ScopeArray[6 - 1, 6]))
                    {
                        return Direction.Up;
                    }
                    if (IsFree(unit.ScopeArray[6 + 1, 6]))
                    {
                        return Direction.Down;
                    }
                }
                return Direction.Left;
            }
            unitsPath.Add(currentLocation);
            return Direction.Stay;
        }

        private bool IsPositionFree(IItem item, Node node, List<Node> nodeList)
        {
            if ((item is FreeSpace || item is Food)
                && !nodeList.Contains(node))
            {
                return true;
            }
            return false;
        }
        private bool IsFree(IItem item)
        {
            if ((item is FreeSpace || item is Food))
            {
                return true;
            }
            return false;
        }

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army, int mapLength)
        {
            throw new NotImplementedException();
        }
    }
}
