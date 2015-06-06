namespace InoDrive.Domain.MigrationsAuthContext
{
    using InoDrive.Domain.Entities;
    using InoDrive.Domain.Helpers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<InoDrive.Domain.Contexts.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"MigrationsAuthContext";
            ContextKey = "InoDrive.Domain.Contexts.AuthContext";
        }

        protected override void Seed(InoDrive.Domain.Contexts.AuthContext context)
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
