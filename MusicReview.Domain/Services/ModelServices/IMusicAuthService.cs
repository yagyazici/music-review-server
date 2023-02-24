using MusicReview.Domain.Models.Base;
using MusicReview.Domain.DTOs;

namespace MusicReview.Domain.Services.ModelServices;

public interface IMusicAuthService
{
    Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums);
    Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum);
    Task<int> GetAlbumLikedCount(string albumId);
}