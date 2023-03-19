using MusicReview.Domain.Auth;
using MusicReview.Domain.Models;
using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Services.ModelServices;

public interface IMusicService
{
    Task<Response> Put(string id, int newRate, string newThoughts);
    Task<Review> Get(string id);
    Task<List<Review>> GetAlbumReviews(string albumId);
    Task<bool> AlbumReviewCheck(string albumId);
    Task<List<Review>> GetUserFeed();
    Task<Response> Post(Review review);
    Task<Response> ToggleLikedReview(string reviewId);
    Task<Response> Delete(string id);
    Task<Response> Reply(string comment, string reviewId);
    Task<Response> DeleteReply(string reviewId, string replyId);
}