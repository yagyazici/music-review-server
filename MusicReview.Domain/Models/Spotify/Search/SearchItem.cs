using MusicReview.Domain.Models.Spotify.Common;

namespace MusicReview.Domain.Models.Spotify.Search;

public class SearchItem
{
    public string name { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string release_date { get; set; }
    public List<SpotifyArtist> artists { get; set; }
}