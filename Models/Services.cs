using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncosBarber.Models;

public class Services
{
    [Key]
    public Guid ServicesId { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    
    [ForeignKey("BarberShop")]
    public Guid BarberShopId { get; set; }
    
    public ICollection<Bookings>? Bookings { get; set; }

    public Services()
    {
        Bookings = new List<Bookings>();
    }
}