using Microsoft.AspNetCore.Http;
using MusicReview.Auth;
using MusicReview.Domain.UserServices;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MusicReview.Infrastructure.Services.UserServices;

public class HttpUserService: IHttpUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HttpUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public User GetCurrentUser(){
        var result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData);
        }
        return JsonConvert.DeserializeObject<User>(result);
    }

    public string GetCurrentUserId()
    {
        var result = string.Empty;
        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData);
        }
        return JsonConvert.DeserializeObject<User>(result).Id;
    }
}

