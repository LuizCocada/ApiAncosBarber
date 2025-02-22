using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AncosBarber.Models;

public class Bookings
{
    [Key] 
    public Guid BookingId { get; set; }

    [ForeignKey("BarberShop")]
    public Guid BarberShopId { get; set; }

    [ForeignKey("Services")]
    public Guid ServicesId { get; set; }
    
    [ForeignKey("AspNetUsers")]
    public Guid UserId { get; set; } // IsCostumer = true
    
    public DateTime BookingDate { get; set; }
}