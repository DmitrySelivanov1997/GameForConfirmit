using System;
using System.Collections.Generic;
using System.Text;

namespace CommonInterfaceLibrary
{
    interface IUnit
    {
        Direction Direction { get; set; }
        IItem[,] ScopeArray { get; set; }
        void Move(Direction direction);
        bool DieOrSurvive();
    }
}
