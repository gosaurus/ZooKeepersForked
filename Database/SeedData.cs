using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZooKeepers.Models;

namespace ZooKeepers.Data
{
    public static class ZooKeepersAnimals
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var zoodbcontext = new ZooDbContext(serviceProvider.GetRequiredService<DbContextOptions<ZooDbContext>>()))
            {

                if (zoodbcontext.Animals.Any()) return;

                zoodbcontext.Animals.AddRange(
                     new Animal { Name = "Simba", Sex = "Male", Species = "Lion", Classification = "Mammal", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)) },
                     new Animal { Name = "Mustafa", Sex = "Male", Species = "Lion", Classification = "Mammal", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) },
                     new Animal { Name = "Pumbaa", Sex = "Male", Species = "Warthog", Classification = "Mammal", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) },
                     new Animal { Name = "Timon", Sex = "Male", Species = "Meerkat", Classification = "Mammal", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-6)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)) },
                     new Animal { Name = "Rafiki", Sex = "Male", Species = "Mandrill", Classification = "Mammal", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
                     new Animal { Name = "Nala", Sex = "Female", Species = "Lion", Classification = "Mammal", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
                     new Animal { Name = "Kenge", Sex = "Male", Species = "Lizard", Classification = "Reptiles", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
                     new Animal { Name = "Zazu", Sex = "Male", Species = "Red-billed horn bill", Classification = "Bird", DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)), DateAcquired = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) }
                 );
                zoodbcontext.SaveChanges();
            }
        }
    }
}
