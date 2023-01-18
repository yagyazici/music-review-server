using Microsoft.AspNetCore.Builder;
using MusicReview.Infrastructure.SignalR.Hubs;

namespace Pharma.Infrastructure.SignalR;

public static class HubRegistration
{
    public static void MapHubs(this WebApplication webApplication){
        webApplication.MapHub<MusicReviewHub>("/music-reviews-hub");
        webApplication.MapHub<UserHub>("/user-hub");
    }
}