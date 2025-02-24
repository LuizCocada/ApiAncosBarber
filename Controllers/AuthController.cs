using AncosBarber.DTOs.Authentication;
using AncosBarber.DTOs.Authentication.Login;
using AncosBarber.DTOs.Authentication.Register;
using AncosBarber.DTOs.Authentication.Token;
using AncosBarber.Repositories.UseCasesRepositories.AuthRepository;
using Microsoft.AspNetCore.Mvc;

namespace AncosBarber.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginModel loginModel)
    {
        if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
            return BadRequest(new { error = "Email e senha são obrigatórios" });

        var loginResponse = await _authRepository.LoginGenerateAccess(loginModel);

        return loginResponse;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterModel registerModel)
    {
        if (string.IsNullOrWhiteSpace(registerModel.Username) || string.IsNullOrWhiteSpace(registerModel.Email) || string.IsNullOrEmpty(registerModel.Password))
            return BadRequest(new { error = "Todos os campos são obrigatórios" });

        var result = await _authRepository.RegisterUser(registerModel);
        if (result.Success == false)
            return StatusCode(StatusCodes.Status500InternalServerError, new StatusResponse { Status = "Error", Message = result.Message });

        return StatusCode(StatusCodes.Status201Created, new StatusResponse { Status = "Success", Message = result.Message });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel tokenModel)
    {
        if (string.IsNullOrWhiteSpace(tokenModel.AccessToken) || string.IsNullOrWhiteSpace(tokenModel.RefreshToken))
            return StatusCode(StatusCodes.Status500InternalServerError,
                new StatusResponse { Status = "Error", Message = "Token de acesso e token de atualização são obrigatórios" });


        var result = await _authRepository.RefreshToken(tokenModel);

        return result;
    }


    [HttpPost("revokeToken")]
    public async Task<ActionResult<RevokeResponse>> RevokeToken(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return StatusCode(StatusCodes.Status500InternalServerError,
                new StatusResponse { Status = "Error", Message = "Token de acesso e token de atualização são obrigatórios" });
        
        var result = await _authRepository.RevokeToken(email);
        return result;
    }
}