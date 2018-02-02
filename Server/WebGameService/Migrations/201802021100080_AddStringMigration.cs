namespace WebGameService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStringMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GameSessionStatistics", "GameDuration", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GameSessionStatistics", "GameDuration", c => c.DateTime(nullable: false));
        }
    }
}
