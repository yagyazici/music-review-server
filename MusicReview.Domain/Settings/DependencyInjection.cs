using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MusicReview.Domain.Settings;

public static class DependencyInjection
{
    public static void AddSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MusicReviewsDB"));
        services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
        services.Configure<SpotifySettings>(builder.Configuration.GetSection("SpotifySettings"));
    }
}