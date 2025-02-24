using AncosBarber.DTOs.Authentication.Login;
using AncosBarber.DTOs.Authentication.Register;
using AncosBarber.DTOs.Authentication.Token;

namespace AncosBarber.Repositories.UseCasesRepositories.AuthRepository;

public interface IAuthRepository
{
    Task<LoginResponse> LoginGenerateAccess(LoginModel loginModel);
    
    Task<RegisterResponse> RegisterUser(RegisterModel registerModel);
    
    Task<TokenModel> RefreshToken(TokenModel tokenModel);
    
    Task<RevokeResponse> RevokeToken(string email);
}