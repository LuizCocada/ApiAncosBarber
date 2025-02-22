using System.ComponentModel.DataAnnotations;

namespace AncosBarber.DTOs.BarberShopDto;

public class BarberShopDto
{
    public Guid BarberShopId { get; set; }
    
    [Required]
    [MaxLength(40)]
    public string? Name { get; set; } 
    
    [Required]
    [MaxLength(150)]
    public string? Address { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [MaxLength(200)]
    public string? ImageUrl { get; set; }
}