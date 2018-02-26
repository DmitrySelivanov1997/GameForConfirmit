using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace WebGameService.Models
{
    public class GameSessionStatisticContext : DbContext
    {
        public DbSet<GameSessionStatistic> GameSessionStatistics { get; set; }

        public GameSessionStatisticContext():base("GameSessionStatisticContext")
        {
            Database.SetInitializer(new ProjectInitializer());
        }
    }
    public class ProjectInitializer : MigrateDatabaseToLatestVersion<GameSessionStatisticContext, Configuration>
    {
    }
    public sealed class Configuration : DbMigrationsConfiguration<GameSessionStatisticContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GameSessionStatisticContext context)
        {

        }
    }

}