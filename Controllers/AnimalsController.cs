using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SQLitePCL;
using ZooKeepers.Data;
using ZooKeepers.Models;
using ZooKeepers.Constants;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> Get()
    {
        _logger.LogInformation("Fetching all animals.");
        return await _context.Animals.ToListAsync();
    }
 
    [HttpGet("{animalid}")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal(int animalid)
    {
         _logger.LogInformation($"Fetching animals by id: {animalid}.");
         var animal = await _context.Animals.FirstOrDefaultAsync(animal=> animalid == animal.AnimalId);
         return animal==null? NotFound():Ok(animal);
    }

    [HttpGet("search2")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetPaginatedResults
    (
        [FromQuery] SearchParameters.SearchParameters searchParameters
    )
    {
        Console.WriteLine(searchParameters);

        IQueryable<Animal> searchQuery = _context.Animals
            .Where(animal => 
                (animal.Name.Contains(searchParameters.AnimalName) || searchParameters.AnimalName == null) &&
                (animal.Species == searchParameters.Species || searchParameters.Species == null) &&  
                (animal.Classification == searchParameters.Classification || searchParameters.Classification == null) &&
                (animal.DateAcquired == searchParameters.DateAcquired || searchParameters.DateAcquired == null) &&
                (searchParameters.Age == null ||
                    (DateTime.Now.Year - animal.DateOfBirth.Year == searchParameters.Age && //current year - dob year == search params
                    (animal.DateOfBirth.Month < DateTime.Now.Month ||  // AND dob month has passed
                    animal.DateOfBirth.Month == DateTime.Now.Month && animal.DateOfBirth.Day <= DateTime.Now.Day)) // if current month, compare dob day;
                ));

        
        string filter = searchParameters.GetFilter(searchParameters); //ensure not null

        Console.WriteLine($"Filter = {filter}");

        // if (searchParameters.OrderByDescending == "True")
        //     searchQuery.OrderByDescending(filterLambdaExpression);
        // else 
        //     searchQuery.OrderBy(filterLambdaExpression);

        var searchQueryCount = await searchQuery.CountAsync();
        
        var animalsList = await searchQuery
            .OrderBy(GetFilterLambdaExpression(filter))
            .Skip((searchParameters.PageNumber - 1) * searchParameters.PageSize)
            .Take(searchParameters.PageSize)
            .ToListAsync();

        var response = new 
        {
                TotalItems = searchQueryCount,
                searchParameters.PageSize,
                searchParameters.PageNumber,
                Animals = animalsList
        };
        return Ok( response);
    }

    // private static async Task<List<Animal>> OrderByDescending
    // (
    //     IQueryable<Animal> searchQuery,
    //     Expression<Func<Animal, object>> FilterLambdaExpression
    // )
    // {
    //     return await searchQuery.OrderByDescending(FilterLambdaExpression).ToListAsync();
    // }

    private static Expression<Func<Animal, object>> GetFilterLambdaExpression(string filter)
    {
        
        switch(filter.ToLower())
        {
            case "name":
            return animal => animal.Name;

            case "classification":
            Console.WriteLine($"Returning {filter} as lambda");
            return animal => animal.Classification;
            
            case "dateacquired":
            return animal  => animal.DateAcquired;

            case "age":
            return animal => animal.DateOfBirth;

            case "species":
            Console.WriteLine($"Returning {filter} as lambda");
            return animal => animal.Species;
            
            default:
            return animal => animal.Species;
        } 
    }

    [HttpGet("search")]
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
            animalsQuery = animalsQuery.Where(animal=> 
            (DateTime.UtcNow.Year-animal.DateOfBirth.Year)-
            ((DateTime.UtcNow.Month<animal.DateOfBirth.Month)||
             (DateTime.UtcNow.Month==animal.DateOfBirth.Month&&DateTime.UtcNow.Day<animal.DateOfBirth.Day)? 1:0)
            == ageQuery);
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
