namespace MusicReview.Domain.Models.Spotify.RefreshToken;
public class RefreshToken
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
}