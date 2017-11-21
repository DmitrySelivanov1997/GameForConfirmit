using System;
using System.Collections.Generic;
using System.Text;

namespace CommonInterfaceLibrary
{
    interface IAlgoritm
    {
        void MoveAllUnits(IReadOnlyCollection<IUnit> army);
    }
}
