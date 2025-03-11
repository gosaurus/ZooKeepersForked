using System.ComponentModel.DataAnnotations;
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

        public required DateOnly DateAcquired {get; set;}

        public required string Species {get; set;}

        public required string Classification {get; set;}

    }

}
