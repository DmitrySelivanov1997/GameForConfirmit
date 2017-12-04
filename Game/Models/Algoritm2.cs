using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class Point : IEquatable<Point>
    {
        public int Y { get; set; }
        public int X { get; set; }

        public Point()
        {
            Y = -1;
            X = -1;
        }

        public Point(int y, int x)
        {
            Y = y;
            X = x;
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Y == other.Y && X == other.X;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Y * 397) ^ X;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !Equals(left, right);
        }
    }

    public class Algoritm2 : IAlgoritm
    {
        public int Count = 0;
        public List<Point> TemporallyInaccessiblePoints { get; set; }
        public IItem[,] Map { get; set; }

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army, int mapLength)
        {
            if (Map == null)
                Map = new IItem[mapLength, mapLength];
            foreach (var unit in army)
            {
                TemporallyInaccessiblePoints = new List<Point>();
                UpdateMap(unit);
                unit.Direction = GetNewDestinationPoint(unit);
            }
        }

        private Direction GetNewDestinationPoint(IUnit unit)
        {
            Point point = new Point();
            Food food = null;
            for (var i = 0; i < Map.GetLength(0); i++)
            {
                for (var j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] is Food)
                        if (food == null || Math.Abs(i - unit.Y) + Math.Abs(j - unit.X) <
                            Math.Abs(food.Y - unit.Y) + Math.Abs(food.X - unit.X)
                            && !TemporallyInaccessiblePoints.Contains(new Point(i, j)))
                        {
                            food = new Food(i, j, TypesOfObject.Food);
                        }
                    if (Map[i, j] == null)
                        if (point.Y == -1 || Math.Abs(i - unit.Y) + Math.Abs(j - unit.X) <
                            Math.Abs(point.Y - unit.Y) + Math.Abs(point.X - unit.X)
                            && !TemporallyInaccessiblePoints.Contains(new Point(i, j)))
                        {
                            point = new Point(i, j);
                        }
                }
            }
            return food == null ? DoWaveAlgorithm(point.Y, point.X, unit) : DoWaveAlgorithm(food.Y, food.X, unit);
        }

        private void UpdateMap(IUnit unit)
        {
            for (var i = 0; i < unit.ScopeArray.GetLength(0); i++)
            {
                for (var j = 0; j < unit.ScopeArray.GetLength(1); j++)
                {
                    if (!(unit.ScopeArray[i, j] is Border) && unit.ScopeArray[i, j] != null)
                    {
                        Map[unit.Y - 6 + i, unit.X - 6 + j] = unit.ScopeArray[i, j];
                    }
                }
            }
        }

        private Direction DoWaveAlgorithm(int y, int x, IUnit unit)
        {
            var arrayForWaveSearch = new int[Map.GetLength(0), Map.GetLength(0)];
            arrayForWaveSearch[unit.Y, unit.X] = 1;
            List<Point> cellsQueue = new List<Point>();
            cellsQueue.Add(new Point(unit.Y, unit.X));
            CheckForNeigbourCells(ref cellsQueue, ref arrayForWaveSearch, unit, y, x);
            if (arrayForWaveSearch[y, x] == 0)
            {
                TemporallyInaccessiblePoints.Add(new Point(y, x));
                return GetNewDestinationPoint(unit);
            }
            else
            {
                return GetDirection(y, x, arrayForWaveSearch, unit);
            }
        }

        private Direction GetDirection(int y, int x, int[,] arrayForWaveSearch, IUnit unit)
        {
            int i = y;
            var j = x;
            while (arrayForWaveSearch[i, j] != 2)
            {
                SetClosestPoint(ref i, ref j, arrayForWaveSearch);
            }
            if (i < unit.Y)
                return Direction.Up;

            if (i > unit.Y)
                return Direction.Down;
            if (j < unit.X)
                return Direction.Left;
            return Direction.Right;
        }

        private void SetClosestPoint(ref int i, ref int j, int[,] arrayForWaveSearch)
        {
            if (i > 0)
                if (arrayForWaveSearch[i - 1, j] == arrayForWaveSearch[i, j] - 1)
                {
                    i--;
                    return;
                }
            if (i < arrayForWaveSearch.GetLength(0) - 1)
                if (arrayForWaveSearch[i + 1, j] == arrayForWaveSearch[i, j] - 1)
                {
                    i++;
                    return;
                }

            if (j > 0)
                if (arrayForWaveSearch[i, j - 1] == arrayForWaveSearch[i, j] - 1)
                {
                    j--;
                    return;
                }
            if (j < arrayForWaveSearch.GetLength(0) - 1)
                if (arrayForWaveSearch[i, j + 1] == arrayForWaveSearch[i, j] - 1)
                {
                    j++;
                }
        }

        private void CheckForNeigbourCells(ref List<Point> cellsQueue, ref int[,] arrayForWaveSearch, IUnit unit, int y,
            int x)
        {
            while (arrayForWaveSearch[y, x] == 0 && cellsQueue.Count!=0)
            {
                Count++;
                if (cellsQueue.First().Y != 0)
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y - 1,
                        cellsQueue.First().X, y, x); //Top cell
                if (cellsQueue.First().X + 1 < Map.GetLength(0))
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y,
                        cellsQueue.First().X + 1, y, x); //Right cell
                if (cellsQueue.First().X != 0)
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y,
                        cellsQueue.First().X - 1, y, x); //Left cell
                if (cellsQueue.First().Y + 1 < Map.GetLength(1))
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y + 1,
                        cellsQueue.First().X, y, x); //down cell
                cellsQueue.Remove(cellsQueue.First());
            }


        }

        private void SetNumberForNeigbourCell(ref List<Point> cellsQueue, ref int[,] arrayForWaveSearch, int i, int j,
            int y, int x)
        {
            if ((Map[i, j] is Food ||
                 Map[i, j] is FreeSpace || (i == y && j == x))
                && arrayForWaveSearch[i, j] == 0)
            {
                arrayForWaveSearch[i, j] = arrayForWaveSearch[cellsQueue.First().Y, cellsQueue.First().X] + 1;
                cellsQueue.Add(new Point(i, j));
            }
        }
    }
}
