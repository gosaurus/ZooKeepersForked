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
using NLog.LayoutRenderers;
using System.Net;

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

        return await _context.Animals
            .Include(animal => animal.Enclosure)
            .ToListAsync();
    }
 
    [HttpGet("{animalid}")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal(int animalid)
    {
        _logger.LogInformation($"Fetching animals by id: {animalid}.");
        var animal = await _context.Animals
            .Include(animal => animal.Enclosure)
            .FirstOrDefaultAsync(animal => animalid == animal.AnimalId);
        return animal == null ? NotFound() : Ok(animal);
    }
    //queries animal 
    [HttpGet("enclosure/{enclosuresid}")]
    public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosure(int enclosureid)
    {
        
        Console.WriteLine($"Fetching enclosure by id: {enclosureid}.");
        _logger.LogInformation($"Fetching enclosure by id: {enclosureid}.");
        var enclosure = await _context.Enclosures
            .FirstOrDefaultAsync(Enclosure => enclosureid == Enclosure.EnclosureId);
        return enclosure == null ? NotFound() : Ok (enclosure);
    }

    [HttpGet("search2")]
    public async Task<ActionResult<IEnumerable<Animal>>> GetPaginatedResults
    (
        [FromQuery] SearchParameters.SearchParameters searchParameters
    )
    {
        IQueryable<Animal> searchQuery = _context.Animals
            .Include(animal => animal.Enclosure)
            .Where(animal => 
                (animal.Name.Contains(searchParameters.AnimalName) || searchParameters.AnimalName == null) &&
                (animal.Species == searchParameters.Species || searchParameters.Species == null) &&  
                (animal.Classification == searchParameters.Classification || searchParameters.Classification == null) &&
                (animal.DateAcquired == searchParameters.DateAcquired || searchParameters.DateAcquired == null) &&
                (searchParameters.Age == null ||
                    (DateTime.Now.Year - animal.DateOfBirth.Year == searchParameters.Age &&
                    (animal.DateOfBirth.Month < DateTime.Now.Month ||
                    animal.DateOfBirth.Month == DateTime.Now.Month && animal.DateOfBirth.Day <= DateTime.Now.Day))
                ));

        string filter = searchParameters.GetFilter()!;

        if (filter == null)
        {
            throw new ArgumentNullException("Results must be ordered: please provide a matching filter in the search paramters");
        }

        var orderedSearchQuery = OrderDirection(searchQuery, GetFilterLambdaExpression(filter), searchParameters.OrderByDescending);

        var animalsList = await orderedSearchQuery
            .Skip((searchParameters.PageNumber - 1) * searchParameters.PageSize)
            .Take(searchParameters.PageSize)
            .ToListAsync();
       
        var searchQueryCount = await searchQuery.CountAsync();

        var response = new 
        {
                TotalItems = searchQueryCount + " matching animal(s) found",
                searchParameters.PageSize,
                searchParameters.PageNumber,
                Animals = animalsList,
        };

        return Ok(response);
    }

    private static IQueryable<Animal> OrderDirection
    (
        IQueryable<Animal> searchQuery,
        Expression<Func<Animal, object>> filter,
        string orderDirection
    )
    {
        if (orderDirection == "False") 
            return searchQuery.OrderBy(filter);
        else
            return searchQuery.OrderByDescending(filter);
    }

    private static Expression<Func<Animal, object>> GetFilterLambdaExpression(string filter)
    {
        
        switch(filter.ToLower())
        {
            case "name":
            return animal => animal.Name;

            case "classification":
            return animal => animal.Classification;
            
            case "dateacquired":
            return animal  => animal.DateAcquired;

            case "age":
            return animal => animal.DateOfBirth;

            case "species":
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

        var PaginatedResponseDetails = 
            new GenericPaginatedResponse
            {
                TotalItems = searchedAnimals,
                PageSize = pageSize,
                PageNumber = pageNumber,
            };

        var response = new AnimalPaginatedResponse
            {
                Pagination = PaginatedResponseDetails, 
                Animals = animals
            };

         return Ok(response);
    }

    [HttpPost]

    public async Task<ActionResult> Post([FromBody] Animal animal)
    {
        _logger.LogInformation($"Adding Animal {animal.Name}");
        if (!ModelState.IsValid)
        {
            BadRequest(ModelState);
        }

        if (!ReadOnlyProperties.ValidateOptions(animal.Sex, ReadOnlyProperties.SexOptions))
        {
            string message = "Invalid input in 'Sex' field. Please try again.";
            return BadRequest(CreateProblemDetailsObject(HttpStatusCode.BadRequest, message));
        }
        if (!ReadOnlyProperties.ValidateOptions(animal.Classification, ReadOnlyProperties.ClassificationOptions))
        {
            string message = "Invalid input in 'Classification' field. Please try again.";
            return BadRequest(CreateProblemDetailsObject(HttpStatusCode.BadRequest, message));
        }
        if (!ReadOnlyProperties.ValidateOptions(animal.Species, ReadOnlyProperties.animalNames))
        {
            string message = "Invalid input in 'Species' field. Please try again.";
            return BadRequest(CreateProblemDetailsObject(HttpStatusCode.BadRequest, message));
        }
        
        if (animal.EnclosureId != null)
        {    
            var matchingEnclosure = await _context.Enclosures
                .FirstOrDefaultAsync(enclosure => enclosure.EnclosureId == animal.EnclosureId);
            if (matchingEnclosure == null) return NotFound("No matching Enclosure");

            if (!matchingEnclosure.HasCapacity)
            {
                string message = $"Cannot add animal because {matchingEnclosure.Name} at maximum capacity.";
                return BadRequest(CreateProblemDetailsObject(HttpStatusCode.BadRequest, message));
            }
        }

        _context.Animals.Add(animal);
        
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAnimal),new{animalid=animal.AnimalId},animal);
    }

    public static object CreateProblemDetailsObject(HttpStatusCode statuscode, string message)
    {
        return new ProblemDetails 
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Status = (int?)statuscode,
            Detail = message,
        };
    }

}
