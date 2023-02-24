namespace MusicReview.Domain.Models.Spotify.CurrentSong;

public class Item
{
    public Album album { get; set; }
    public List<Artist> artists { get; set; }
    public string name { get; set; }
}