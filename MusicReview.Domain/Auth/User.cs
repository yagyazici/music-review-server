using MusicReview.Domain.Models.Base;
using MusicReview.DTOs;

namespace MusicReview.Auth;

public class User : MongoEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string ProfilePicture { get; set; }
    public string Bio { get; set; }
    public DateTime BirthDate { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
    public List<AlbumDTO> FavoriteAlbums { get; set; }
    public List<AlbumDTO> LikedAlbums { get; set; }
    public List<UserProfileDTO> Followings { get; set; }
    public List<UserProfileDTO> Followers { get; set; }
    public List<Notification> Notifications { get; set; }
}