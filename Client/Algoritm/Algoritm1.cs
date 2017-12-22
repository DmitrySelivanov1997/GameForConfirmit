using System;
using System.Collections.Generic;
using InterfaceLibrary;

namespace Algoritm
{
    public class Algoritm1:IAlgorithm
    {
        public Random Rnd;
        public Algoritm1()
        {
            Rnd= new Random();
        }
        public void MoveAllUnits(IReadOnlyCollection<IUnit> army, int mapLength)
        {
            foreach (var unit in army)
            {
                unit.Direction = (Direction)Rnd.Next(0, 5);
            }
        }
    }
}
