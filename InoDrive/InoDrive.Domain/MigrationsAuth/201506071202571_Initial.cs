namespace InoDrive.Domain.MigrationsAuth
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        DateOfBirth = c.DateTimeOffset(precision: 7),
                        DateOfStage = c.DateTimeOffset(precision: 7),
                        Phone = c.String(),
                        About = c.String(),
                        AvatarImage = c.String(),
                        AvatarImageExtension = c.String(),
                        Car = c.String(),
                        CarImage = c.String(),
                        CarImageExtension = c.String(),
                        CarClass = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Bids",
                c => new
                {
                    BidId = c.Int(nullable: false, identity: true),
                    IsAccepted = c.Boolean(),
                    IsWatchedBySender = c.Boolean(nullable: false),
                    IsWatchedByReceiver = c.Boolean(nullable: false),
                    CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    UserId = c.String(maxLength: 128),
                    TripId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.BidId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TripId);

            CreateTable(
                "dbo.Trips",
                c => new
                {
                    TripId = c.Int(nullable: false, identity: true),
                    UserId = c.String(maxLength: 128),
                    CreationDate = c.DateTimeOffset(nullable: false, precision: 7),
                    LeavingDate = c.DateTimeOffset(nullable: false, precision: 7),
                    EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                    PeopleCount = c.Int(nullable: false),
                    IsAllowdedDeviation = c.Boolean(nullable: false),
                    IsAllowdedChildren = c.Boolean(nullable: false),
                    IsAllowdedPets = c.Boolean(nullable: false),
                    IsAllowdedMusic = c.Boolean(nullable: false),
                    IsAllowdedEat = c.Boolean(nullable: false),
                    IsAllowdedDrink = c.Boolean(nullable: false),
                    IsAllowdedSmoke = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    About = c.String(),
                    Car = c.String(),
                    CarImage = c.String(),
                    CarImageExtension = c.String(),
                    CarClass = c.String(),
                    OriginPlaceId = c.String(nullable: false, maxLength: 128),
                    DestinationPlaceId = c.String(nullable: false, maxLength: 128),
                    Pay = c.Decimal(storeType: "money"),
                })
                .PrimaryKey(t => t.TripId)
                .ForeignKey("dbo.Places", t => t.DestinationPlaceId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Places", t => t.OriginPlaceId)
                .Index(t => t.UserId)
                .Index(t => t.OriginPlaceId)
                .Index(t => t.DestinationPlaceId);

            CreateTable(
                "dbo.Places",
                c => new
                {
                    PlaceId = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    About = c.String(),
                    Latitude = c.Double(nullable: false),
                    Longitude = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.PlaceId);

            CreateTable(
                "dbo.Likes",
                c => new
                {
                    LikeId = c.Int(nullable: false, identity: true),
                    Vote = c.Int(nullable: false),
                    UserId = c.String(maxLength: 128),
                    TripId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TripId);

            CreateTable(
                "dbo.WayPoints",
                c => new
                {
                    WayPointId = c.Int(nullable: false, identity: true),
                    WayPointIndex = c.Int(nullable: false),
                    TripId = c.Int(nullable: false),
                    PlaceId = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.WayPointId)
                .ForeignKey("dbo.Places", t => t.PlaceId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .Index(t => t.TripId)
                .Index(t => t.PlaceId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.Clients");
        }
    }
}
