using MongoDB.Bson;
using MusicReview.Auth;
using MusicReview.Domain.Models.Base;
using MusicReview.Domain.NotificationServices;
using MusicReview.Domain.Services;
using MusicReview.Domain.Services.HubServices;
using MusicReview.Domain.DTOs;

namespace MusicReview.Infrastructure.Services.NotificationServices;

public class NotificationServices : INotificationServices
{
    private readonly IGenericMongoRepository<User> _mongoRepository;
    private readonly IUserHubService _hubService;

    public NotificationServices(IGenericMongoRepository<User> mongoRepository, IUserHubService hubService)
    {
        _mongoRepository = mongoRepository;
        _hubService = hubService;
    }

    public async Task DeleteAllNotification(User user)
    {
        user.Notifications.Clear();
        await _mongoRepository.UpdateAsync(user);
    }

    public async Task<List<Notification>> DeleteNotification(User user, Notification notification)
    {
        user.Notifications.Remove(
            user.Notifications.Where(notif => notif.Id == notification.Id).FirstOrDefault()
        );
        await _mongoRepository.UpdateAsync(user);
        return user.Notifications;
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
            Id = ObjectId.GenerateNewId().ToString(),
            NotificationType = notifType,
            NotificationSeen = false,
            FromUser = fromUser,
            NotificationDate = DateTime.Now
        };
        toUser.Notifications.Add(notification);
        await _mongoRepository.UpdateAsync(toUser);
        await _hubService.UserSendNotificitionMessageAsync(toUserId);
        return "Notification sended";
    }
}