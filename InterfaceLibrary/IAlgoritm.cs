using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceLibrary
{
    public interface IAlgorithm
    {
        void MoveAllUnits(IReadOnlyCollection<IUnit> army,int mapLength);
    }
}
