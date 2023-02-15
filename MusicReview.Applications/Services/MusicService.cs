using AutoMapper;
using MongoDB.Driver;
using MusicReview.Applications.Applications;
using MusicReview.Domain.Models;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Models.Responses;
using MusicReview.Domain.NotificationServices;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Domain.Services.ModelServices;
using MusicReview.DTOs;

namespace MusicReview.Applications.Services;

public class MusicService : IMusicService
{
    private readonly AuthApplications _authApplications;
    private readonly IGenericMongoRepository<Review> _mongoRepository;
    private readonly IMapper _mapper;
    private readonly IMusicReviewHubService _musicHubService;
    private readonly INotificationServices _notificationService;
    private readonly IUserHubService _hubService;

    public MusicService(
        AuthApplications authApplications,
        IGenericMongoRepository<Review> mongoRepository,
        IMapper mapper,
        IMusicReviewHubService musicHubService,
        INotificationServices notificationService,
        IUserHubService hubService)
    {
        _authApplications = authApplications;
        _mongoRepository = mongoRepository;
        _mapper = mapper;
        _musicHubService = musicHubService;
        _notificationService = notificationService;
        _hubService = hubService;
    }

    public async Task<bool> AlbumReviewCheck(string albumId)
    {
        var user = await _authApplications.GetCurrentUser();
        var check = await _mongoRepository.FilterAsync(
            review => review.AlbumId == albumId && review.Author.Id == user.Id
        );
        return check.Any();
    }

    public async Task<Response> Delete(string id)
    {
        var user = await _authApplications.GetCurrentUser();
        var review = await _mongoRepository.GetByIdAsync(id);
        if (review.Author.Id == user.Id)
        {
            await _mongoRepository.RemoveAsync(review.Id);
            await _musicHubService.MusicReviewDeletedMessageAsync(user.Id);
            return new Success(review.AlbumName, "review deleted");
        }
        return new Fail(user.Username, "not correct author");
    }

    public async Task<Review> Get(string id) => await _mongoRepository.GetByIdAsync(id);

    public async Task<List<Review>> GetAlbumReviews(string albumId) =>
        await _mongoRepository.FilterAsync(review => review.AlbumId == albumId);

    public async Task<List<Review>> GetUserAlbumReviews(string userId) =>
        await _mongoRepository.FilterAsync(review => review.Author.Id == userId);

    public async Task<List<Review>> GetUserFeed()
    {
        var user = await _authApplications.GetCurrentUser();
        var ids = user.Followings.Select(user => user.Id).ToList();
        var feed = await _mongoRepository.Filter(
            Builders<Review>.Filter.In(review => review.Author.Id, ids)
        );
        return feed.ToList();
    }

    public async Task<Response> ToggleLikedReview(string reviewId)
    {
        var user = await _authApplications.GetCurrentUser();
        var fromUser = _mapper.Map<UserProfileDTO>(user);
        var userId = user.Id;
        var review = await _mongoRepository.GetByIdAsync(reviewId);
        var contains = review.Likes.Contains(userId);
        if (contains)
        {
            review.Likes.Remove(userId);
            await _mongoRepository.UpdateAsync(review);
            return new Success(review.AlbumName, "removed");
        }        
        review.Likes.Add(userId);
        await _mongoRepository.UpdateAsync(review);
        await _notificationService.SendNotification(review.Author.Id, fromUser, "review like");
        await _hubService.UserSendNotificitionMessageAsync(review.Author.Id);
        return new Success(review.AlbumName, contains ? "removed" : "added");
    }

    public async Task<Response> Post(Review review)
    {
        await _mongoRepository.AddAsync(review);
        await _musicHubService.MusicReviewAddedMessageAsync(review);
        return new Success(review.AlbumName, "review posted");
    }

    public async Task<Response> Put(string id, int newRate, string newThoughts)
    {
        var user = await _authApplications.GetCurrentUser();
        var review = await _mongoRepository.GetByIdAsync(id);
        if (review.Author.Id == user.Id)
        {
            review.Edited = true;
            review.EditedDate = DateTime.Now;
            review.AlbumRate = newRate;
            review.AlbumThoughts = newThoughts;
            await _mongoRepository.UpdateAsync(review);
            await _musicHubService.MusicReviewUpdateMessageAsync(id);
            return new Success(review.AlbumName, "updated");
        }
        return new Fail(false, "not correct author");
    }
}