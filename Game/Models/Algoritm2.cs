using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Models.BaseItems;
using InterfaceLibrary;

namespace Game.Models
{
    public class Algoritm2:IAlgoritm
    {

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            Random rnd2 = new Random();
            foreach (var unit in army)
            {
                unit.Move((Direction)rnd2.Next(0, 5));
            }
        }
    }
}
