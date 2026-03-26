using MovieApp.Core.Models;
using MovieApp.Core.Repositories;
using MovieApp.Core.Services;

namespace MovieApp.Ui.ViewModels.Events;

public sealed class EventManagementViewModel : EventListPageViewModel
{
    public override string PageTitle => "Event Management";

    private readonly IEventRepository? _eventRepository;
    private readonly INotificationService? _notificationService;

    public System.Windows.Input.ICommand SimulateUpdateCommand { get; }

    public EventManagementViewModel()
    {
        _eventRepository = App.EventRepository;
        _notificationService = App.NotificationService;

        SimulateUpdateCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand(SimulateEventUpdateAsync);
    }

    private async Task SimulateEventUpdateAsync()
    {
        if (_eventRepository is null || _notificationService is null) return;

        var ev = VisibleEvents.FirstOrDefault();
        if (ev is null) return;

        // Simulate price drop
        var oldPrice = ev.TicketPrice;
        ev.TicketPrice = oldPrice > 5 ? oldPrice - 5 : 0;
        
        // Simulate seats became available
        var oldEnrollment = ev.CurrentEnrollment;
        if (ev.CurrentEnrollment > 0)
        {
            ev.CurrentEnrollment -= 1; 
        }

        await _eventRepository.UpdateEventAsync(ev);
        await _notificationService.NotifyPriceDropAsync(ev.Id, oldPrice, ev.TicketPrice);
        await _notificationService.NotifySeatsAvailableAsync(ev.Id, ev.MaxCapacity);
    }

    protected override async Task<IReadOnlyList<Event>> LoadEventsAsync()
    {
        if (_eventRepository is null)
            return [];

        var events = await _eventRepository.GetAllAsync();
        return events.ToList();
    }
}
