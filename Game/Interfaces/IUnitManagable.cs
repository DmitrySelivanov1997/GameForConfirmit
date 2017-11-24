using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceLibrary
{
    public interface IUnitManagable:IUnit
    {
        int I { get; set; }
        int J { get; set; }
    }
}
