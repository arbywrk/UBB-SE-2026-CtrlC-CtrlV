using MovieApp.Models;

namespace MovieApp.Services;

public interface IUserRepository
{
    Task<User?> FindByAuthIdentityAsync(string authProvider, string authSubject, CancellationToken cancellationToken = default);
}
