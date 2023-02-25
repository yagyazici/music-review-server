namespace MusicReview.Domain.Models.Spotify.Common;

public class SpotifyAlbum
{
    public string album_type { get; set; }
    public List<SpotifyArtist> artists { get; set; }
    public List<SpotifyCopyright> copyrights { get; set; }
    public SpotifyExternalUrls external_urls { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
    public string release_date { get; set; }
    public int total_tracks { get; set; }
    public SpotifyTracks tracks { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}