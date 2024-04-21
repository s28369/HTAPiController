using HtApiController.Animals;
using HtApiController.Services.Animals;
using Microsoft.AspNetCore.Mvc;

namespace HtApiController.Controllers;

[ApiController]
[Route("animals")]
public class AnimalController : ControllerBase
{
    private readonly IAnimalsService _animalsService;

    public AnimalController(IAnimalsService animalsService)
    {
        _animalsService = animalsService;
    }

    [HttpPost]
    public IActionResult CreateAnimal(CreateAnimalRequest request)
    {
        var animal = new Animal(
            request.name,
            request.description,
            request.category,
            request.area

        );
        //Save
        _animalsService.CreateAnimal(animal);
        
        
        var response = new AnimalResponse(
            animal.IdAnimal,
            animal.Name,
            animal.Description,
            animal.Category,
            animal.Area
        );
        
        
        return CreatedAtAction(
            nameof(GetAnimal),
            new {id = animal.IdAnimal},
            
            response
        
        );
    }
    
    [HttpGet("{id}")]
    public IActionResult GetAnimal(int id)
    {
        Animal animal = _animalsService.GetAnimal(id);
        
        var response = new AnimalResponse(
            animal.IdAnimal,
            animal.Name,
            animal.Description,
            animal.Category,
            animal.Area
        );
        
        return Ok(response);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpsertAnimal(int id, UpsertAnimalRequest request)
    {
        var animal = new Animal(
            request.name,
            request.description,
            request.category,
            request.area

        );
        
        _animalsService.UpsertAnimal(id,animal);
        
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        _animalsService.DeleteAnimal(id);
        
        return NoContent();
    }
    
    [HttpGet]
    public IActionResult GetAnimal([FromQuery] string orderBy = "name")
    {
        List<Animal> animals = _animalsService.GetAnimal(orderBy);
        
        
        return Ok(animals);
    }
}