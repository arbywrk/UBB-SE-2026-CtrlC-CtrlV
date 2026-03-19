using MovieApp.Core.EventLists;
using MovieApp.Core.Models;

namespace MovieApp.Ui.ViewModels.Events;

public abstract class EventListPageViewModel : ViewModelBase
{
    private IReadOnlyList<Event> _allEvents = [];
    private IReadOnlyList<Event> _visibleEvents = [];

    protected EventListPageViewModel()
    {
        ListState = new EventListState();
    }

    public abstract string PageTitle { get; }

    public EventListState ListState { get; }

    public IReadOnlyList<EventSortOption> AvailableSortOptions => ListState.AvailableSortOptions;

    public IReadOnlyList<Event> AllEvents
    {
        get => _allEvents;
        protected set => SetProperty(ref _allEvents, value);
    }

    public IReadOnlyList<Event> VisibleEvents
    {
        get => _visibleEvents;
        protected set => SetProperty(ref _visibleEvents, value);
    }

    public Task InitializeAsync()
    {
        // TODO: 6 Load this screen's events, assign AllEvents, and call RefreshVisibleEvents.
        throw new NotImplementedException();
    }

    public void SetSearchText(string? searchText)
    {
        // TODO: 2 Copy the incoming value into ListState.SearchText and refresh VisibleEvents.
        throw new NotImplementedException();
    }

    public void SetSortOption(EventSortOption sortOption)
    {
        // TODO: 3 Update ListState.SelectedSortOption and refresh VisibleEvents.
        throw new NotImplementedException();
    }

    public void UpdateFilters(Action<EventFilterState> updateFilters)
    {
        // TODO: 4 Apply the provided filter mutation to ListState.ActiveFilters and refresh VisibleEvents.
        throw new NotImplementedException();
    }

    public void ResetListState()
    {
        // TODO: 5 Reset the per-screen state and refresh VisibleEvents.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Rebuild the visible event list by applying the current search, filter,
    /// and sort state to <see cref="AllEvents"/>
    /// </summary>
    /// <remarks>
    /// This method must be called after any change to <see cref="ListState"/>
    /// so that <see cref="VisibleEvents"/> raises a property change notification
    /// and the UI can refresh.
    /// </remarks>
    public void RefreshVisibleEvents()
    {
        VisibleEvents = EventListTransformer.Apply(AllEvents, ListState);
    }

    protected abstract Task<IReadOnlyList<Event>> LoadEventsAsync();

    protected virtual IReadOnlyList<Event> BuildSampleEvents()
    {
        // TODO: Return sample events for this screen while the real data source is not wired up yet.
        throw new NotImplementedException();
    }
}
