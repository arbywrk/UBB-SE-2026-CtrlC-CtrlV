using MovieApp.Models;

namespace MovieApp.Services;

public interface ICurrentUserService
{
    User CurrentUser { get; }

    Task InitializeAsync(CancellationToken cancellationToken = default);
}
