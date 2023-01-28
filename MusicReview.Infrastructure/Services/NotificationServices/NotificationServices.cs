using MusicReview.Auth;
using MusicReview.Domain.NotificationServices;
using MusicReview.Domain.Services;
using MusicReview.DTOs;

namespace MusicReview.Infrastructure.Services.NotificationServices;

public class NotificationServices : INotificationServices
{
    private readonly IGenericMongoRepository<User> _mongoRepository;

    public NotificationServices(IGenericMongoRepository<User> mongoRepository)
    {
        _mongoRepository = mongoRepository;
    }

    public int GetUserNotificationCount(User user)
    {
        var notifications = user.Notifications.Where(notification => notification.NotificationSeen == false).Count();
        return notifications;
    }

    public async Task<List<Notification>> GetUserNotifications(User user)
    {
        user.Notifications.FindAll(notification => notification.NotificationSeen = true);
        await _mongoRepository.UpdateAsync(user);
        return user.Notifications;
    }

    public async Task<string> SendNotification(string toUserId, UserProfileDTO fromUser, string notifType)
    {
        var toUser = await _mongoRepository.GetByIdAsync(toUserId);
        var notification = new Notification {
            // Id = ObjectId.GenerateNewId().ToString(),
            NotificationType = notifType,
            NotificationSeen = false,
            FromUser = fromUser,
            NotificationDate = DateTime.Now
        };
        toUser.Notifications.Add(notification);
        await _mongoRepository.UpdateAsync(toUser);
        return "Notification sended";
    }
}