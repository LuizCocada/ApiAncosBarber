using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AncosBarber.Models;

public class ApplicationUser : IdentityUser
{
    public bool IsCostumer { get; set; } 
    
    [ForeignKey("BarberShop")]
    public Guid? BarberShopId { get; set; } 
    
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}



// Perguntar se é correto adicionar logica no model
// Exemplo: se IsCostumer for true, BarberShopId deve ser null, e vice-versa


//ROLES: se IsCostumer for true, Role deve ser "Costumer", senão, dono/funcionario