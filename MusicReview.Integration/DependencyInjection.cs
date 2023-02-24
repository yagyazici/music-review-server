using Microsoft.Extensions.DependencyInjection;
using MusicReview.Integration.Services;

namespace MusicReview.Integration;


public static class DependencyInjection
{
    public static void AddIntegration(this IServiceCollection services)
    {
        services.AddHttpClient<SpotifyRefreshToken>();
    }
}