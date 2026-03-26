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
<<<<<<< Updated upstream
=======
    public static IConfigurationRoot? Configuration { get; private set; }
    public static IMarathonRepository? MarathonRepository { get; private set; }
    
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
            Configuration = configuration;
            var connectionString = configuration["Database:ConnectionString"];
            
>>>>>>> Stashed changes
            var databaseOptions = new DatabaseOptions
            {
                ConnectionString = connectionString
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

            var favoriteEventRepository = new SqlFavoriteEventRepository(databaseOptions);
            var notificationRepository = new SqlNotificationRepository(databaseOptions);

            FavoriteEventService = new FavoriteEventService(favoriteEventRepository, eventRepository);
            NotificationService = new NotificationService(notificationRepository, favoriteEventRepository, eventRepository);

            viewModel = new MainViewModel(_currentUserService.CurrentUser);
        }
        catch (Exception)
        {
<<<<<<< Updated upstream
            // Fully functional offline fallback for testing without LocalDB
            EventRepository = UnavailableEventRepository.Instance;
            var fakeFavs = new MovieApp.Ui.Services.UnavailableFavoriteEventRepository();
            var fakeNotifs = new MovieApp.Ui.Services.UnavailableNotificationRepository();
            FavoriteEventService = new FavoriteEventService(fakeFavs, EventRepository);
            NotificationService = new NotificationService(fakeNotifs, fakeFavs);
            
            var fakeUserSvc = new MovieApp.Ui.Services.FakeCurrentUserService();
            CurrentUserService = fakeUserSvc;

            viewModel = new MainViewModel(fakeUserSvc.CurrentUser!);
=======
            // If LocalDB is not found, we still want to show the app in DEMO MODE as requested.
            if (exception.Message.Contains("Local Database Runtime") || exception.InnerException?.Message.Contains("Local Database Runtime") == true)
            {
                var dummyUserRepo = new DummyUserRepository();
                _currentUserService = new CurrentUserService(dummyUserRepo, new BootstrapUserOptions { AuthProvider = "dummy", AuthSubject = "default-user" });
                _currentUserService.InitializeAsync().Wait();
                CurrentUserService = _currentUserService;

                var favoriteRepo = new InMemoryFavoriteEventRepository();
                FavoriteEventService = new FavoriteEventService(favoriteRepo, UnavailableEventRepository.Instance);
                NotificationService = new NotificationService(new InMemoryNotificationRepository(), favoriteRepo, UnavailableEventRepository.Instance);

                viewModel = new MainViewModel(_currentUserService.CurrentUser);
            }
            else
            {
                viewModel = MainViewModel.CreateStartupError(BuildStartupErrorMessage(exception));
            }
>>>>>>> Stashed changes
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