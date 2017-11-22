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
        public Random Rnd2=new Random();

        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            foreach (var unit in army)
            {
                unit.Move((Direction)Rnd2.Next(0, 5));
            }
        }
    }
}
