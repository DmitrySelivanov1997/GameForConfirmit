using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Interfaces;
using Game.Models.BaseItems;

namespace Game.Models
{
    public class Engine
    {
        public IPrinter Printer { get; set; }
        private Map Map { get; }
        public IAlgoritm FirstAlgoritm { get; set; }
        public IAlgoritm SecondAlgoritm { get; set; }

        public Engine(IAlgoritm firstAlgoritm, IAlgoritm secondAlgoritm, Map map)
        {
            Map = map;
            FirstAlgoritm = firstAlgoritm;
            SecondAlgoritm = secondAlgoritm;
        }

        public void Startbattle()
        {
            for (;;)
            {
                FirstAlgoritm.MoveAllUnits(Map.WhiteArmy);
                UpdateUnits(Map.WhiteArmy);
                SecondAlgoritm.MoveAllUnits(Map.BlackArmy);
                UpdateUnits(Map.BlackArmy);

            }
        }

        private void UpdateUnits(List<Unit> mapWhiteArmy)
        {
            
        }
    }
}
