using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Game.Models.BaseItems
{
    public class BaseItem : IEquatable<BaseItem>
    {
        public Color Color { get; set; }
        public int X { get; }
        public int Y { get; }

        public BaseItem(int y, int x)
        {
            X = x;
            Y = y;
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
            if (obj.GetType() != this.GetType()) return false;
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
