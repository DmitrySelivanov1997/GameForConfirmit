﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game
{
    public class Exploring:IStrategy
    {
        public IItem[,] Map { get; set; }
        public List<Point> TemporallyInaccessiblePoints { get; set; }

        public Exploring(int size)
        {
            Map = new IItem[size,size];
        }
        public Direction FindUnitDirection(IUnit unit)
        {
            TemporallyInaccessiblePoints = new List<Point>();
            UpdateMap(unit);
            while (true)
            {
                var closestPoint = GetNewDestinationPoint(unit);
                var arrayAfterWaveAlgorithm = DoWaveAlgorithm(closestPoint, unit);
                if (arrayAfterWaveAlgorithm[closestPoint.Y, closestPoint.X] == 0)
                {
                    TemporallyInaccessiblePoints.Add(closestPoint);
                    if (TemporallyInaccessiblePoints.Count > 50)
                    {
                        return Direction.Stay;
                    }
                }
                else
                {
                    return GetDirection(closestPoint.Y, closestPoint.X, arrayAfterWaveAlgorithm, unit);
                }
            }
        }

        private bool IsPointAccessibleAndClosereThanOtherOrNull(IUnit unit, Point point, int i, int j)
        {
            return (point.Y == -1 || (Math.Abs(i - unit.Y) + Math.Abs(j - unit.X) < Math.Abs(point.Y - unit.Y) + Math.Abs(point.X - unit.X))) && !TemporallyInaccessiblePoints.Contains(new Point(i, j));
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

        private Point GetNewDestinationPoint(IUnit unit)
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
                            if (IsPointAccessibleAndClosereThanOtherOrNull(unit, food, i, j))
                                food = new Point(i, j);
                            break;
                        case null:
                            if (IsPointAccessibleAndClosereThanOtherOrNull(unit, point, i, j))
                                point = new Point(i, j);
                            break;
                    }
                }
            }
            return food.Y == -1 ? point : new Point(food.Y, food.X);
        }

        private int[,] DoWaveAlgorithm(Point point, IUnit unit)
        {
            var arrayForWaveSearch = new int[Map.GetLength(0), Map.GetLength(0)];
            arrayForWaveSearch[unit.Y, unit.X] = 1;
            var cellsQueue = new List<Point> { new Point(unit.Y, unit.X) };
            CheckForNeigbourCells(ref cellsQueue, ref arrayForWaveSearch, point);
            return arrayForWaveSearch;
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

        private void CheckForNeigbourCells(ref List<Point> cellsQueue, ref int[,] arrayForWaveSearch, Point point)
        {
            while (arrayForWaveSearch[point.Y, point.X] == 0 && cellsQueue.Count != 0)
            {
                if (cellsQueue.First().Y != 0)
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y - 1, cellsQueue.First().X, point); //Top cell
                if (cellsQueue.First().X + 1 < Map.GetLength(0))
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y, cellsQueue.First().X + 1, point); //Right cell
                if (cellsQueue.First().X != 0)
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y, cellsQueue.First().X - 1, point); //Left cell
                if (cellsQueue.First().Y + 1 < Map.GetLength(1))
                    SetNumberForNeigbourCell(ref cellsQueue, ref arrayForWaveSearch, cellsQueue.First().Y + 1, cellsQueue.First().X, point); //down cell
                cellsQueue.Remove(cellsQueue.First());
            }

        }

        private void SetNumberForNeigbourCell(ref List<Point> cellsQueue, ref int[,] arrayForWaveSearch, int i, int j, Point point)
        {
            if ((Map[i, j] is Food || Map[i, j] is FreeSpace || i == point.Y && j == point.X) && arrayForWaveSearch[i, j] == 0)
            {
                arrayForWaveSearch[i, j] = arrayForWaveSearch[cellsQueue.First().Y, cellsQueue.First().X] + 1;
                cellsQueue.Add(new Point(i, j));
            }
        }
    }
}
