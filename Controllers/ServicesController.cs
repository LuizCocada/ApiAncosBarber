using AncosBarber.context;
using AncosBarber.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncosBarber.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ServicesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Post(Services services)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Services.Add(services);
        _context.SaveChanges();

        return Ok("Serviço Criado.");
    }
}