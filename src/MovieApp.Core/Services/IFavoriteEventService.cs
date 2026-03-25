using MovieApp.Core.Models;

namespace MovieApp.Core.Services;

public interface IFavoriteEventService
{
    Task AddFavoriteAsync(int userId, int eventId, CancellationToken cancellationToken = default);
    Task RemoveFavoriteAsync(int userId, int eventId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FavoriteEvent>> GetFavoritesByUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsFavoriteAsync(int userId, int eventId, CancellationToken cancellationToken = default);
}
