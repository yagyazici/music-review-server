using MusicReview.Domain.Models.Spotify.Common;

namespace MusicReview.Domain.Models.Spotify.CurrentSong;

public class CurrentSongItem
{
    public SpotifyAlbum album { get; set; }
    public List<SpotifyArtist> artists { get; set; }
    public string name { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
}