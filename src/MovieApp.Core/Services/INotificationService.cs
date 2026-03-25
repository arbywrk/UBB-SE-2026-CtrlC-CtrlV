using MovieApp.Core.Models;

namespace MovieApp.Core.Services;

public interface INotificationService
{
    Task GeneratePriceDropNotificationAsync(int eventId, string eventTitle, CancellationToken cancellationToken = default);
    Task GenerateSeatsAvailableNotificationAsync(int eventId, string eventTitle, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Notification>> GetNotificationsByUserAsync(int userId, CancellationToken cancellationToken = default);
    Task RemoveNotificationAsync(int notificationId, CancellationToken cancellationToken = default);
}
