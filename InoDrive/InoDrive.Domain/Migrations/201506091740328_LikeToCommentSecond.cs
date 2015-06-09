namespace InoDrive.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikeToCommentSecond : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Likes", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "UserId" });
            DropIndex("dbo.Likes", new[] { "TripId" });
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Vote = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TripId);
            
            DropTable("dbo.Likes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        Vote = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TripId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LikeId);
            
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "TripId", "dbo.Trips");
            DropIndex("dbo.Comments", new[] { "TripId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Comments");
            CreateIndex("dbo.Likes", "TripId");
            CreateIndex("dbo.Likes", "UserId");
            AddForeignKey("dbo.Likes", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Likes", "TripId", "dbo.Trips", "TripId");
        }
    }
}
