using MovieApp.Models;

namespace MovieApp.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    public const string DummyAuthProvider = "dummy";
    public const string DummyAuthSubject = "default-user";

    private readonly IUserRepository _userRepository;
    private User? _currentUser;

    public CurrentUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User CurrentUser =>
        _currentUser ?? throw new InvalidOperationException("The current user has not been initialized.");

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUser is not null)
        {
            return;
        }

        _currentUser = await _userRepository.FindByAuthIdentityAsync(
            DummyAuthProvider,
            DummyAuthSubject,
            cancellationToken);

        if (_currentUser is null)
        {
            throw new InvalidOperationException(
                $"The seeded dummy user '{DummyAuthProvider}:{DummyAuthSubject}' could not be found.");
        }
    }
}
