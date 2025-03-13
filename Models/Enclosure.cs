using System.ComponentModel.DataAnnotations;

namespace ZooKeepers.Models
{
    public class Enclosure
    {
        [Key]
        public int EnclosureId { get; set; }
        public required string Name { get; set; }
        public required int MaxCapacity { get; set; }
        public ICollection<Animal>? Animals = new List<Animal>();
        public void AddAnimalToEnclosure(Animal animal)
        {
            Animals?.Add(animal);
        }
    }

}