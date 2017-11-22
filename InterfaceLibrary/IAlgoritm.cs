using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceLibrary
{
    public interface IAlgoritm
    {
        void MoveAllUnits(IReadOnlyCollection<IUnit> army);
    }
}
