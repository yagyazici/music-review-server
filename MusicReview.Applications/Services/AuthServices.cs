using MusicReview.Auth;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Applications.Applications;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.Models.Responses;
using AutoMapper;
using MusicReview.DTOs;
using MongoDB.Driver;
using MusicReview.Domain.Models.AlbumEntities;
using MusicReview.Domain.Services.ModelServices;

namespace MusicReview.Applications.Services;

public class AuthServices : IAuthServices
{
    private readonly IGenericMongoRepository<User> _mongoRepository;
    private readonly IUserHubService _hubService;
    private readonly AuthApplications _authApplications;
    private readonly IMapper _mapper;

    public AuthServices(
        IGenericMongoRepository<User> mongoRepository,
        IUserHubService hubService,
        AuthApplications authApplications,
        IMapper mapper
    )
    {
        _mongoRepository = mongoRepository;
        _hubService = hubService;
        _authApplications = authApplications;
        _mapper = mapper;
    }

    public async Task<Response> UpdateUser(string username, string bio, string birthDate, string email)
    {
        var user = await _authApplications.GetCurrentUser();
        List<string> errorList = _authApplications.UpdateUserSendErrorList(user, username, email);
        if (errorList.Count > 0) return new Fail(errorList, "Error occured while updating.");
        user.Username = username;
        user.Bio = bio;
        user.BirthDate = DateTime.ParseExact(birthDate, "dd-MM-yyyy", null);
        user.Email = email;
        await _mongoRepository.UpdateAsync(user);
        return new Success(user.Username == username, "user updated");
    }

    public async Task<CurrentUserDTO> GetMeUser()
    {
        var user = await _authApplications.GetCurrentUser();
        return _mapper.Map<CurrentUserDTO>(user);
    }

    public async Task<UserProfileDTO> GetUserProfile(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        return _mapper.Map<UserProfileDTO>(user);
    }

    public async Task<List<UserProfileDTO>> SearchUserProfile(string username)
    {
        var users = await _mongoRepository.FilterAsync(user => user.Username == username);
        return _mapper.Map<List<UserProfileDTO>>(users);
    }

    public async Task<List<Album>> GetCurrentUserFavoriteAlbums()
    {
        var user = await _authApplications.GetCurrentUser();
        return _mapper.Map<List<Album>>(user.FavoriteAlbums);
    }

    public async Task<bool> CheckUserAlbumLiked(string albumId)
    {
        var user = await _authApplications.GetCurrentUser();
        var check = user.LikedAlbums.Where(
            album => album.id == albumId
        ).Any();
        return check;
    }

    public async Task<List<Album>> GetUserLikedAlbums(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        return _mapper.Map<List<Album>>(user.LikedAlbums);
    }

    public async Task<bool> CheckUserFollowed(string userId)
    {
        var user = await _authApplications.GetCurrentUser();
        return user.Followings.Where(user => user.Id == userId).Any();
    }

    public async Task<Follingers> GetUserFollingers(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        var followers = user.Followings.Count();
        var followings = user.Followings.Count();
        return new Follingers(followers, followings);
    }

    public async Task<List<UserProfileDTO>> GetUserFollowings(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        return user.Followings;
    }

    public async Task<List<UserProfileDTO>> GetUserFollowers(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        return user.Followers;
    }

    public async Task<List<Notification>> GetUserNotifications()
    {
        var user = await _authApplications.GetCurrentUser();
        return user.Notifications;
    }

    public async Task<int> GetUserNotificationCount()
    {
        var user = await _authApplications.GetCurrentUser();
        return user.Notifications.Count();
    }

    public async Task<Response> Register(UserDTO request)
    {
        List<string> errorList = _authApplications.RegisterSendErrorList(request, request.Username, request.Email, request.Password);
        if (errorList.Count > 0) return new Fail(errorList, "Error(s) occured");
        _authApplications.CreatePassword(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = request.Email,
        };
        await _mongoRepository.AddAsync(user);
        return new Success(user, "successful register");
    }

    public async Task<Response> Login(UserDTO request)
    {
        var emailCheck = await _mongoRepository.FilterAsync(user => user.Email == request.Email);
        if (!emailCheck.Any()) return new Fail(false, "Email not found");
        User user = _mongoRepository.FilterAsync(user => user.Email == request.Email).Result.FirstOrDefault();
        if (!_authApplications.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new Fail("Wrong password", "!");
        }
        var authToken = _authApplications.CreateToken(user);
        var refreshToken = _authApplications.GenerateRefreshToken();
        _authApplications.SetRefreshToken(refreshToken, user);
        await _mongoRepository.UpdateAsync(user);
        return new Success(new { authToken, refreshToken }, "successful login");
    }

    public async Task<Response> RefreshToken(string refreshToken, string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        if (!user.RefreshToken.Equals(refreshToken)) return new Fail(false, "Invalid refresh token");
        var token = _authApplications.CreateToken(user);
        return new Success(token, "refresh token created");
    }

    public async Task<Response> UploadProfileImage(string databasePath)
    {
        var currentUser = await _authApplications.GetCurrentUser();
        currentUser.ProfilePicture = databasePath;
        await _mongoRepository.UpdateAsync(currentUser);
        return new Success(currentUser, "profile picture updated");
    }
}