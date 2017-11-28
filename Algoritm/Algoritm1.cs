using System;
using System.Collections.Generic;
using InterfaceLibrary;

namespace Algoritm
{
    public class Algoritm1:IAlgoritm
    {
        public Random rnd;

        public Algoritm1()
        {
            rnd = new Random();
        }
        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            foreach (var unit in army)
            {
                unit.Move((Direction)rnd.Next(0,5));
            }
        }
    }
}
