using AutoMapper;
using MusicReview.Applications.Applications;
using MusicReview.Auth;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Models.Responses;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Domain.Services.ModelServices;
using MusicReview.DTOs;

namespace MusicReview.Applications.Services;

public class MusicAuthService : IMusicAuthService
{
    private readonly AuthApplications _authApplications;
    private readonly IGenericMongoRepository<User> _mongoRepository;
    private readonly IMapper _mapper;
    private readonly IUserHubService _userHubService;

    public MusicAuthService(AuthApplications authApplications,
        IGenericMongoRepository<User> mongoRepository,
        IMapper mapper,
        IUserHubService userHubService)
    {
        _authApplications = authApplications;
        _mongoRepository = mongoRepository;
        _mapper = mapper;
        _userHubService = userHubService;
    }

    public async Task<Response> AddUserFavoriteAlbums(List<AlbumDTO> favoriteAlbums)
    {
        var user = await _authApplications.GetCurrentUser();
        user.FavoriteAlbums = favoriteAlbums;
        await _mongoRepository.UpdateAsync(user);
        await _userHubService.UserFavoriteAlbumsUpdatedMessageAsync(user.Id);
        return new Success(true, "Album(s) appended");
    }

    public async Task<int> GetAlbumLikedCount(string albumId)
    {
        var users = await _mongoRepository.FilterAsync(
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
        if (check) user.LikedAlbums.Remove(likedAlbum); 
        else user.LikedAlbums.Add(likedAlbum);
        await _mongoRepository.UpdateAsync(user);
        return new Success(true, check ? "removed" : "liked");
    }
}