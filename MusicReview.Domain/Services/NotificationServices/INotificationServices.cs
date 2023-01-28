using MusicReview.Auth;
using MusicReview.DTOs;

namespace MusicReview.Domain.NotificationServices;

public interface INotificationServices
{
    Task<string> SendNotification(string toUserId, UserProfileDTO fromUser, string notifType);
    Task<List<Notification>> GetUserNotifications(User user);
    int GetUserNotificationCount(User user);
}