using System.IdentityModel.Tokens.Jwt;  
using System.Security.Claims;  
using System.Security.Cryptography;  
using System.Text;  
using Microsoft.IdentityModel.Tokens;  

namespace AncosBarber.Providers.TokenProvider;

public class TokenProvider : ITokenProvider
{
    private readonly IConfiguration _config;  
  
    public TokenProvider(IConfiguration config)  
    {  
        _config = config;  
    }  
  
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)  
    {  
        var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??  
                  throw new InvalidOperationException("invalid Secret Key");  
        var privateKey = Encoding.UTF8.GetBytes(key);  
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);  
  
        var tokenDescription = new SecurityTokenDescriptor  
        {  
            Subject = new ClaimsIdentity(claims),  
            Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT")  
                .GetValue<double>("TokenValidInMinutes")),  
            Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),  
            Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),  
            SigningCredentials = signingCredentials  
        };  
  
        var tokenHandler = new JwtSecurityTokenHandler();  
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescription);  
  
        return token;  
    }  
  
  
    public string GenerateRefreshToken()  
    {  
        var secureRandomBytes = new byte[128];  
        using var randomNumberGenerator = RandomNumberGenerator.Create();  
        randomNumberGenerator.GetBytes(secureRandomBytes);  
  
        var refreshToken = Convert.ToBase64String(secureRandomBytes);  
        return refreshToken;  
    }  
  
  
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)  
    {  
        var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid secret key");  
  
        var tokenValidationParameters = new TokenValidationParameters  
        {  
            ValidateAudience = false,  
            ValidateIssuer = false,  
            ValidateIssuerSigningKey = true,  
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),  
            ValidateLifetime = false  
        };  
  
        var tokenHandler = new JwtSecurityTokenHandler();  
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);  
  
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||  
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,  
                StringComparison.InvariantCultureIgnoreCase))  
        {  
            throw new SecurityTokenException("Invalid token");  
        }  
          
        return principal;  
    }  
}