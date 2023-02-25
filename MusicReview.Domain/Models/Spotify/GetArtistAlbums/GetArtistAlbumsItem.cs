using MusicReview.Domain.Models.Spotify.Common;

namespace MusicReview.Domain.Models.Spotify.GetArtistAlbums;

public class GetArtistAlbumsItem
{
    public List<SpotifyArtist> artists { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
    public string release_date { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}