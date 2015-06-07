namespace InoDrive.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Sex", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Sex");
        }
    }
}
