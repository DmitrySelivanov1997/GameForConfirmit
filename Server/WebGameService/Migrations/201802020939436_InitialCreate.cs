namespace WebGameService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameSessionStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameStartTime = c.DateTime(nullable: false),
                        GameDuration = c.DateTime(nullable: false),
                        TurnsNumber = c.Int(nullable: false),
                        MapSize = c.Int(nullable: false),
                        GameResult = c.String(),
                        WhiteAlgorithmName = c.String(),
                        BlackAlgorithmName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GameSessionStatistics");
        }
    }
}
