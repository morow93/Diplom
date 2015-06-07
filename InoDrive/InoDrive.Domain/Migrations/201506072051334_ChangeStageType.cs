namespace InoDrive.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStageType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Stage", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "DateOfStage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DateOfStage", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.AspNetUsers", "Stage");
        }
    }
}
