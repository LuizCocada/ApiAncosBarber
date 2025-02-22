using System.ComponentModel.DataAnnotations;

namespace AncosBarber.DTOs.ServiceDto;

public class ServicesDto
{
    public Guid ServicesId { get; set; }
    
    [Required]
    public string? Name { get; set; } 
    
    public string? Description { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public string? ImageUrl { get; set; } 
    
    public Guid BarberShopId { get; set; }
}