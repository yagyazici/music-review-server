using AutoMapper;
using MongoDB.Bson;
using MusicReview.Applications.Applications;
using MusicReview.Auth;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Models.Responses;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.DTOs;

public class MusicAuthService : IMusicAuthService
{
    private readonly AuthApplications _authApplications;
    private readonly IGenericMongoRepository<User> _mongoRepository;
    private readonly IMapper _mapper;
    private readonly IUserHubService _userHubService;

    public MusicAuthService(AuthApplications authApplications,
        IGenericMongoRepository<User> mongoRepository,
        IMapper mapper,
        IUserHubService userHubService)
    {
        _authApplications = authApplications;
        _mongoRepository = mongoRepository;
        _mapper = mapper;
        _userHubService = userHubService;
    }

    public async Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums)
    {
        var user = await _authApplications.GetCurrentUser();
        user.FavoriteAlbums = favoriteAlbums;
        await _mongoRepository.UpdateAsync(user);
        await _userHubService.UserFavoriteAlbumsUpdatedMessageAsync(user.Id);
        return new Success(true, "Album(s) appended");
    }

    public async Task<Response> ToggleUserFollowedUser(UserProfileDTO followedUser)
    {
        var currentUser = await _authApplications.GetCurrentUser();
        var currentUserDto = _mapper.Map<UserProfileDTO>(currentUser);
        var otherUser = await _mongoRepository.GetByIdAsync(followedUser.Id);

        var check = currentUser.Followings.Where(user => user.Id == followedUser.Id).Any();
        if (check)
        {
            currentUser.Followings.Remove(followedUser);
            otherUser.Followers.Remove(currentUserDto);
            await _mongoRepository.UpdateAsync(currentUser);
            await _mongoRepository.UpdateAsync(otherUser);
            return new Success(true, "unfollowed");
        }

        currentUser.Followings.Append(followedUser);
        otherUser.Followers.Append(currentUserDto);
        await _mongoRepository.UpdateAsync(currentUser);
        await _mongoRepository.UpdateAsync(otherUser);

        var currentUserId = currentUser.Id;
        var followedUserId = followedUser.Id;
        var userIds = new {
            currentUserId,
            followedUserId
        };

        await _userHubService.UserFollowedUserMessageAsync(userIds);
        await _userHubService.UserSendNotificitionMessageAsync(followedUser.Id);

        // TODO
        await SendNotification(followedUser.Id, currentUserDto, "follower");
        await _userHubService.UserSendNotificitionMessageAsync(followedUser.Id);

        return new Success(true, "followed");
    }

    public async Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum)
    {
        var user = await _authApplications.GetCurrentUser();
        var check = user.LikedAlbums.Where(album => album.id == likedAlbum.id).Any();
        if (check) user.FavoriteAlbums.Remove(likedAlbum); 
        else user.FavoriteAlbums.Add(likedAlbum);
        await _mongoRepository.UpdateAsync(user);
        return new Success(true, check ? "removed" : "liked");
    }

    private async Task<string> SendNotification(string toUserId, UserProfileDTO fromUser, string notifType)
    {
        var toUser = await _mongoRepository.GetByIdAsync(toUserId);
        var notification = new Notification {
            // Id = ObjectId.GenerateNewId().ToString(),
            NotificationType = notifType,
            NotificationSeen = false,
            FromUser = fromUser,
            NotificationDate = DateTime.Now
        };
        toUser.Notifications.Append(notification);
        await _mongoRepository.UpdateAsync(toUser);
        return "Notification sended";
    }
}