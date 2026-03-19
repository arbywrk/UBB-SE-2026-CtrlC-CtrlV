namespace MovieApp.Models;

public sealed class Event
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string PosterUrl { get; init; } = "";
    public required DateTime EventDateTime { get; init; }
    public required string LocationReference { get; init; }
    public required decimal TicketPrice { get; init; }

    
    public double HistoricalRating { get; init; } = 0.0;
    public required string EventType { get; init; }
    public int MaxCapacity { get; init; } = 50;
    public int CurrentEnrollment { get; init; } = 0;

    public required int CreatorUserId { get; init; }

    public int AvailableSpots => MaxCapacity - CurrentEnrollment;
}