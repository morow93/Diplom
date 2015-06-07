namespace InoDrive.Domain.Migrations
{
    using InoDrive.Domain.Entities;
    using InoDrive.Domain.Helpers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<InoDrive.Domain.Contexts.InoDriveContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(InoDrive.Domain.Contexts.InoDriveContext context)
        {
            if (context.Clients.Count() > 0)
            {
                return;
            }
            context.Clients.Add(new Client
            {
                Id = "InoDriveAngularApp",
                Secret = Helper.GetHash("abc@123"),
                Name = "Front-End Angualar Based SPA",
                ApplicationType = ApplicationTypes.JavaScript,
                Active = true,
                RefreshTokenLifeTime = 7200
            });
            context.SaveChanges();
        }
    }
}
