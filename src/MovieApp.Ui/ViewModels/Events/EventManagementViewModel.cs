using MovieApp.Core.Models;

namespace MovieApp.Ui.ViewModels.Events;

public sealed class EventManagementViewModel : EventListPageViewModel
{
    public override string PageTitle => "Event Management";

    protected override Task<IReadOnlyList<Event>> LoadEventsAsync()
    {
        // TODO: 7 Load the event-management list or temporarily fall back to BuildSampleEvents().
        throw new NotImplementedException();
    }

    protected override IReadOnlyList<Event> BuildSampleEvents()
    {
        // TODO: 8 Seed representative events that exercise search, sort, and filter behavior on this screen.
        throw new NotImplementedException();
    }
}
