using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ZooKeepers.Models
{

    public class Animal
    {
        [Key]
        public int AnimalId {get; set;}
        public required string Name {get; set;}
        public required string Sex {get; set;}
        public required DateOnly DateOfBirth {get; set;}
        public DateOnly DateAcquired { get;set; } 
        public required string Species {get; set;}
        public required string Classification {get; set;}
        public int? EnclosureId { get; set; }
        public Enclosure? Enclosure { get; set; }
        public override string ToString()
        {
            return $"{AnimalId} Name: {Name} - Enclosure: {Enclosure}";
        }

        public Animal () {}

        public Animal 
        (
            string name,
            string sex,
            DateOnly dateofbirth,
            DateOnly dateacquired,
            string species,
            string classification,
            int? enclosureid = null
        )
        { 
            Name = name;
            Sex = sex;
            DateOfBirth = dateofbirth;
            DateAcquired = dateacquired;
            Species = species;
            Classification = classification;
            EnclosureId = enclosureid;
        }
    }
}
