using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MusicReview.Domain.Models.Base;
using System.Net.Http.Headers;
using MusicReview.Domain.Models.Responses;
using MusicReview.Domain.DTOs;
using MusicReview.Domain.Models.AlbumEntities;
using MusicReview.Auth;
using MusicReview.Domain.Services.ModelServices;
using MusicReview.Integration.Services;

namespace MusicReview.ApiService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserAuthController : ControllerBase
{
    private readonly IAuthServices _authService;
    private readonly IMusicAuthService _musicAuthService;
    private readonly SpotifyRefreshToken _spotifyRefreshToken;

    public UserAuthController(
        IAuthServices authService,
        IMusicAuthService musicAuthService,
        SpotifyRefreshToken spotifyRefreshToken)
    {
        _authService = authService;
        _musicAuthService = musicAuthService;
        _spotifyRefreshToken = spotifyRefreshToken;
    }

    #region PutRequests
    [HttpPut, Authorize]
    public async Task<Response> UpdateUser(string username, string bio, string birthDate, string email) =>
        await _authService.UpdateUser(username, bio, birthDate, email);

    [HttpPut, DisableRequestSizeLimit, Authorize]
    public async Task<Response> UploadProfileImage()
    {
        try
        {
            var file = Request.Form.Files[0];
            var folderName = Path.Combine("Recourses", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return await _authService.UploadProfileImage(dbPath);
            }
            else
            {
                return new Fail<bool>(false, "error");
            }
        }
        catch (System.Exception)
        {
            return new Fail<bool>(false, "error");
        }
    }

    [HttpPut, Authorize]
    public async Task<Response> UpdatePassword(string currentPassword, string newPassword) =>
        await _authService.UpdatePassword(currentPassword, newPassword);
    #endregion

    #region GetRequests

    [HttpGet, Authorize]
    public async Task<CurrentUserDTO> GetMeUser() => await _authService.GetMeUser();

    [HttpGet]
    public async Task<UserProfileDTO> GetUserProfile(string userId) => await _authService.GetUserProfile(userId);

    [HttpGet]
    public async Task<List<UserProfileDTO>> SearchUserProfile(string username) =>
        await _authService.SearchUserProfile(username);

    [HttpGet, Authorize]
    public async Task<List<Album>> GetCurrentUserFavoriteAlbums() => await _authService.GetCurrentUserFavoriteAlbums();

    [HttpGet, Authorize]
    public async Task<List<Album>> GetUsersFavoriteAlbums(string userId) =>
        await _authService.GetUsersFavoriteAlbums(userId);

    [HttpGet, Authorize]
    public async Task<bool> CheckUserAlbumLiked(string albumId) => await _authService.CheckUserAlbumLiked(albumId);

    [HttpGet]
    public async Task<List<Album>> GetUserLikedAlbums(string userId) => await _authService.GetUserLikedAlbums(userId);

    [HttpGet, Authorize]
    public async Task<bool> CheckUserFollowed(string userId) => await _authService.CheckUserFollowed(userId);

    [HttpGet]
    public async Task<Follingers> GetUserFollingers(string userId) => await _authService.GetUserFollingers(userId);

    [HttpGet]
    public async Task<List<UserProfileDTO>> GetUserFollowings(string userId) => await _authService.GetUserFollowings(userId);

    [HttpGet]
    public async Task<List<UserProfileDTO>> GetUserFollowers(string userId) => await _authService.GetUserFollowers(userId);

    [HttpGet, Authorize]
    public async Task<List<Notification>> GetUserNotifications() => await _authService.GetUserNotifications();

    [HttpGet]
    public async Task<int> GetUserNotificationCount() => await _authService.GetUserNotificationCount();
    #endregion

    #region PostRequests
    [HttpPost]
    public async Task<Response> Register(UserDTO request) => await _authService.Register(request);

    [HttpPost]
    public async Task<Response> Login(UserDTO request) => await _authService.Login(request);

    [HttpPost]
    public async Task<Response> RefreshToken(string refreshToken, string userId) =>
        await _authService.RefreshToken(refreshToken, userId);

    [HttpPost, Authorize]
    public async Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums) =>
        await _musicAuthService.AddUserFavoriteAlbums(favoriteAlbums);

    [HttpPost, Authorize]
    public async Task<Response> ToggleUserFollowedUser(UserProfileDTO followedUser) =>
        await _authService.ToggleUserFollowedUser(followedUser);

    [HttpPost, Authorize]
    public async Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum) =>
        await _musicAuthService.ToggleUserLikedAlbum(likedAlbum);

    #endregion

    #region DeleteRequests
    [HttpDelete, Authorize]
    public async Task<Response> DeleteProfileImage() => await _authService.DeleteProfileImage();

    [HttpDelete, Authorize]
    public async Task<Response> DeleteNotification(Notification notification) =>
        await _authService.DeleteNotification(notification);

    [HttpDelete, Authorize]
    public async Task<Response> DeleteAllNotifications() => await _authService.DeleteAllNotifications();

    [HttpGet]
    public async Task<string> RefreshToken() => await _spotifyRefreshToken.RefreshToken();
    #endregion
}