using MusicReview.Auth;

namespace MusicReview.Domain.UserServices;

public interface IHttpUserService
{
    public User GetCurrentUser();

    public string GetCurrentUserId();
}