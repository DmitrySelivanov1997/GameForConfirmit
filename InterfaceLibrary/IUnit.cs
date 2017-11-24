using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceLibrary
{
    public interface IUnit:IItem
    {
        TypesOfObject Fraction { get; }
        Direction Direction { get; set; }
        IItem[,] ScopeArray { get; set; }
        void Move(Direction direction);
        bool DieOrSurvive();
    }
}
