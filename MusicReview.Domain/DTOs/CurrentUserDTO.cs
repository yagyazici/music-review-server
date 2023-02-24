namespace MusicReview.Domain.DTOs;

public class CurrentUserDTO
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ProfilePicture { get; set; }
    public string Bio { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime TokenExpires { get; set; }
}