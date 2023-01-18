namespace MusicReview.Domain.Models.AlbumEntities;

public class Album
{
    public List<Artist> artists { get; set; }
    public string id { get; set; }
    public List<Image> images { get; set; }
    public string name { get; set; }
    public string release_date { get; set; }
    public int total_tracks { get; set; }
    public string album_type { get; set; }
    public List<Copyright> copyrights { get; set; }
    public ExternalUrls external_urls { get; set; }
    public Tracks tracks { get; set; }
}