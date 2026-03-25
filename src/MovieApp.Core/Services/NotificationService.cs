using MovieApp.Core.Models;
using MovieApp.Core.Repositories;

namespace MovieApp.Core.Services;

public sealed class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IFavoriteEventRepository _favoriteEventRepository;

    public NotificationService(INotificationRepository notificationRepository, IFavoriteEventRepository favoriteEventRepository)
    {
        _notificationRepository = notificationRepository;
        _favoriteEventRepository = favoriteEventRepository;
    }

    public async Task GeneratePriceDropNotificationAsync(int eventId, string eventTitle, CancellationToken cancellationToken = default)
    {
        await GenerateNotificationForFavoritesAsync(eventId, "PRICE_DROP", $"The ticket price for '{eventTitle}' has dropped!", cancellationToken);
    }

    public async Task GenerateSeatsAvailableNotificationAsync(int eventId, string eventTitle, CancellationToken cancellationToken = default)
    {
        await GenerateNotificationForFavoritesAsync(eventId, "SEATS_AVAILABLE", $"Seats are now available for '{eventTitle}'!", cancellationToken);
    }

    private async Task GenerateNotificationForFavoritesAsync(int eventId, string type, string message, CancellationToken cancellationToken)
    {
        var favoritedUsers = await _favoriteEventRepository.GetUsersByFavoriteEventAsync(eventId, cancellationToken);
        
        foreach (var userId in favoritedUsers)
        {
            var notifications = await _notificationRepository.FindByUserAsync(userId, cancellationToken);
            var recentNotification = notifications.FirstOrDefault(n => n.EventId == eventId && n.Type == type);
            
            if (recentNotification != null && recentNotification.State == NotificationState.Unread)
            {
                continue;
            }

            var notification = new Notification
            {
                Id = 0,
                UserId = userId,
                EventId = eventId,
                Type = type,
                Message = message,
                State = NotificationState.Unread,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification, cancellationToken);
        }
    }

    public async Task<IReadOnlyList<Notification>> GetNotificationsByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _notificationRepository.FindByUserAsync(userId, cancellationToken);
    }

    public async Task RemoveNotificationAsync(int notificationId, CancellationToken cancellationToken = default)
    {
        await _notificationRepository.RemoveAsync(notificationId, cancellationToken);
    }
}
