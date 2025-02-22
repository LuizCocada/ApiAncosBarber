using System.ComponentModel.DataAnnotations;

namespace AncosBarber.Models;

public class BarberShop
{
    [Key]
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
    
    public bool IsOpen { get; set; }
    
    public DateTime RegistrationDate { get; set; }

    public ICollection<Services>? Services { get; set; }
    
    public ICollection<Bookings>? Bookings { get; set; }
    
    public ICollection<ApplicationUser>? Employees { get; set; } // IsCostumer = false
    
    public BarberShop()
    {
        Services = new List<Services>();
        Bookings = new List<Bookings>();
        Employees = new List<ApplicationUser>();
    }
}

