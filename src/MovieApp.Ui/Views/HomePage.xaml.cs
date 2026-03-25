using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MovieApp.Core.Models;
using MovieApp.Core.Repositories;
using MovieApp.Ui.Controls;
using MovieApp.Ui.Navigation;
using MovieApp.Ui.Services;
using MovieApp.Ui.ViewModels.Events;

namespace MovieApp.Ui.Views;

/// <summary>
/// Hosts the discovery-first home experience, including horizontal event sections
/// and launch points into the other requirement-driven feature areas.
/// </summary>
public sealed partial class HomePage : Page
{
    private bool _initialized;

    public HomeEventsViewModel ViewModel { get; }

    /// <summary>
    /// Returns the repository used to populate the home event rows.
    /// </summary>
    private static IEventRepository GetEventRepository()
        => App.EventRepository ?? UnavailableEventRepository.Instance;

    public HomePage()
    {
        ViewModel = new HomeEventsViewModel(GetEventRepository());
        NavigationCacheMode = NavigationCacheMode.Required;
        InitializeComponent();
        DataContext = ViewModel;
        Loaded += HomePage_Loaded;
    }

    /// <summary>
    /// Initializes the page's event data once after the visual tree is loaded.
    /// </summary>
    private async void HomePage_Loaded(object sender, RoutedEventArgs e)
    {
        if (_initialized) return;
        _initialized = true;

        if (App.AmbassadorRepository is not null && App.CurrentUserService?.CurrentUser is { } currentUser)
        {
            var existingCode = await App.AmbassadorRepository.GetReferralCodeAsync(currentUser.Id);
            if (string.IsNullOrEmpty(existingCode))
            {
                var generator = new MovieApp.Core.Services.ReferralCodeGenerator();
                var newCode = generator.Generate(currentUser.Username, currentUser.Id);
                await App.AmbassadorRepository.CreateAmbassadorProfileAsync(currentUser.Id, newCode);
            }
        }

        await ViewModel.InitializeAsync();
    }

    private void ShortcutButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.DataContext is not HomeNavigationShortcut shortcut)
        {
            return;
        }

        if (App.CurrentMainWindow is not null)
        {
            App.CurrentMainWindow.NavigateToRoute(shortcut.RouteTag);
            return;
        }

        Frame.Navigate(AppRouteResolver.ResolvePageType(shortcut.RouteTag));
    }

    private async void EventCardButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.DataContext is not Event selectedEvent)
        {
            return;
        }

        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
            Title = selectedEvent.Title,
            PrimaryButtonText = "Close",
            DefaultButton = ContentDialogButton.Primary,
            Content = BuildEventDialogContent(selectedEvent),
        };

        await dialog.ShowAsync();
    }

    private static UIElement BuildEventDialogContent(Event selectedEvent)
    {
        var layout = new StackPanel
        {
            Spacing = 12,
        };

        layout.Children.Add(new TextBlock
        {
            Text = selectedEvent.Description,
            TextWrapping = TextWrapping.WrapWholeWords,
        });

        layout.Children.Add(new TextBlock
        {
            Text = $"When: {selectedEvent.EventDateTime:g}",
        });

        layout.Children.Add(new TextBlock
        {
            Text = $"Where: {selectedEvent.LocationReference}",
        });

        layout.Children.Add(new TextBlock
        {
            Text = $"Price: {EventCard.GetPriceText(selectedEvent, System.Globalization.CultureInfo.CurrentCulture)}",
        });

        layout.Children.Add(new TextBlock
        {
            Text = $"Rating: {EventCard.GetRatingText(selectedEvent)}",
        });

        layout.Children.Add(new TextBlock
        {
            Text = $"Seats: {EventCard.GetCapacityText(selectedEvent)}",
        });

        layout.Children.Add(new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            Children =
            {
                new Button { Content = "Will attend" },
                new Button { Content = "Buy ticket" },
                new Button { Content = "Favorite" },
                new Button { Content = "Seat guide" },
            },
        });

        return layout;
    }
}