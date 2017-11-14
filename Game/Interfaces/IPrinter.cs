using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models;
using Game.Models.BaseItems;

namespace Game.Interfaces
{
    public interface IPrinter
    {
        void Print(Map map);
        void UpdateItem( BaseItem item);
    }
}
