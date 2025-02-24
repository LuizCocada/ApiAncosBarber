using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AncosBarber.DTOs.Authentication.Login;
using AncosBarber.DTOs.Authentication.Register;
using AncosBarber.DTOs.Authentication.Token;
using AncosBarber.Models;
using AncosBarber.Providers.TokenProvider;
using Microsoft.AspNetCore.Identity;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AncosBarber.Repositories.UseCasesRepositories.AuthRepository;

public class AuthRepository : IAuthRepository
{
    private readonly IConfiguration _config;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<AuthRepository> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthRepository(IConfiguration config, ITokenProvider tokenProvider, ILogger<AuthRepository> logger, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _config = config;
        _tokenProvider = tokenProvider;
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<LoginResponse> LoginGenerateAccess(LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email!);
        
        if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenProvider.GenerateAccessToken(authClaims, _config);
            string refreshToken = _tokenProvider.GenerateRefreshToken();

            _ = int.TryParse(_config["JWT:RefreshTokenValidInMinutes"], out int refreshTokenValidInMinutes);

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidInMinutes);
            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }

        throw new UnauthorizedAccessException();
    }

    public async Task<RegisterResponse> RegisterUser(RegisterModel registerModel)
    {
        var userExist = await _userManager.FindByEmailAsync(registerModel.Email!);
        if (userExist != null)
            return new RegisterResponse { Success = false, Message = "Email já cadastrado." };

        ApplicationUser user = new()
        {
            UserName = registerModel.Username,
            Email = registerModel.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password!);
        
        if (result.Succeeded) return new RegisterResponse{Success = true , Message = $"Usuário {registerModel.Username} cadastrado com sucesso."};

        return new RegisterResponse{Success = false, Message = "Erro ao cadastrar usuário."};
    }

    public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
    {
        var claimsPrincipal = _tokenProvider.GetPrincipalFromExpiredToken(tokenModel.AccessToken!, _config);

        string? email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if(string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
        
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new UnauthorizedAccessException("Invalid Operation refresh token");
        }

        var newAccessToken = _tokenProvider.GenerateAccessToken(claimsPrincipal.Claims.ToList(), _config);
        string newRefreshToken = _tokenProvider.GenerateRefreshToken();
        
        
        //testar depois atualizar o tempo de expiração do refreshToken
        
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        
        return new TokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken
        };
    }
    
    public async Task<RevokeResponse> RevokeToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return new RevokeResponse{Success = false, Message = "Usuário não encontrado."};
        
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        
        return new RevokeResponse{Success = true, Message = "Token revogado com sucesso."};
        
    }
}