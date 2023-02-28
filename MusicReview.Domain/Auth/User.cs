using MusicReview.Domain.DTOs;
using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Auth;

public class User : MongoEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ProfilePicture { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
    public List<AlbumDTO> FavoriteAlbums { get; set; } = new List<AlbumDTO>();
    public List<AlbumDTO> LikedAlbums { get; set; } = new List<AlbumDTO>();
    public List<UserProfileDTO> Followings { get; set; } = new List<UserProfileDTO>();
    public List<UserProfileDTO> Followers { get; set; } = new List<UserProfileDTO>();
    public List<Notification> Notifications { get; set; } = new List<Notification>();
}