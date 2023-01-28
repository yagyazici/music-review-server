using MusicReview.Domain.Models;
using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Services.ModelServices;

public interface IMusicService
{
    Task<Response> Put(string id, int newRate, string newThoughts);
    Task<Review> Get(string id);
    Task<List<Review>> GetAlbumReviews(string albumId);
    Task<List<Review>> GetUserAlbumReviews(string userId);
    Task<bool> AlbumReviewCheck(string albumId);
    Task<List<Review>> GetUserFeed();
    Task<Response> Post(Review review);
    Task<Response> Delete(string id);
}