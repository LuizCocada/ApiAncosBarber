using AncosBarber.DTOs;
using AncosBarber.DTOs.ServiceDto;
using AncosBarber.Repositories.UseCasesRepositories.ServicesRepositories;
using Microsoft.AspNetCore.Mvc;

namespace AncosBarber.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServicesRepository _repository;

    public ServicesController(IServicesRepository repository)
    {
        _repository = repository;
    }

    [HttpPost] //att
    public async Task<ActionResult<ServicesDto>> CreateServices(ServicesDto servicesDto)
    {
        if (!ModelState.IsValid) return BadRequest("Dados inválidos.");

        var serviceCreated = await _repository.CreateService(servicesDto);

        return new CreatedAtRouteResult("GetService", new { id = serviceCreated.ServicesId }, serviceCreated);
    }

    
    [HttpGet("GetServiceById/{id}", Name = "GetService")]
    public async Task<ActionResult<ServicesDto>> GetServiceById(Guid id)
    {
        var service = await _repository.GetServiceById(id);

        return Ok(service);
    }


    [HttpGet("GetServicesByBarberShopName")]
    public async Task<ActionResult<IEnumerable<ServicesDto>>> GetServicesByBarberShopName(string name)
    {
        var services = await _repository.GetServicesByBarberShopName(name);

        if (services.Count() <= 0) return NotFound("Nenhum serviço encontrado.");

        return Ok(services);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServicesDto>> UpdateService(Guid id, ServicesDto servicesDto)
    {
        if (!ModelState.IsValid || id != servicesDto.ServicesId) return BadRequest("Dados inválidos.");
        
        var serviceUpdated = await _repository.UpdateService(servicesDto);

        return Ok(serviceUpdated);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServicesDto>> DeleteService(Guid id)
    {
        var service = await _repository.GetServiceById(id);

        return Ok($"Serviço {service.Name} deletado com sucesso.");
    }
}