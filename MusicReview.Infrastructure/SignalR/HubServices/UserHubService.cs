using Microsoft.AspNetCore.SignalR;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Infrastructure.SignalR.Hubs;

namespace MusicReview.Infrastructure.SignalR.HubServices;

public class UserHubService : IUserHubService
{
    readonly IHubContext<UserHub> _hubContext;
    public UserHubService(IHubContext<UserHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task UserFavoriteAlbumsUpdatedMessageAsync(string id)
    {
        await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.UserFavoriteAlbumsUpdatedMessage, id);
    }

    public async Task UserFollowedUserMessageAsync(object userIds){
        await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.UserFollowedUserMessage, userIds);
    }

    public async Task UserSendNotificitionMessageAsync(string toUserId){
        await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.UserSendNotificitionMessage, toUserId);
    }
}