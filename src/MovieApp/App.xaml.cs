using Microsoft.UI.Xaml;
using MovieApp.Services;
using MovieApp.ViewModels;
using MovieApp.Views;

namespace MovieApp;

public partial class App : Application
{
    private const string LocalSqlServerConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=MovieApp;Trusted_Connection=True;TrustServerCertificate=True;";

    private Window? _window;
    private ICurrentUserService? _currentUserService;

    public App()
    {
        InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var databaseOptions = new DatabaseOptions
        {
            ConnectionString = LocalSqlServerConnectionString,
        };

        var userRepository = new SqlUserRepository(databaseOptions);
        _currentUserService = new CurrentUserService(userRepository);
        await _currentUserService.InitializeAsync();

        var viewModel = new MainViewModel(_currentUserService.CurrentUser);

        _window = new MainWindow(viewModel);
        _window.Activate();
    }
}
