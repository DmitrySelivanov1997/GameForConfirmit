using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class Algoritm1:IAlgoritm
    {
       public Random Rnd=new Random();
        public void MoveAllUnits(IReadOnlyCollection<Unit> army)
        {
            foreach (var unit in army)
            {
                unit.Move((Direction)Rnd.Next(0,5));
            }
        }
    }
}
