namespace WebGameService.Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GameSessionStatistics", "GameStartTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GameSessionStatistics", "GameStartTime", c => c.String());
        }
    }
}
