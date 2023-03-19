using MusicReview.Domain.Models.Base;
using MusicReview.Domain.DTOs;
using MusicReview.Domain.Models;

namespace MusicReview.Domain.Services.ModelServices;

public interface IMusicAuthService
{
    Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums);
    Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum);
    Task<int> GetAlbumLikedCount(string albumId);
    Task<List<Review>> GetUserAlbumReviews(string userId);
}