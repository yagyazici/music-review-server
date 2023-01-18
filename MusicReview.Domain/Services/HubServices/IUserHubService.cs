namespace MusicReview.Domain.Services.HubServices;
public interface IUserHubService
{
    Task UserFavoriteAlbumsUpdatedMessageAsync(string id);
    Task UserFollowedUserMessageAsync(object userIds);
    Task UserSendNotificitionMessageAsync(string toUserId);
}