namespace MusicReview.Domain.Models.Spotify.CurrentSong;

public class SpotifyCurrentSong
{
    public bool is_playing { get; set; }
    public CurrentSongItem item { get; set; }
}

