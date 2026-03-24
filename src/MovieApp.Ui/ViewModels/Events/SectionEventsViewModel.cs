using MovieApp.Core.Models;
using MovieApp.Core.Repositories;

namespace MovieApp.Ui.ViewModels.Events;

public sealed class SectionEventsViewModel : EventListPageViewModel
{
    private readonly IEventRepository _repository;

    public SectionEventsViewModel(IEventRepository repository, SectionNavigationContext context)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public SectionNavigationContext Context { get; }

    public override string PageTitle => Context.Title;

    protected override async Task<IReadOnlyList<Event>> LoadEventsAsync()
    {
        var allEvents = await _repository.GetAllAsync();

        return allEvents
            .Where(e => MatchesSection(e, Context.GroupingValue))
            .OrderBy(e => e.EventDateTime)
            .ToList();
    }

    internal static bool MatchesSection(Event? @event, string groupingValue)
    {
        var normalizedGroupingValue = string.IsNullOrWhiteSpace(groupingValue)
            ? HomeEventsViewModel.FallbackTitle
            : groupingValue.Trim();

        var eventGroupingValue = HomeEventsViewModel.NormalizeGroupingValue(@event);

        return string.Equals(
            eventGroupingValue,
            normalizedGroupingValue,
            StringComparison.OrdinalIgnoreCase);
    }
}