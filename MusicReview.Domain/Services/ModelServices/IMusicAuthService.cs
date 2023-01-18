using MusicReview.Domain.Models.Base;
using MusicReview.DTOs;

public interface IMusicAuthService
{
    Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums);
    Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum);
    Task<Response> ToggleUserFollowedUser(UserProfileDTO followedUser);
}