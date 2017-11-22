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
       public Random Rnd1=new Random();
        public void MoveAllUnits(IReadOnlyCollection<IUnit> army)
        {
            foreach (var unit in army)
            {
                unit.Move((Direction)Rnd1.Next(0,5));
            }
        }
    }
}
