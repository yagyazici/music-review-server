using Microsoft.Extensions.DependencyInjection;
using MusicReview.Domain.NotificationServices;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Domain.UserServices;
using MusicReview.Infrastructure.Services;
using MusicReview.Infrastructure.Services.NotificationServices;
using MusicReview.Infrastructure.Services.UserServices;
using MusicReview.Infrastructure.SignalR.HubServices;

namespace MusicReview.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IHttpUserService, HttpUserService>();
        services.AddSingleton(typeof(IGenericMongoRepository<>), typeof(GenericMongoRepository<>));
        services.AddScoped<INotificationServices, NotificationServices>();
    }

    public static void AddSignalRServices(this IServiceCollection services)
    {
        services.AddTransient<IMusicReviewHubService, MusicReviewHubService>();
        services.AddTransient<IUserHubService, UserHubService>();
        services.AddSignalR();
    }
}