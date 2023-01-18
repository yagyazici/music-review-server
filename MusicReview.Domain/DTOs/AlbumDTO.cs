using MusicReview.Domain.Models.AlbumEntities;

namespace MusicReview.DTOs;

public class AlbumDTO
{
    public List<Artist> artists { get; set; }
    public string id { get; set; }
    public List<Image> images { get; set; }
    public string name { get; set; }
}