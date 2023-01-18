namespace MusicReview.Infrastructure.SignalR;

public static class RecieveFunctionNames
{
    public const string MusicReviewAddedMessage = "recieveMusicReviewAddedMessage";
    public const string MusicReviewUpdatedMessage = "recieveMusicReviewUpdatedMessage";
    public const string MusicReviewDeletedMessage = "recieveMusicReviewDeletedMessage";

    public const string UserFavoriteAlbumsUpdatedMessage = "receiveUserFavoriteAlbumsUpdatedMessage";
    public const string UserFollowedUserMessage = "receiveUserFollowedUserMessage";
    public const string UserSendNotificitionMessage = "receiveUserSendNotificitionMessage";
}