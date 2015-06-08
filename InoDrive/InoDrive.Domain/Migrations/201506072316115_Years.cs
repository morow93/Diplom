namespace InoDrive.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Years : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "YearOfBirth", c => c.Int());
            AddColumn("dbo.AspNetUsers", "YearOfStage", c => c.Int());
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "Stage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Stage", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTimeOffset(precision: 7));
            DropColumn("dbo.AspNetUsers", "YearOfStage");
            DropColumn("dbo.AspNetUsers", "YearOfBirth");
        }
    }
}
