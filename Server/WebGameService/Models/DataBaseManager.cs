using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebGameService.Models
{
    public class DataBaseManager
    {
        private int numberOfObjectsInList = 25;
        public List<GameSessionStatistic> GetRangeOfObjects(int startPosition)
        {
            List < GameSessionStatistic > selectedList=new List<GameSessionStatistic>();
            using (var db = new GameSessionStatisticContext())
            {
                selectedList = db.GameSessionStatistics.Where(p => p.Id <=startPosition+numberOfObjectsInList).ToList();
            }
            return selectedList;
        }
    }
}