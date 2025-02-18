using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AncosBarber.Models;

public class ApplicationUser : IdentityUser
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}