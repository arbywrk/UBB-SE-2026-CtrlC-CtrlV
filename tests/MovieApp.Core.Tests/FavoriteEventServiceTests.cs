using System;
using System.Threading.Tasks;
using MovieApp.Core.Models;
using MovieApp.Core.Services;
using MovieApp.Core.Tests.Fakes;
using Xunit;

namespace MovieApp.Core.Tests;

public class FavoriteEventServiceTests
{
    private readonly FakeFavoriteEventRepository _favoriteRepo;
    private readonly FakeEventRepository _eventRepo;
    private readonly FavoriteEventService _sut;

    public FavoriteEventServiceTests()
    {
        _favoriteRepo = new FakeFavoriteEventRepository();
        _eventRepo = new FakeEventRepository();
        _sut = new FavoriteEventService(_favoriteRepo, _eventRepo);
    }

    [Fact]
    public async Task AddFavoriteAsync_AddsToRepository()
    {
        _eventRepo.Items.Add(new Event { Id = 1, Title = "Test Event", EventDateTime = DateTime.Now, LocationReference = "Loc", TicketPrice = 10, CreatorUserId = 1 });

        await _sut.AddFavoriteAsync(100, 1);

        var favorites = await _sut.GetFavoritesByUserAsync(100);
        Assert.Single(favorites);
        Assert.Equal(1, favorites[0].EventId);
    }

    [Fact]
    public async Task AddFavoriteAsync_DuplicateThrowsException()
    {
        _eventRepo.Items.Add(new Event { Id = 1, Title = "Test Event", EventDateTime = DateTime.Now, LocationReference = "Loc", TicketPrice = 10, CreatorUserId = 1 });
        await _sut.AddFavoriteAsync(100, 1);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.AddFavoriteAsync(100, 1));
    }

    [Fact]
    public async Task RemoveFavoriteAsync_RemovesFromRepository()
    {
        _eventRepo.Items.Add(new Event { Id = 1, Title = "Test Event", EventDateTime = DateTime.Now, LocationReference = "Loc", TicketPrice = 10, CreatorUserId = 1 });
        await _sut.AddFavoriteAsync(100, 1);

        await _sut.RemoveFavoriteAsync(100, 1);

        var favorites = await _sut.GetFavoritesByUserAsync(100);
        Assert.Empty(favorites);
    }
}
