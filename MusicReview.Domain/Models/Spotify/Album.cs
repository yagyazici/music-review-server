namespace MusicReview.Domain.Models.Spotify;

public class Album
{
    public string id { get; set; }
    public List<Image> images { get; set; }
    public string name { get; set; }
}