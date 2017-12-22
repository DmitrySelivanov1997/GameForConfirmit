using CommonClient_WebServiseParts;
using InterfaceLibrary;

namespace WebGameService.Models.EngineLogic
{
    public class Rules
    {
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
