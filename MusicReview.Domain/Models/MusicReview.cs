using MusicReview.Domain.Models.Base;
using MusicReview.DTOs;

namespace MusicReview.Domain.Models;

public class Review : MongoEntity
{
    public UserProfileDTO Author { get; set;}
    public string ArtistName { get; set; }
    public string ArtistId { get; set;}
    public string AlbumId { get; set; }
    public string AlbumName { get; set;}
    public string AlbumImage { get; set; }
    public int AlbumRate { get; set; }
    public string AlbumThoughts { get; set; }
    public DateTime PublishedDate { get; set; }
    public bool Edited { get; set; } = false;
    public DateTime EditedDate { get; set; }
    public List<string> Likes { get; set; } = new List<string>();
}