using MovieApp.Core.Models;

namespace MovieApp.Core.Repositories;

public interface IUserRepository
{
    Task<User?> FindByAuthIdentityAsync(string authProvider, string authSubject, CancellationToken cancellationToken = default);
}
