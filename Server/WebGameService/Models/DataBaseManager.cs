using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebGameService.Models
{
    public class DataBaseManager
    {
        private const int NumberOfObjectsInList = 25;
        public List<GameSessionStatistic> GetRangeOfObjects(int startPosition)
        {
            List < GameSessionStatistic > selectedList=new List<GameSessionStatistic>();
            using (var db = new GameSessionStatisticContext())
            {
                try
                {
                    selectedList = db.GameSessionStatistics.ToList().GetRange(startPosition, NumberOfObjectsInList);
                }
                catch (Exception e)
                {
                    if (db.GameSessionStatistics.Count() <= startPosition)
                        return null;
                    selectedList = db.GameSessionStatistics.ToList().GetRange(startPosition, db.GameSessionStatistics.Count()-startPosition);
                }
            }
            return selectedList;
        }
    }
}