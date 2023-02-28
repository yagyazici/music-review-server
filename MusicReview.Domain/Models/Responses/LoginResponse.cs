using MusicReview.Domain.Auth;
using MusicReview.Domain.DTOs;

namespace MusicReview.Domain.Models.Responses;

public class LoginResponse
{
    public LoginResponse(AuthToken authToken, RefreshToken refreshToken, CurrentUserDTO currentUser)
    {
        AuthToken = authToken;
        RefreshToken = refreshToken;
        CurrentUser = currentUser;
    }

    public AuthToken AuthToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
    public CurrentUserDTO CurrentUser { get; set; }
}