using AncosBarber.DTOs;
using AncosBarber.DTOs.BarberShopDto;
using AncosBarber.Repositories.UseCasesRepositories.BarberShopRepository;
using Microsoft.AspNetCore.Mvc;

namespace AncosBarber.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BarberShopsController : ControllerBase
{
    private readonly IBarberShopRepository _repository;

    public BarberShopsController(IBarberShopRepository repository, ILogger<BarberShopsController> logger)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<BarberShopDto>> CreateBarberShop(BarberShopDto barberShopDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Dados inválidos.");
        }

        var barberShopCreated = await _repository.CreateBarberShop(barberShopDto);

        return Created($"Barbearia {barberShopCreated.Name} criada.", barberShopCreated);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BarberShopDto>>> GetAllBarberShops()
    {
        var barberShops = await _repository.GetAllBarberShops();
        if (barberShops.Count() >= 1)
        {
            return Ok(barberShops);
        }

        return NotFound("Nenhuma barbearia encontrada.");
    }


    [HttpGet("GetBarberShopByName")]
    public async Task<ActionResult<BarberShopDto>> GetBarberShopByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Nome da barbearia não pode ser vazio.");
        }

        var barberShop = await _repository.GetBarberShopByName(name);
        return Ok(barberShop);
    }

    [HttpGet("GetBarberShopById")]
    public async Task<ActionResult<BarberShopDto>> GetBarberShopById(Guid id)
    {
        var barberShop = await _repository.GetBarberShopById(id);

        return Ok(barberShop);
    }
    

    [HttpPut("{id}")]
    public async Task<ActionResult<BarberShopDto>> UpdateBarberShop(Guid id, BarberShopDto barberShopDto)
    {
        if (!ModelState.IsValid || id != barberShopDto.BarberShopId) return BadRequest("Dados inválidos.");

        var barberShopUpdated = await _repository.UpdateBarberShop(barberShopDto);

        if (barberShopUpdated == false) return BadRequest("Error inesperado ao atualizar barbearia.");

        return Ok($"Barbearia {barberShopDto.Name} atualizada com sucesso.");
    }

    [HttpDelete]
    public async Task<ActionResult<BarberShopDto>> DeleteBarberShop(string name)
    {
        var barberShopDeleted = await _repository.DeleteBarberShop(name);

        return Ok($"Barbearia {barberShopDeleted.Name} deletada com sucesso.");
    }
}