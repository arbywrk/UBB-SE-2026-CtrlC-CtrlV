using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MovieApp.Ui.ViewModels;

namespace MovieApp.Ui.Views;

/// <summary>
/// Hosts the slot-machine game surface, its spin economy, matching results,
/// and jackpot-reward plug-in regions.
/// </summary>
public sealed partial class SlotMachinePage : Page
{
    public SlotMachinePage()
    {
        InitializeComponent();
        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= OnPageLoaded;

        var currentUser = App.CurrentUserService?.CurrentUser;
        if (currentUser is null)
            return;

        var viewModel = new SlotMachineViewModel(
            currentUser.Id,
            App.SlotMachineService ?? throw new InvalidOperationException("SlotMachineService not initialized"),
            App.SlotMachineResultService ?? throw new InvalidOperationException("SlotMachineResultService not initialized"),
            App.ReelAnimationService ?? throw new InvalidOperationException("ReelAnimationService not initialized"));

        DataContext = viewModel;
        await viewModel.InitializeAsync();
    }
}

