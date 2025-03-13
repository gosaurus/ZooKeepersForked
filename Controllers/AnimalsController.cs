using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SQLitePCL;
using ZooKeepers.Data;
using ZooKeepers.Models;
using ZooKeepers.Utils;
using System.Globalization;



namespace ZooKeepers.Controllers;

[ApiController]
[Route("[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly ILogger<AnimalsController> _logger;
    private readonly ZooDbContext _context;

    public AnimalsController(ZooDbContext context, ILogger<AnimalsController> logger)
    {
        _logger = logger;
        _context = context;
        
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Animal>>> Get()
    // {
    //      _logger.LogInformation("Fetching all animals.");
    //      return await _context.Animals.ToListAsync();
    // }
 
    [HttpGet("{animalid}")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal(int animalid)
    {
         _logger.LogInformation($"Fetching animals by id: {animalid}.");
         var animal = await _context.Animals.FirstOrDefaultAsync(animal=> animalid == animal.AnimalId);
         return animal==null? NotFound():Ok(animal);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> GetPaginatedAnimals(
        [FromQuery] string searchQuery ="",
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? ageQuery= null,
        [FromQuery] string orderBy = "species")
    {
         _logger.LogInformation($"Fetching animals with pageNumber:{pageNumber},pagesize:{pageSize},searchquery:{searchQuery},orderby:{orderBy}");
         
         IQueryable<Animal> animalsQuery = _context.Animals;
         if(!string.IsNullOrEmpty(searchQuery))
         {
            animalsQuery=animalsQuery.Where(animal=>
            animal.Species.Contains(searchQuery)||
            animal.Classification.Contains(searchQuery)||
            animal.Name.Contains(searchQuery)||
            animal.DateAcquired==DateOnly.Parse(searchQuery));
         }

         if(ageQuery.HasValue)
         {
            animalsQuery = animalsQuery.Where(animal=> Age.CalculateAge(animal) == ageQuery);
         }

         switch(orderBy.ToLower())
         {
            case "name":
            animalsQuery = animalsQuery.OrderBy(animal=>animal.Name);
            break;

            case "classification":
            animalsQuery = animalsQuery.OrderBy(animal=>animal.Classification);
            break;
            
            case "dateacquired":
            animalsQuery = animalsQuery.OrderBy(animal=>animal.DateAcquired);
            break;

            case "dateofbirth":
            animalsQuery = animalsQuery.OrderBy(animal=>animal.DateOfBirth);
            break;

            default:
            animalsQuery = animalsQuery.OrderBy(animal=>animal.Species);
            break;
         }

         var searchedAnimals= await animalsQuery.CountAsync();
         var animals= await animalsQuery.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();

         var response = new 
         {
            TotalItems = searchedAnimals,
            PageSize = pageSize,
            PageNumber = pageNumber,
            Animals = animals
         };

         return Ok(response);
    }

    [HttpPost]

    public ActionResult Post(Animal animal)
    {
        _logger.LogInformation($"Adding Animal {animal.Name}");
        _context.Animals.Add(animal);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAnimal),new{animalid=animal.AnimalId},animal);
    }

}
