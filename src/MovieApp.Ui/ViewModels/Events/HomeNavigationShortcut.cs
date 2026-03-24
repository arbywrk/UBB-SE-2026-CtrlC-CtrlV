namespace MovieApp.Ui.ViewModels.Events;

public sealed class HomeNavigationShortcut
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string RouteTag { get; init; }
}