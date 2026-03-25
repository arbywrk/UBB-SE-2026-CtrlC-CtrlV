using Microsoft.UI.Xaml.Controls;

using MovieApp.Ui.ViewModels.Events;

namespace MovieApp.Ui.Views;

/// <summary>
/// Provides the saved-event inventory surface while keeping favorite-event alerts
/// separate in their own page.
/// </summary>
public sealed partial class FavoritesPage : Page
{
    public FavoritesPage()
    {
        ViewModel = new FavoritesViewModel();
        InitializeComponent();
        DataContext = ViewModel;
        Loaded += async (s, e) => { await ViewModel.InitializeAsync(); };
    }

    public FavoritesViewModel ViewModel { get; }

    private async void RemoveFavorite_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is MovieApp.Core.Models.Event @event)
        {
            await ViewModel.RemoveFavoriteAsync(@event.Id);
        }
    }
}
