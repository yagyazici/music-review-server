using MusicReview.Domain.DTOs;
using MusicReview.Domain.Models.Base;

namespace MusicReview.Domain.Auth;
public class Reply : MongoEntity
{
    public UserProfileDTO User { get; set; }
    public string Comment { get; set; }
    public DateTime ReplyDate { get; set; }
}