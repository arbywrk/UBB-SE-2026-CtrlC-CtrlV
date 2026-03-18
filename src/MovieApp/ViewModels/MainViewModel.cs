using MovieApp.Models;

namespace MovieApp.ViewModels;

public sealed class MainViewModel : ViewModelBase
{
    public MainViewModel()
        : this(new User
        {
            Id = 0,
            AuthProvider = "dummy",
            AuthSubject = "default-user",
            Username = "Dummy User",
        })
    {
    }

    public MainViewModel(User currentUser)
    {
        CurrentUser = currentUser;
    }

    public string AppTitle => "MovieApp";

    public User CurrentUser { get; }

    public string Greeting => $"{CurrentUser.Username} is ready to start";

    public string Description => $"Authenticated as {CurrentUser.StableId}";
}
