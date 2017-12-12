using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceLibrary;

namespace Game
{
    public interface IStrategy
    {
        IItem [,] Map { get; set; }
        Direction FindUnitDirection(IUnit unit);
    }
}
