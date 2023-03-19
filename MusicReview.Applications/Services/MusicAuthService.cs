using AutoMapper;
using MusicReview.Applications.Applications;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Models.Responses;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Domain.Services.ModelServices;
using MusicReview.Domain.DTOs;
using MusicReview.Domain.Auth;
using MusicReview.Domain.Models;

namespace MusicReview.Applications.Services;

public class MusicAuthService : IMusicAuthService
{
    private readonly AuthApplications _authApplications;
    private readonly IGenericMongoRepository<User> _userRepository;
    private readonly IGenericMongoRepository<Review> _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IUserHubService _userHubService;

    public MusicAuthService(
        AuthApplications authApplications,
        IGenericMongoRepository<User> mongoRepository,
        IMapper mapper,
        IUserHubService userHubService,
        IGenericMongoRepository<Review> reviewRepository)
    {
        _authApplications = authApplications;
        _userRepository = mongoRepository;
        _mapper = mapper;
        _userHubService = userHubService;
        _reviewRepository = reviewRepository;
    }

    public async Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums)
    {
        var user = await _authApplications.GetCurrentUser();
        user.FavoriteAlbums = favoriteAlbums;
        await _userRepository.UpdateAsync(user);
        await _userHubService.UserFavoriteAlbumsUpdatedMessageAsync(user.Id);
        return new Success<bool>(true, "Album(s) appended");
    }

    public async Task<int> GetAlbumLikedCount(string albumId)
    {
        var users = await _userRepository.FilterAsync(
            user => user.LikedAlbums.Where(
                album => album.id == albumId
            ).Any()
        );
        return users.Count();
    }
    
    public async Task<Response> ToggleUserLikedAlbum(AlbumDTO likedAlbum)
    {
        var user = await _authApplications.GetCurrentUser();
        var check = user.LikedAlbums.Where(album => album.id == likedAlbum.id).Any();
        if (check) user.LikedAlbums.Remove(
            user.LikedAlbums.Where(album => album.id == likedAlbum.id).FirstOrDefault()
        ); 
        else user.LikedAlbums.Add(likedAlbum);
        await _userRepository.UpdateAsync(user);
        return new Success<bool>(true, check ? "removed" : "liked");
    }

    public async Task<List<Review>> GetUserAlbumReviews(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var userDto = _mapper.Map<UserProfileDTO>(user);
        var reviews = await _reviewRepository.FilterAsync(review => review.Author.Id == userId);
        reviews = reviews.Select(review => { review.Author = userDto; return review; }).ToList();
        return reviews;
    }
}