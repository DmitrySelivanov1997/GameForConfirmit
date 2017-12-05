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
        public IItem Base { get; set; }
        public IStrategy Strategy { get; set; }

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army, int mapLength)
        {
            if (Base == null)
                Strategy=new Exploring(mapLength);
            else
            {
                int t = 0;
            }
            foreach (var unit in army)
            {
                Base = unit.ScopeArray.Cast<IItem>().FirstOrDefault(x=>  unit.TypeOfObject==TypesOfObject.UnitBlack? x.TypeOfObject==TypesOfObject.BaseWhite:x.TypeOfObject==TypesOfObject.BaseBlack);
                unit.Direction=Strategy.FindUnitDirection(unit);
            }
        }
        private  bool IsBase(IItem p)
        {
            return p.TypeOfObject == TypesOfObject.BaseBlack;
        }

    }
}
