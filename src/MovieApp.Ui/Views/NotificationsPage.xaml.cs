using Microsoft.UI.Xaml.Controls;

using MovieApp.Ui.ViewModels.Events;

namespace MovieApp.Ui.Views;

/// <summary>
/// Hosts persisted favorite-event notifications and the related deduplication
/// and event-linkage surfaces.
/// </summary>
public sealed partial class NotificationsPage : Page
{
    public NotificationsPage()
    {
        ViewModel = new NotificationsViewModel();
        InitializeComponent();
        DataContext = ViewModel;
        Loaded += async (s, e) => { await ViewModel.InitializeAsync(); };
    }

    public NotificationsViewModel ViewModel { get; }

    private async void DismissNotification_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is MovieApp.Core.Models.Notification notification)
        {
            await ViewModel.RemoveNotificationAsync(notification.Id);
        }
    }
}
