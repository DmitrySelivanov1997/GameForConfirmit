using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceLibrary;


namespace Game.Models
{
    public class Algoritm1:IAlgoritm
    {
        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {

            Random rnd1 = new Random();
            foreach (var unit in army)
            {
                unit.Move((Direction)rnd1.Next(0,5));
            }
        }
    }
}
