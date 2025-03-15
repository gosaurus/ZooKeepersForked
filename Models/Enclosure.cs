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
            if (Animals?.Count == MaxCapacity)
            {
                throw new Exception("Enclosure at maximum capacity.");
            }
            Animals?.Add(animal);
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