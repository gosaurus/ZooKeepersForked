using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers;
using ZooKeepers.Models;

namespace ZooKeepers.Data
{
    public static class ZooKeepersSeed
    {
        private static readonly Dictionary<string, (string Species, string Classification)> zooanimals = new Dictionary<string, (string Species, string Classification)>
            {
                    {"Dory", ("Blue Tang", "Fish")},
                    {"Nemo", ("Clownfish", "Fish")},
                    {"Flounder", ("Fish", "Fish")},
                    {"Marlin", ("Clownfish", "Fish")},
                    {"Sebastian", ("Red Jamaican Crab", "Invertebrate")},
                    {"Bambi", ("Deer", "Mammal")},
                    {"Thumper", ("Rabbit", "Mammal")},
                    {"Flower", ("Skunk", "Mammal")},
                    {"Gus Gus", ("Mouse", "Mammal")},
                    {"Jaq", ("Mouse", "Mammal")},
                    {"Mickey Mouse", ("Mouse", "Mammal")},
                    {"Minnie Mouse", ("Mouse", "Mammal")},
                    {"Goofy", ("Dog", "Mammal")},
                    {"Pluto", ("Dog", "Mammal")},
                    {"Copper", ("Hound Dog", "Mammal")},
                    {"Todd", ("Fox", "Mammal")},
                    {"Robin Hood", ("Fox", "Mammal")},
                    {"Little John", ("Bear", "Mammal")},
                    {"Ben", ("Rat", "Mammal")},
                    {"Wilbur", ("Pig", "Mammal")},
                    {"Charlotte", ("Spider", "Invertebrate")},
                    {"Bagheera", ("Panther", "Mammal")},
                    {"Kaa", ("Python", "Reptile")},
                    {"Tiana", ("Frog", "Amphibian")},
                    {"Louis", ("Alligator", "Reptile")},
                    {"Pascal", ("Chameleon", "Reptile")},
                    {"Maximus", ("Horse", "Mammal")},
                    {"Sven", ("Reindeer", "Mammal")},
                    {"Flit", ("Hummingbird", "Bird")},
                    {"Donald Duck", ("Duck", "Bird")},
                    {"Scrooge McDuck", ("Duck", "Bird")},
                    {"Robin", ("Bird", "Bird")},
                    {"Lenny", ("Whale", "Fish")},
                    {"Gordon", ("Crab", "Invertebrate")},
                    {"Crush", ("Sea Turtle", "Reptile")},
                    {"Zazu", ("Hornbill", "Bird")},
                    {"Timon", ("Meerkat", "Mammal")},
                    {"Pumbaa", ("Warthog", "Mammal")},
                    {"Lumière", ("Candle", "Other")},
                    {"Madame Mim", ("Snake", "Reptile")},
                    {"Frou-Frou", ("Bird", "Bird")},
                    {"Gaston", ("Boar", "Mammal")},
                    {"Coco", ("Dog", "Mammal")},
                    {"Sally", ("Spider", "Invertebrate")},
                    {"Moo", ("Cow", "Mammal")},
                    {"Dumbo", ("Elephant", "Mammal")},
                    {"Tramp", ("Dog", "Mammal")},
                    {"Lady", ("Dog", "Mammal")},
                    {"Jock", ("Dog", "Mammal")},
                    {"Scrooge", ("Duck", "Bird")},
                    {"Huey", ("Duck", "Bird")},
                    {"Dewey", ("Duck", "Bird")},
                    {"Louie", ("Duck", "Bird")},
                    {"Perdita", ("Dog", "Mammal")},
                    {"Pongo", ("Dog", "Mammal")},
                    {"Luna", ("Polar Bear", "Mammal")},
                    {"Pip", ("Squirrel", "Mammal")},
                    {"Cody", ("Golden Retriever", "Mammal")},
                    {"Bernadette", ("Poodle", "Mammal")},
                    {"Princess", ("Cat", "Mammal")},
                    {"Shenzi", ("Hyena", "Mammal")},
                    {"Banzai", ("Hyena", "Mammal")},
                    {"Ed", ("Hyena", "Mammal")},
                    {"Lizard", ("Lizard", "Reptile")},
                    {"Bolt", ("Dog", "Mammal")},
                    {"Slinky Dog", ("Dog", "Mammal")},
                    {"Mr. Pricklepants", ("Hedgehog", "Mammal")}
                };
        private static readonly string[] gender = ["Male", "Female", "Other"];

        public static void SeedAnimals(IServiceProvider serviceProvider)
        {
            using (var zoodbcontext = new ZooDbContext(serviceProvider.GetRequiredService<DbContextOptions<ZooDbContext>>()))
            {
                foreach (var animal in zooanimals)
                {
                    string name = animal.Key;
                    string sex = gender[Random.Shared.Next(gender.Length)];
                    DateOnly dateOfBirth = GetRandomDate();
                    DateOnly dateAcquired = dateOfBirth.AddYears(Random.Shared.Next(1, 5));
                    string species = animal.Value.Species;
                    string classification = animal.Value.Classification;
                    Console.WriteLine(name);

                    zoodbcontext.Animals.Add(new Animal{Name=name, Sex=sex, DateOfBirth=dateOfBirth, DateAcquired=dateAcquired, Species=species, Classification=classification});
                    zoodbcontext.SaveChanges();
                }
                Console.WriteLine("test");
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