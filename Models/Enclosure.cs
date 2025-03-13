using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace ZooKeepers.Models
{
    public class Enclosure
    {
        [Key]
        public int EnclosureId { get; set; }
        public required string Name { get; set; }
        public int MaxCapacity { get; set; }
        public ICollection<Animal>? Animals = new List<Animal>();
        public void AddAnimalToEnclosure(Animal animal)
        {
            Animals?.Add(animal);
        }

        public int GetEnclosureCapacity(string name)
        {
            switch(name)
            {
                case "Lion":
                    return (int)EnclosureCapacities.Lion;
                case "Aviary":
                    return (int)EnclosureCapacities.Aviary;
                case "Reptile":
                    return (int)EnclosureCapacities.Reptile;
                case "Giraffe":
                    return (int)EnclosureCapacities.Giraffe;
                case "Hippo":
                    return (int)EnclosureCapacities.Hippo;
                default:
                    throw new Exception("Invalid Enclosure. This Enclosure does not exist.");
            }
        }

        enum EnclosureCapacities
        {
            Lion = 10,
            Aviary = 50,
            Reptile = 40,
            Giraffe = 6,
            Hippo = 10,
        }

        public static readonly Dictionary<string, int> enclosuresDict = new Dictionary<string, int> 
        {
            {"Lions' Den", 10},
            {"Aviary", 50},
            {"Reptile", 40},
            {"Giraffe", 6},
            {"Hippo", 10},
        }; 
    }

}