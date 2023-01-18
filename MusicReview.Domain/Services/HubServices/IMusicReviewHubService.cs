using MusicReview.Domain.Models;

namespace MusicReview.Domain.Services.HubServices;

public interface IMusicReviewHubService
{
    Task MusicReviewAddedMessageAsync(Review musicReview);
    Task MusicReviewUpdateMessageAsync(string id);
    Task MusicReviewDeletedMessageAsync(string id);
}