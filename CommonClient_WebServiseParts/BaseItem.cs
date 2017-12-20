using System;
using System.Windows.Media;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class BaseItem : IEquatable<BaseItem>, IItem
    {
        public int I { get; set; }
        public int J { get; set; }
        public Color Color { get; set; }
        public int X => J;
        public int Y => I;
        public TypesOfObject TypeOfObject { get; set; }

        public BaseItem(int i, int j , TypesOfObject obj)
        {
            TypeOfObject = obj;
            J = j;
            I = i;
        }

        public bool Equals(BaseItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BaseItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(BaseItem left, BaseItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseItem left, BaseItem right)
        {
            return !Equals(left, right);
        }
    }
}
