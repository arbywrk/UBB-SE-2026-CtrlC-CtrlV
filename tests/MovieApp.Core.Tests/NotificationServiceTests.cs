using System.Threading.Tasks;
using MovieApp.Core.Tests.Fakes;
using MovieApp.Core.Services;
using Xunit;
using System.Linq;

namespace MovieApp.Core.Tests;

public class NotificationServiceTests
{
    private readonly FakeNotificationRepository _notificationRepo;
    private readonly FakeFavoriteEventRepository _favoriteRepo;
    private readonly NotificationService _sut;

    public NotificationServiceTests()
    {
        _notificationRepo = new FakeNotificationRepository();
        _favoriteRepo = new FakeFavoriteEventRepository();
        _sut = new NotificationService(_notificationRepo, _favoriteRepo);
    }

    [Fact]
    public async Task GeneratePriceDropNotificationAsync_NotifiesFavoritedUsers()
    {
        await _favoriteRepo.AddAsync(100, 1);
        await _favoriteRepo.AddAsync(101, 1);
        await _favoriteRepo.AddAsync(102, 2); // Different event

        await _sut.GeneratePriceDropNotificationAsync(1, "Test Event");

        var user100Notifs = await _sut.GetNotificationsByUserAsync(100);
        Assert.Single(user100Notifs);
        Assert.Equal("PRICE_DROP", user100Notifs[0].Type);

        var user101Notifs = await _sut.GetNotificationsByUserAsync(101);
        Assert.Single(user101Notifs);

        var user102Notifs = await _sut.GetNotificationsByUserAsync(102);
        Assert.Empty(user102Notifs);
    }

    [Fact]
    public async Task GenerateSeatsAvailableNotificationAsync_AvoidsDuplicates()
    {
        await _favoriteRepo.AddAsync(100, 1);

        await _sut.GenerateSeatsAvailableNotificationAsync(1, "Test Event");
        await _sut.GenerateSeatsAvailableNotificationAsync(1, "Test Event");

        var user100Notifs = await _sut.GetNotificationsByUserAsync(100);
        Assert.Single(user100Notifs); // Still 1 because it prevents duplicates
    }
}
