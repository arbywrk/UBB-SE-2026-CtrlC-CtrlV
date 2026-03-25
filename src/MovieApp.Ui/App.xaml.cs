using Microsoft.UI.Xaml;
using Microsoft.Extensions.Configuration;
using MovieApp.Core.Repositories;
using MovieApp.Infrastructure;
using MovieApp.Core.Services;
using MovieApp.Ui.Services;
using MovieApp.Ui.ViewModels;
using MovieApp.Ui.Views;

namespace MovieApp.Ui;

public partial class App : Application
{
    private Window? _window;
    private ICurrentUserService? _currentUserService;

    public static ICurrentUserService? CurrentUserService { get; private set; }

    public static IEventRepository? EventRepository { get; private set; }
    public static ITriviaRepository? TriviaRepository { get; private set; }
    public static IAmbassadorRepository? AmbassadorRepository { get; private set; }
    public static MovieApp.Core.Services.IReferralValidator? ReferralValidator { get; private set; }
    public static MainWindow? CurrentMainWindow { get; private set; }
    public static IFavoriteEventService? FavoriteEventService { get; private set; }
    public static INotificationService? NotificationService { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainViewModel viewModel;
        EventRepository = UnavailableEventRepository.Instance;
        TriviaRepository = null;

        try
        {
            var configuration = BuildConfiguration();
            var databaseOptions = new DatabaseOptions
            {
                ConnectionString = configuration["Database:ConnectionString"]
                    ?? throw new InvalidOperationException("Missing configuration value 'Database:ConnectionString'."),
            };
            var bootstrapUserOptions = new BootstrapUserOptions
            {
                AuthProvider = configuration["Authentication:DummyUser:AuthProvider"]
                    ?? throw new InvalidOperationException("Missing configuration value 'Authentication:DummyUser:AuthProvider'."),
                AuthSubject = configuration["Authentication:DummyUser:AuthSubject"]
                    ?? throw new InvalidOperationException("Missing configuration value 'Authentication:DummyUser:AuthSubject'."),
            };

            var userRepository = new SqlUserRepository(databaseOptions);
            var eventRepository = new SqlEventRepository(databaseOptions);
            var triviaRepository = new SqlTriviaRepository(databaseOptions);
            var ambassadorRepository = new SqlAmbassadorRepository(databaseOptions);
            var favoriteEventRepository = new SqlFavoriteEventRepository(databaseOptions);
            var notificationRepository = new SqlNotificationRepository(databaseOptions);

            _currentUserService = new CurrentUserService(userRepository, bootstrapUserOptions);
            await _currentUserService.InitializeAsync();
            CurrentUserService = _currentUserService;

            EventRepository = eventRepository;
            TriviaRepository = triviaRepository;
            AmbassadorRepository = ambassadorRepository;
            ReferralValidator = new MovieApp.Core.Services.ReferralValidator(ambassadorRepository);
            
            FavoriteEventService = new FavoriteEventService(favoriteEventRepository, eventRepository);
            NotificationService = new NotificationService(notificationRepository, favoriteEventRepository);

            viewModel = new MainViewModel(_currentUserService.CurrentUser);
        }
        catch (Exception)
        {
            // Fully functional offline fallback for testing without LocalDB
            EventRepository = UnavailableEventRepository.Instance;
            var fakeFavs = new MovieApp.Ui.Services.UnavailableFavoriteEventRepository();
            var fakeNotifs = new MovieApp.Ui.Services.UnavailableNotificationRepository();
            FavoriteEventService = new FavoriteEventService(fakeFavs, EventRepository);
            NotificationService = new NotificationService(fakeNotifs, fakeFavs);
            
            var fakeUserSvc = new MovieApp.Ui.Services.FakeCurrentUserService();
            CurrentUserService = fakeUserSvc;

            viewModel = new MainViewModel(fakeUserSvc.CurrentUser!);
        }

        CurrentMainWindow = new MainWindow(viewModel);
        _window = CurrentMainWindow;
        _window.Activate();
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();
    }

    private static string BuildStartupErrorMessage(Exception exception)
    {
        return "Check the database setup scripts and the appsettings.json connection string."
            + Environment.NewLine
            + Environment.NewLine
            + exception.Message;
    }
}