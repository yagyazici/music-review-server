using MusicReview.Auth;
using MusicReview.Domain.DTOs;

namespace MusicReview.Domain.NotificationServices;

public interface INotificationServices
{
    Task<string> SendNotification(string toUserId, UserProfileDTO fromUser, string notifType);
    Task<List<Notification>> GetUserNotifications(User user);
    Task<List<Notification>> DeleteNotification(User user, Notification notification);
    Task DeleteAllNotification(User user);
    int GetUserNotificationCount(User user);
}