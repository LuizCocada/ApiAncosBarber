using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AncosBarber.Providers.TokenProvider;

public interface ITokenProvider
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);  
      
    string GenerateRefreshToken();  
      
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);  
    
    
}