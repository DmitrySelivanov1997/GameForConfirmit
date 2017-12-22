using System.Collections.Generic;
using InterfaceLibrary;

namespace CommonClient_WebServiseParts
{
    public class Statistics
    {
        public int NumberOfWins { get; set; }
        public int TurnNumber { get; set; }
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
