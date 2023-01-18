using MusicReview.Domain.Models.Base;
using MusicReview.DTOs;

namespace MusicReview.Auth;

public class Notification : MongoEntity
{
    public string NotificationType { get; set; }
    public bool NotificationSeen { get; set; }
    public UserProfileDTO FromUser { get; set; }
    public DateTime NotificationDate { get; set; }
}