using MovieApp.Core.Models;
using MovieApp.Core.Repositories;

namespace MovieApp.Core.Services;

public sealed class FavoriteEventService : IFavoriteEventService
{
    private readonly IFavoriteEventRepository _favoriteEventRepository;
    private readonly IEventRepository _eventRepository;

    public FavoriteEventService(IFavoriteEventRepository favoriteEventRepository, IEventRepository eventRepository)
    {
        _favoriteEventRepository = favoriteEventRepository;
        _eventRepository = eventRepository;
    }

    public async Task AddFavoriteAsync(int userId, int eventId, CancellationToken cancellationToken = default)
    {
        if (await ExistsFavoriteAsync(userId, eventId, cancellationToken))
        {
            throw new InvalidOperationException("Event is already favorited by this user.");
        }

        var @event = await _eventRepository.FindByIdAsync(eventId, cancellationToken);
        if (@event == null)
        {
            throw new InvalidOperationException("Event not found.");
        }

        await _favoriteEventRepository.AddAsync(userId, eventId, cancellationToken);
    }

    public async Task RemoveFavoriteAsync(int userId, int eventId, CancellationToken cancellationToken = default)
    {
        await _favoriteEventRepository.RemoveAsync(userId, eventId, cancellationToken);
    }

    public async Task<IReadOnlyList<FavoriteEvent>> GetFavoritesByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _favoriteEventRepository.FindByUserAsync(userId, cancellationToken);
    }

    public async Task<bool> ExistsFavoriteAsync(int userId, int eventId, CancellationToken cancellationToken = default)
    {
        return await _favoriteEventRepository.ExistsAsync(userId, eventId, cancellationToken);
    }
}
