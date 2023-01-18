namespace MusicReview.Domain.Models.AlbumEntities;

public class Item
{
    public int duration_ms { get; set; }
    public bool @explicit { get; set; }
    public string name { get; set; }
}