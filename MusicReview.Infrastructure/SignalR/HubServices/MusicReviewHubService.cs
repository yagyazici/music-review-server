using Microsoft.AspNetCore.SignalR;
using MusicReview.Domain.Models;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Infrastructure.SignalR.Hubs;

namespace MusicReview.Infrastructure.SignalR.HubServices;

public class MusicReviewHubService : IMusicReviewHubService
{
    readonly IHubContext<MusicReviewHub> _hubContext;

    public MusicReviewHubService(IHubContext<MusicReviewHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task MusicReviewAddedMessageAsync(Review musicReview)
    {
        await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.MusicReviewAddedMessage, musicReview);
    }

    public async Task MusicReviewDeletedMessageAsync(string id)
    {
        await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.MusicReviewDeletedMessage, id);
    }

    public async Task MusicReviewUpdateMessageAsync(string id)
    {
        await _hubContext.Clients.All.SendAsync(RecieveFunctionNames.MusicReviewUpdatedMessage, id);
    }
}