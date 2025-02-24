namespace AncosBarber.DTOs.Authentication.Token;

public class TokenModel
{
    public string? AccessToken { get; set; }
    
    public string? RefreshToken { get; set; }
}