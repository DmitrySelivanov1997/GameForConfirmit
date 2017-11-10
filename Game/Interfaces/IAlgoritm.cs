using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models.BaseItems;

namespace Game.Interfaces
{
    public interface IAlgoritm
    {
        void MoveAllUnits(List<Unit> army);
    }
}
