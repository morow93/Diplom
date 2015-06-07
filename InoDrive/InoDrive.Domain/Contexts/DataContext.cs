using InoDrive.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("InoDrive")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<WayPoint> WayPoint { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
