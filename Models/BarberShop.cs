using System.ComponentModel.DataAnnotations;

namespace AncosBarber.Models;

public class BarberShop
{
    [Key]
    public Guid BarberShopId { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public bool IsOpen { get; set; }
    
    public DateTime RegistrationDate { get; set; }

    public ICollection<Services>? Services { get; set; }
    
    public ICollection<Bookings>? Bookings { get; set; }
    
    public BarberShop()
    {
        Services = new List<Services>();
        Bookings = new List<Bookings>();
    }
}

