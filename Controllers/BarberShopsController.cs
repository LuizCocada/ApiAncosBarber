using AncosBarber.context;
using AncosBarber.Models;
using Microsoft.AspNetCore.Mvc;

namespace AncosBarber.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BarberShopsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BarberShopsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] BarberShop barberShop)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
         _context.BarberShops.Add(barberShop);
        _context.SaveChanges();

        return Ok("Barbearia Criada.");
    }
}