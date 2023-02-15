using MusicReview.Auth;

namespace MusicReview.Domain.UserServices;

public interface IHttpUserService
{
    public string GetCurrentUserId();
}