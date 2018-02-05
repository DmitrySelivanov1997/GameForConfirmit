using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebGameService.Models
{
    public class GameSessionStatisticContext : DbContext
    {
        public DbSet<GameSessionStatistic> GameSessionStatistics { get; set; }

        public GameSessionStatisticContext():base("GameSessionStatisticContext") 
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GameSessionStatisticContext>());
        }
    }
}