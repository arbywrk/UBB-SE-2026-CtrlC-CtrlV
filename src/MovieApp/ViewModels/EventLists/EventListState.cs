namespace MovieApp.ViewModels.EventLists;

public sealed class EventListState
{
    public string SearchText { get; set; } = string.Empty;

    public EventSortOption SelectedSortOption { get; set; } = EventSortOption.DateAscending;

    public EventFilterState ActiveFilters { get; set; } = new();

    public IReadOnlyList<EventSortOption> AvailableSortOptions { get; } =
    [
        EventSortOption.DateAscending,
        EventSortOption.DateDescending,
        EventSortOption.PriceAscending,
        EventSortOption.PriceDescending,
        EventSortOption.RatingDescending,
        EventSortOption.TitleAscending,
    ];

    public EventListState Clone()
    {
        return new EventListState()
        {
            SearchText = SearchText,
            SelectedSortOption = SelectedSortOption,
            ActiveFilters = ActiveFilters.Clone(),
        };
    }

    public void Reset()
    {
        SearchText = string.Empty;
        SelectedSortOption = EventSortOption.DateAscending;
        ActiveFilters.Reset();
    }
}
