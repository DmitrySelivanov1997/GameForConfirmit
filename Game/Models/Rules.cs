using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class Rules
    {
        public Rules()
        {
            Brick.Probability = 0.0;
            Food.Probability = 0.01;
            
        }
        public void GetNewPositionAccordingToDirection(ref int yNew,ref int xNew, IUnit unit)
        {
            switch (unit.Direction)
            {
                case Direction.Down:
                    xNew = unit.X;
                    yNew = unit.Y + 1;
                    break;
                case Direction.Left:
                    xNew = unit.X - 1;
                    yNew = unit.Y;
                    break;
                case Direction.Right:
                    xNew = unit.X + 1;
                    yNew = unit.Y;
                    break;
                case Direction.Up:
                    xNew = unit.X;
                    yNew = unit.Y - 1;
                    break;
                case Direction.Stay:
                    break;
            }
        }

        public bool ShouldUnitsBeUpdated(IItem item)
        {
            if (item is FreeSpace || item is Food)
                return true;
                return false;
        }
    }
}
