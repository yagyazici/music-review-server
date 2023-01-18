using MusicReview.Domain.Models.Base;
using MusicReview.DTOs;

namespace MusicReview.Domain.Services.ModelServices;

public interface IMusicAuthService
{
    Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums);
    Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum);
    Task<Response> ToggleUserFollowedUser(UserProfileDTO followedUser);
    Task<int> GetAlbumLikedCount(string albumId);
}