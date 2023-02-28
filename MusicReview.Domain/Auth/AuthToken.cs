namespace MusicReview.Domain.Auth;

public class AuthToken
{
    public AuthToken(string token, DateTime expires)
    {
        Token = token;
        Expires = expires;
    }

    public string Token { get; set; }
    public DateTime Expires { get; set; } 
}