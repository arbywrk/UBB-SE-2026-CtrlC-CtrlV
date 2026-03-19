using MovieApp.Core.Models;

namespace MovieApp.Ui.Services;

public interface ICurrentUserService
{
    User CurrentUser { get; }

    Task InitializeAsync(CancellationToken cancellationToken = default);
}
