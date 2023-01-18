using MusicReview.Auth;
using MusicReview.Domain.Models.AlbumEntities;
using MusicReview.Domain.Models.Base;
using MusicReview.DTOs;

namespace MusicReview.Domain.Services.ModelServices;

public interface IAuthServices
{
    Task<Response> UpdateUser(string username, string bio, string birthDate, string email);
    Task<Response> UploadProfileImage(string databasePath);
    Task<CurrentUserDTO> GetMeUser();
    Task<UserProfileDTO> GetUserProfile(string userId);
    Task<List<UserProfileDTO>> SearchUserProfile(string username);
    Task<List<Album>> GetCurrentUserFavoriteAlbums();
    Task<bool> CheckUserAlbumLiked(string albumId);
    Task<List<Album>> GetUserLikedAlbums(string userId);
    Task<bool> CheckUserFollowed(string userId);
    Task<Follingers> GetUserFollingers(string userId);
    Task<List<UserProfileDTO>> GetUserFollowings(string userId);
    Task<List<UserProfileDTO>> GetUserFollowers(string userId);
    Task<List<Notification>> GetUserNotifications();
    Task<int> GetUserNotificationCount();
    Task<Response> Register(UserDTO user);
    Task<Response> Login(UserDTO user);
    Task<Response> RefreshToken(string refreshToken, string userId);
}