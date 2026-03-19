namespace MovieApp.Core.EventLists;

public sealed class EventFilterState
{
    public string? EventType { get; set; }

    public string? LocationReference { get; set; }

    public decimal? MinimumTicketPrice { get; set; }

    public decimal? MaximumTicketPrice { get; set; }

    public bool OnlyAvailableEvents { get; set; }

    public bool HasActiveFilters()
    {
        return !string.IsNullOrWhiteSpace(EventType)
            || !string.IsNullOrWhiteSpace(LocationReference)
            || MinimumTicketPrice is not null
            || MaximumTicketPrice is not null
            || OnlyAvailableEvents;
    }

    public EventFilterState Clone()
    {
        return new EventFilterState
        {
            EventType = EventType,
            LocationReference = LocationReference,
            MinimumTicketPrice = MinimumTicketPrice,
            MaximumTicketPrice = MaximumTicketPrice,
            OnlyAvailableEvents = OnlyAvailableEvents,
        };
    }

    public void Reset()
    {
        EventType = null;
        LocationReference = null;
        MinimumTicketPrice = null;
        MaximumTicketPrice = null;
        OnlyAvailableEvents = false;
    }
}
