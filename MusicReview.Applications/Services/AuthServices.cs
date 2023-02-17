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
using MusicReview.Domain.NotificationServices;

namespace MusicReview.Applications.Services;

public class AuthServices : IAuthServices
{
    private readonly IGenericMongoRepository<User> _mongoRepository;
    private readonly IUserHubService _hubService;
    private readonly AuthApplications _authApplications;
    private readonly IMapper _mapper;
    private readonly INotificationServices _notificationService;

    public AuthServices(
        IGenericMongoRepository<User> mongoRepository,
        IUserHubService hubService,
        AuthApplications authApplications,
        IMapper mapper,
        INotificationServices notificationService)
    {
        _mongoRepository = mongoRepository;
        _hubService = hubService;
        _authApplications = authApplications;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task<Response> UpdateUser(string username, string bio, string birthDate, string email)
    {
        var user = await _authApplications.GetCurrentUser();
        List<string> errorList = _authApplications.UpdateUserSendErrorList(user, username, email);
        if (errorList.Count > 0) return new Fail<List<string>>(errorList, "Error occured while updating.");
        user.Username = username;
        user.Bio = bio;
        user.BirthDate = DateTime.ParseExact(birthDate, "dd-MM-yyyy", null);
        user.Email = email;
        await _mongoRepository.UpdateAsync(user);
        return new Success<bool>(user.Username == username, "user updated");
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
        var builder = Builders<User>.Filter;
        var filter = builder.Regex("Username", "^" + username + ".*");
        var users = await _mongoRepository.Filter(filter);
        return _mapper.Map<List<UserProfileDTO>>(users);
    }

    public async Task<List<Album>> GetCurrentUserFavoriteAlbums()
    {
        var user = await _authApplications.GetCurrentUser();
        return _mapper.Map<List<Album>>(user.FavoriteAlbums);
    }

    public async Task<List<Album>> GetUsersFavoriteAlbums(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
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
        var followers = user.Followers.Count();
        var followings = user.Followings.Count();
        return new Follingers(followers, followings);
    }

    public async Task<Response> ToggleUserFollowedUser(UserProfileDTO followedUser)
    {
        var currentUser = await _authApplications.GetCurrentUser();
        var currentUserDto = _mapper.Map<UserProfileDTO>(currentUser);
        var otherUser = await _mongoRepository.GetByIdAsync(followedUser.Id);

        var check = currentUser.Followings.Where(user => user.Id == followedUser.Id).Any();
        if (check)
        {
            currentUser.Followings.Remove(
                currentUser.Followings.Where(user => user.Id == followedUser.Id).FirstOrDefault()
            );
            otherUser.Followers.Remove(
                otherUser.Followers.Where(user => user.Id == currentUser.Id).FirstOrDefault()
            );
            await _mongoRepository.UpdateAsync(currentUser);
            await _mongoRepository.UpdateAsync(otherUser);
            return new Success<bool>(true, "unfollowed");
        }

        currentUser.Followings.Add(followedUser);
        otherUser.Followers.Add(currentUserDto);
        await _mongoRepository.UpdateAsync(currentUser);
        await _mongoRepository.UpdateAsync(otherUser);

        var currentUserId = currentUser.Id;
        var followedUserId = followedUser.Id;
        var userIds = new
        {
            currentUserId,
            followedUserId
        };

        await _hubService.UserFollowedUserMessageAsync(userIds);

        // TODO
        if (followedUserId != currentUserId)
        {
            await _notificationService.SendNotification(followedUserId, currentUserDto, "follower");
        }

        return new Success<bool>(true, "followed");
    }

    public async Task<List<UserProfileDTO>> GetUserFollowings(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        var userIds = user.Followings.Select(user => user.Id);
        var followings = await _mongoRepository.Filter(
            Builders<User>.Filter.In(user => user.Id, userIds)
        );
        return _mapper.Map<List<UserProfileDTO>>(followings);
    }

    public async Task<List<UserProfileDTO>> GetUserFollowers(string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        var userIds = user.Followers.Select(user => user.Id);
        var followers = await _mongoRepository.Filter(
            Builders<User>.Filter.In(user => user.Id, userIds)
        );
        return _mapper.Map<List<UserProfileDTO>>(followers);
    }

    public async Task<List<Notification>> GetUserNotifications()
    {
        var user = await _authApplications.GetCurrentUser();
        return await _notificationService.GetUserNotifications(user);
    }

    public async Task<int> GetUserNotificationCount()
    {
        var user = await _authApplications.GetCurrentUser();
        return _notificationService.GetUserNotificationCount(user);
    }

    public async Task<Response> Register(UserDTO request)
    {
        List<string> errorList = _authApplications.RegisterSendErrorList(request, request.Username, request.Email, request.Password);
        if (errorList.Count > 0) return new Fail<List<string>>(errorList, "Error(s) occured");
        _authApplications.CreatePassword(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = request.Email,
        };
        await _mongoRepository.AddAsync(user);
        return new Success<User>(user, "successful register");
    }

    public async Task<Response> Login(UserDTO request)
    {
        var usernameCheck = await _mongoRepository.FilterAsync(user => user.Username == request.Username);
        if (!usernameCheck.Any()) return new Fail<string>("Wrong username", "Error occured");
        User user = _mongoRepository.FilterAsync(user => user.Username == request.Username).Result.FirstOrDefault();
        if (!_authApplications.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new Fail<string>("Wrong password", "Error occured");
        }
        var authToken = _authApplications.CreateToken(user);
        var currentUser = _mapper.Map<CurrentUserDTO>(user);
        // 
        var refreshToken = _authApplications.GenerateRefreshToken();
        _authApplications.SetRefreshToken(refreshToken, user);
        await _mongoRepository.UpdateAsync(user);
        return new Success<LoginResponse>(new LoginResponse(authToken, refreshToken, currentUser), "successful login");
    }

    public async Task<Response> UpdatePassword(string currentPassword, string newPassword)
    {
        var user = await _authApplications.GetCurrentUser();
        if ((currentPassword == null) || (newPassword == null))
        {
            return new Fail<string>("Passwords cant be null", "Error occured");
        }
        if (!_authApplications.VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
        {
            return new Fail<string>("Old password didn't match", "Error occured");
        }
        if (!_authApplications.UpdatePasswordSendError(newPassword).Equals("true"))
        {
            return new Fail<string>("Invalid password.", "Error occured");
        }
        _authApplications.CreatePassword(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _mongoRepository.UpdateAsync(user);
        return new Success<string>(newPassword, "password changed successfully");
    }

    public async Task<Response> RefreshToken(string refreshToken, string userId)
    {
        var user = await _mongoRepository.GetByIdAsync(userId);
        if (!user.RefreshToken.Equals(refreshToken)) return new Fail<bool>(false, "Invalid refresh token");
        var token = _authApplications.CreateToken(user);
        return new Success<AuthToken>(token, "refresh token created");
    }

    public async Task<Response> UploadProfileImage(string databasePath)
    {
        var currentUser = await _authApplications.GetCurrentUser();
        currentUser.ProfilePicture = databasePath;
        await _mongoRepository.UpdateAsync(currentUser);
        return new Success<User>(currentUser, "profile picture updated");
    }

    public async Task<Response> DeleteProfileImage()
    {
        try
        {
            var currentUser = await _authApplications.GetCurrentUser();
            var fileName = currentUser.ProfilePicture;
            var applicationPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "MusicReview.ApiService");
            var fullPath = Path.Combine(applicationPath, fileName);
            File.Delete(fullPath);
            if (!File.Exists(fullPath))
            {
                currentUser.ProfilePicture = "";
                await _mongoRepository.UpdateAsync(currentUser);
                return new Success<string>(currentUser.Id, "profile picture deleted");
            }
            else
            {
                return new Fail<string>("file does not exists", "error");
            }
        }
        catch (Exception ex)
        {
            return new Fail<string>(ex.Message, "error");
        }
    }
}