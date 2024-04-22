using APBD_06.Models;
using APBD_06.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_06.Controllers;

[Route("api/animals")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalsController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    public IActionResult GetAnimals(string orderBy)
    {
        if (!_animalService.IsValidOrderBy(orderBy))
        {
            return BadRequest($"Niepoprawna nazwa kolumny. Dozwolone nazwy: {string.Join(", ", _animalService.GetAllowedSortFields())}.");
        }

        var animals = _animalService.GetAnimals(orderBy);
        return Ok(animals);
    }

    
    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        var result = _animalService.AddAnimal(animal);
        return result > 0 ? StatusCode(201) : BadRequest("No rows affected");
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, Animal animal)
    {
        var result = _animalService.UpdateAnimal(idAnimal, animal);
        return result > 0 ? NoContent() : NotFound();
    }

    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        var result = _animalService.DeleteAnimal(idAnimal);
        return result > 0 ? NoContent() : NotFound();
    }
}