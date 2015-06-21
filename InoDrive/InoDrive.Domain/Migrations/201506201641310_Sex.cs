namespace InoDrive.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Sex", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Sex", c => c.Boolean(nullable: false));
        }
    }
}
