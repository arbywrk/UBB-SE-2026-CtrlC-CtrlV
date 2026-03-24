using MovieApp.Core.Models;
using MovieApp.Core.Repositories;
using System.ComponentModel;

namespace MovieApp.Ui.ViewModels.Events;

public sealed class HomeEventsViewModel : EventListPageViewModel
{
    public const string FallbackTitle = "Other events";

    private readonly IEventRepository _repository;
    private IReadOnlyList<EventSection> _sections = [];

    public HomeEventsViewModel(IEventRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        PropertyChanged += OnBasePropertyChanged;
    }

    public override string PageTitle => "Home";

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

    public SectionNavigationContext CreateNavigationContext(EventSection section)
    {
        ArgumentNullException.ThrowIfNull(section);

        return new SectionNavigationContext
        {
            Title = section.Title,
            GroupingValue = section.GroupingValue,
        };
    }

    internal static string NormalizeGroupingValue(Event? @event)
    {
        return string.IsNullOrWhiteSpace(@event?.EventType)
            ? FallbackTitle
            : @event.EventType.Trim();
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
            .GroupBy(NormalizeGroupingValue, StringComparer.OrdinalIgnoreCase);

        var sections = groups
            .Select(g => new EventSection
            {
                Title = g.Key,
                GroupingValue = g.Key,
                Events = g.OrderBy(ev => ev.EventDateTime).ToList(),
            })
            .OrderBy(s => s.Title.Equals(FallbackTitle, StringComparison.OrdinalIgnoreCase) ? 1 : 0)
            .ThenBy(s => s.Title)
            .ToList();

        return sections;
    }
}