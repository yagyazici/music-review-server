namespace MusicReview.Domain.Models.Spotify.Common;

public class SpotifyItem
{
    public int duration_ms { get; set; }
    public bool @explicit { get; set; }
    public string name { get; set; }
}