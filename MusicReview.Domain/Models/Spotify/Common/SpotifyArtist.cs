namespace MusicReview.Domain.Models.Spotify.Common;

public class SpotifyArtist
{
    public SpotifyExternalUrls external_urls { get; set; }
    public List<string> genres { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
}