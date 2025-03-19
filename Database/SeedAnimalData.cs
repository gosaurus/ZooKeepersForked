using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers;
using ZooKeepers.Constants;
using ZooKeepers.Models;

namespace ZooKeepers.Data
{
    public static class ZooKeepersSeed
    {
        public static bool createEnclosures(IServiceProvider serviceProvider)
        {
            using (var zoodbcontext = new ZooDbContext
                (serviceProvider.GetRequiredService<DbContextOptions<ZooDbContext>>()))
            {
                if (zoodbcontext.Enclosures.Any()) return false;

                var seededEnclosures = new List<Enclosure>();
                
                foreach (var enclosure in ReadOnlyProperties.enclosuresDict)
                {
                    var enclosureToAdd = new Enclosure 
                    {
                        Name = enclosure.Key,
                        MaxCapacity = enclosure.Value
                    };
                    zoodbcontext.Enclosures.Add(enclosureToAdd);
                    zoodbcontext.SaveChanges();
                }
            }
            return true;
        }
        public static void SeedAnimals(IServiceProvider serviceProvider)
        {
            using (var zoodbcontext = new ZooDbContext(serviceProvider.GetRequiredService<DbContextOptions<ZooDbContext>>()))
            {
                if(zoodbcontext.Animals.Any()) return;

                var allEnclosures = zoodbcontext.Enclosures;

                for (int count = 1; count <= 116; count++)
                {
                    string name = ReadOnlyProperties.animalNames[Random.Shared.Next(ReadOnlyProperties.animalNames.Count)] + count;
                    string sex = 
                        ReadOnlyProperties.SexOptions
                        [Random.Shared.Next(ReadOnlyProperties.SexOptions.Count)];
                    DateOnly dateOfBirth = GetRandomDate(2010, 2024);
                    DateOnly dateAcquired = dateOfBirth.AddMonths(-1*Random.Shared.Next(1, 18));
                    string species = ReadOnlyProperties.animalNames[Random.Shared.Next(ReadOnlyProperties.animalNames.Count)];
                    string classification = 
                        ReadOnlyProperties.ClassificationOptions
                        [Random.Shared.Next(ReadOnlyProperties.ClassificationOptions.Count)];
                    int enclosureid = Random.Shared.Next(1,5);

                    Animal animalToAdd = new(name, sex, dateOfBirth, dateAcquired, species, classification)
                    {
                            Name = name,
                            Sex = sex,
                            DateOfBirth = dateOfBirth,
                            DateAcquired = dateAcquired,      
                            Species = species,
                            Classification = classification,
                    };
                    
                    var enclosureToAdd = allEnclosures
                        .FirstOrDefault(enclosure => enclosure.EnclosureId == enclosureid);
                    
                    if (enclosureToAdd == null)
                        throw new Exception ("No matching enclosure");
                    enclosureToAdd.AddAnimal(animalToAdd);
                    zoodbcontext.SaveChanges();
                }
            }
        }

        public static void seedEnclosures(IServiceProvider serviceProvider)
        {
            using (var zoodbcontext = new ZooDbContext
                ( serviceProvider.GetRequiredService<DbContextOptions<ZooDbContext>>()))
            {
                var allEnclosures = zoodbcontext.Enclosures.ToList();
                var someAnimals = zoodbcontext.Animals.ToList();

                foreach (var enclosure in allEnclosures)
                {
                    for (var count = 0; count < enclosure.MaxCapacity; count++)
                    {
                        enclosure.AddAnimal(someAnimals[count]);
                        someAnimals[count].EnclosureId = enclosure.EnclosureId;
                        zoodbcontext.SaveChanges();
                    }
                    someAnimals.RemoveRange(0,enclosure.MaxCapacity-1);
                }
                zoodbcontext.SaveChanges();
            }
        }

        public static DateOnly GetRandomDate(int minYear = 2010, int maxYear = 2024)
        {
            var year = Random.Shared.Next(minYear, maxYear);
            var month = Random.Shared.Next(1, 12);
            var noOfDaysInMonth = DateTime.DaysInMonth(year, month);
            var day = Random.Shared.Next(1, noOfDaysInMonth);
            return new DateOnly(year, month, day);
        }
    }
}