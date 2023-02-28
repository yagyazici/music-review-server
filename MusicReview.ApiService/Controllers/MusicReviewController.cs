using Microsoft.AspNetCore.Mvc;
using MusicReview.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Services.ModelServices;
using MusicReview.Domain.Auth;

namespace MusicReview.ApiService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MusicReviewController : ControllerBase
{
    private readonly IMusicService _musicService;
    private readonly IMusicAuthService _musicAuthService;

    public MusicReviewController(
        IMusicService musicService,
        IMusicAuthService musicAuthService
    )
    {
        _musicService = musicService;
        _musicAuthService = musicAuthService;
    }

    #region PutRequests
    [HttpPut]
    public async Task<Response> Put(string id, int newRate, string newThoughts) =>
        await _musicService.Put(id, newRate, newThoughts);
    #endregion

    #region GetRequests
    [HttpGet]
    public async Task<Review> Get(string id) => await _musicService.Get(id);

    [HttpGet]
    public async Task<List<Review>> GetAlbumReviews(string albumId) => await _musicService.GetAlbumReviews(albumId);

    [HttpGet]
    public async Task<List<Review>> GetUserAlbumReviews(string userId) => await _musicService.GetUserAlbumReviews(userId);

    [HttpGet]
    public async Task<bool> AlbumReviewCheck(string albumId) => await _musicService.AlbumReviewCheck(albumId);

    [HttpGet]
    public async Task<int> GetAlbumLikedCount(string albumId) => await _musicAuthService.GetAlbumLikedCount(albumId);

    [HttpGet, Authorize]
    public async Task<List<Review>> GetUserFeed() => await _musicService.GetUserFeed();
    #endregion

    #region PostRequests
    [HttpPost, Authorize]
    public async Task<Response> Post(Review review) => await _musicService.Post(review);
    
    [HttpPost, Authorize]
    public async Task<Response> ToggleLikedReview(string reviewId) => await _musicService.ToggleLikedReview(reviewId);

    [HttpPost, Authorize]
    public async Task<Response> Reply(Reply reply, string reviewId) => await _musicService.Reply(reply, reviewId);
    // ^Test this feature
    #endregion

    #region DeleteRequests
    [HttpDelete]
    public async Task<Response> Delete(string id) => await _musicService.Delete(id);
    #endregion
}