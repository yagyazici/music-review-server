using Microsoft.Extensions.DependencyInjection;
using MusicReview.Applications.Applications;
using MusicReview.Applications.Services;
using MusicReview.Services.ModelServices;

namespace MusicReview.Applications;

public static class DependencyInjection
{
    public static void AddApplicationsServices(this IServiceCollection services)
    {
        services.AddScoped<AuthApplications>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IMusicAuthService, MusicAuthService>();
        services.AddScoped<IMusicService, MusicService>();
    }
}