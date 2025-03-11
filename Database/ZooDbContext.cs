using Microsoft.EntityFrameworkCore;
using ZooKeepers.Models;

namespace ZooKeepers.Data
{
    public class ZooDbContext : DbContext
    {
        public DbSet<Animal> Animals{get; set;}

        public ZooDbContext(DbContextOptions<ZooDbContext>options) : base(options) {}


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source = ZooDb.db");
            }
        }
    }

}