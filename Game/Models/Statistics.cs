using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using InterfaceLibrary;

namespace Game.Models
{
    public class Statistics
    {
        public int FoodEaten { get; set; }
        public int CurrentArmyNumber { get; set; }
        public int EnemiesKilled { get; set; }
        public void IncFood()
        {
            FoodEaten++;
        }

        public void SetArmyNumber(List<IUnitManagable> army)
        {
            CurrentArmyNumber = army.Count;
        }

        public void EnemyGotKilled()
        {
            EnemiesKilled++;
        }
    }
}
