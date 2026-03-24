using MovieApp.Core.Models;
using MovieApp.Core.Repositories;

namespace MovieApp.Ui.ViewModels.Events;

public sealed class SectionEventsViewModel(IEventRepository repository, SectionNavigationContext context)
    : EventListPageViewModel
{
    private readonly IEventRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public SectionNavigationContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));

    public override string PageTitle => Context.Title;

    protected override async Task<IReadOnlyList<Event>> LoadEventsAsync()
    {
        var allEvents = await _repository.GetAllAsync();

        return allEvents
            .Where(e => MatchesSection(e, Context.GroupingValue))
            .OrderBy(e => e.EventDateTime)
            .ToList();
    }

    private static bool MatchesSection(Event? @event, string groupingValue)
    {
        if (@event is null || string.IsNullOrWhiteSpace(@event.EventType) || string.IsNullOrWhiteSpace(groupingValue))
        {
            return false;
        }
        var normalizedGroupingValue = groupingValue.Trim();

        var eventGroupingValue = @event.EventType.Trim();

        return string.Equals(
            eventGroupingValue,
            normalizedGroupingValue,
            StringComparison.OrdinalIgnoreCase);
    }
}
