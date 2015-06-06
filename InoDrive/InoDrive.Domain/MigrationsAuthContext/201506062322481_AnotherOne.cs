namespace InoDrive.Domain.MigrationsAuthContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnotherOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bids",
                c => new
                {
                    BidId = c.Int(nullable: false, identity: true),
                    IsAccepted = c.Boolean(),
                    IsWatchedByOwnerUser = c.Boolean(nullable: false),
                    IsWatchedByAssignedUser = c.Boolean(nullable: false),
                    CreationDate = c.DateTime(nullable: false),
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
                    ISAllowdedPets = c.Boolean(nullable: false),
                    IsAllowdedMusic = c.Boolean(nullable: false),
                    IsAllowdedEat = c.Boolean(nullable: false),
                    IsAllowdedDrink = c.Boolean(nullable: false),
                    IsAllowdedSmoke = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    About = c.String(),
                    CarDescription = c.String(),
                    CarImage = c.String(),
                    CarImageExtension = c.String(),
                    CarClass = c.String(),
                    OriginCityId = c.Int(nullable: false),
                    DestinationCityId = c.Int(nullable: false),
                    PayForOne = c.Decimal(storeType: "money"),
                })
                .PrimaryKey(t => t.TripId)
                .ForeignKey("dbo.Places", t => t.DestinationCityId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Places", t => t.OriginCityId)
                .Index(t => t.UserId)
                .Index(t => t.OriginCityId)
                .Index(t => t.DestinationCityId);

            CreateTable(
                "dbo.Places",
                c => new
                {
                    PlaceId = c.Int(nullable: false, identity: true),
                    GooglePlaceId = c.String(),
                    Name = c.String(),
                    AdditionalInfo = c.String(),
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
                    Index = c.Int(nullable: false),
                    TripId = c.Int(nullable: false),
                    CityId = c.Int(nullable: false),
                    City_PlaceId = c.Int(),
                })
                .PrimaryKey(t => t.WayPointId)
                .ForeignKey("dbo.Places", t => t.City_PlaceId)
                .ForeignKey("dbo.Trips", t => t.TripId)
                .Index(t => t.TripId)
                .Index(t => t.City_PlaceId);
        }
        
        public override void Down()
        {
        }
    }
}
