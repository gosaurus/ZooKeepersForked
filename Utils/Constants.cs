namespace ZooKeepers.Constants
{
    public class ReadOnlyProperties 
    {
        public static readonly List<string> ClassificationOptions = new List<string>()
        {
            "Mammal", "Reptile", "Bird", "Insect", "Fish", "Invertebrate"
        };

        public static readonly List<string> SexOptions = new List<string>()
        {
            "Male", "Female", "Other"
        }; 
        public static readonly string[] animalNames = 
        { 
            "Aardvark",
            "Donkey",
            "India tiger",
            "Panther",
            "Leopard",
            "Cheetah",
            "African Elephant",
            "Black bear",
            "Red panda",
            "Nile crocodile",
            "Porcupine",
            "Ostrich",
            "Chimpanzee",
            "Baboon",
            "Gazelle",
            "Hippo",
            "Emu",
            "Sloth",
            "Hummingbird",
            "Python",
            "Toucan",
            "Vulture"
        }; 

        public static readonly Dictionary<string, int> enclosuresDict = new Dictionary<string, int> 
        {
            {"Lions' Den", 10},
            {"Aviary", 50},
            {"Reptile", 40},
            {"Giraffe", 6},
            {"Hippo", 10},
        };
    } 
};