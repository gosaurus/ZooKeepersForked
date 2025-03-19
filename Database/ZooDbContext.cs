using Microsoft.EntityFrameworkCore;
using ZooKeepers.Models;

namespace ZooKeepers.Data
{
    public class ZooDbContext : DbContext
    {
        public DbSet<Animal> Animals {get; set;}
        public DbSet<Enclosure> Enclosures {get; set;}

        public ZooDbContext(DbContextOptions<ZooDbContext>options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                Console.WriteLine("Using fallback configuration on database. To change this, add a DefaultConnection setting in appsettings.json.");
                optionsBuilder.UseSqlite("Data Source = ZooDb.db");
            }
        }
    }

}