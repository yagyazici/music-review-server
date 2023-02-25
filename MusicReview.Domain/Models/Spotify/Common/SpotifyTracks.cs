namespace MusicReview.Domain.Models.Spotify.Common;

public class SpotifyTracks
{
    public string href { get; set; }
    public List<SpotifyItem> items { get; set; }
    public int limit { get; set; }
    public object next { get; set; }
    public int offset { get; set; }
    public object previous { get; set; }
    public int total { get; set; }
}