using MovieApp.Core.Models;
using MovieApp.Core.Repositories;
using MovieApp.Ui.Navigation;
using System.ComponentModel;

namespace MovieApp.Ui.ViewModels.Events;

public sealed class HomeEventsViewModel : EventListPageViewModel
{
    private const string FallbackTitle = "Other events";
    private readonly IEventRepository _repository;
    private IReadOnlyList<EventSection> _sections = [];

    public HomeEventsViewModel(IEventRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        PropertyChanged += OnBasePropertyChanged;
    }

    public override string PageTitle => "Home";

    public IReadOnlyList<HomeNavigationShortcut> NavigationShortcuts { get; } =
    [
        new HomeNavigationShortcut
        {
            Title = "My Events",
            Description = "Open your personal event workspace.",
            RouteTag = AppRouteResolver.MyEvents,
        },
        new HomeNavigationShortcut
        {
            Title = "Event Management",
            Description = "Open the event administration workspace.",
            RouteTag = AppRouteResolver.EventManagement,
        },
    ];

    public IReadOnlyList<EventSection> Sections
    {
        get => _sections;
        private set => SetProperty(ref _sections, value);
    }

    protected override async Task<IReadOnlyList<Event>> LoadEventsAsync()
    {
        var allEvents = await _repository.GetAllAsync();
        return allEvents.ToList();
    }

    private void OnBasePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(VisibleEvents))
        {
            Sections = BuildSections(VisibleEvents);
        }
    }

    private static IReadOnlyList<EventSection> BuildSections(IEnumerable<Event> events)
    {
        var groups = events
            .GroupBy(
                e => string.IsNullOrWhiteSpace(e.EventType) ? FallbackTitle : e.EventType.Trim(),
                StringComparer.OrdinalIgnoreCase);

        var sections = groups
            .Select(g => new EventSection
            {
                Title = g.Key,
                Events = g.OrderBy(ev => ev.EventDateTime).ToList(),
            })
            .OrderBy(s => s.Title.Equals(FallbackTitle, StringComparison.OrdinalIgnoreCase) ? 1 : 0)
            .ThenBy(s => s.Title)
            .ToList();

        return sections;
    }
}