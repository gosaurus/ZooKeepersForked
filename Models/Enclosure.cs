using System.ComponentModel.DataAnnotations;

namespace ZooKeepers.Models
{
    public class Enclosure
    {
        [Key]
        public int EnclosureId { get; set; }
        public required string Name { get; set; }
        public int MaxCapacity { get; set; }
        public ICollection<Animal>? Animals = new List<Animal>();
        public bool HasCapacity()
        {
            return Animals?.Count < MaxCapacity;
        }

        public void AddAnimal(Animal animalToAdd)
        {
            if (this.HasCapacity())
            {
                Animals?.Add(animalToAdd);
            }
        }
        public override string ToString()
        {
            return $"{Name} {MaxCapacity}";
        }
    }

}