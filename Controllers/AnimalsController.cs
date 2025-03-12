using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SQLitePCL;
using ZooKeepers.Data;
using ZooKeepers.Models;

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

    [HttpPost]

    public ActionResult Post(Animal animal)
    {
        _logger.LogInformation($"Adding Animal {animal.Name}");
        _context.Animals.Add(animal);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAnimal),new{animalid=animal.AnimalId},animal);
    }

}
