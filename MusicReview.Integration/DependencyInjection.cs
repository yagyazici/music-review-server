using Microsoft.Extensions.DependencyInjection;
using MusicReview.Domain.SpotifyServices;
using MusicReview.Integration.Services;

namespace MusicReview.Integration;


public static class DependencyInjection
{
    public static void AddIntegration(this IServiceCollection services)
    {
        services.AddHttpClient<SpotifyRefreshToken>(client => 
        {
            client.BaseAddress = new Uri("https://accounts.spotify.com/api/token");
        });
        services.AddHttpClient<ISpotifyClient, SpotifyClient>(client => 
        {
            client.BaseAddress = new Uri("https://api.spotify.com/v1");
        });
    }
}