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
        public bool HasCapacity 
        { 
            get { return Animals?.Count < MaxCapacity; }
        }
        public ICollection<Animal>? Animals = new List<Animal>();
        public void AddAnimalToEnclosure(Animal animal)
        {
            if (!HasCapacity)
            {
                throw new Exception("Enclosure at maximum capacity.");
            }
            Animals?.Add(animal);
        }


        public override string ToString()
        {
            return $"{Name} {MaxCapacity}";
        }
    }

}