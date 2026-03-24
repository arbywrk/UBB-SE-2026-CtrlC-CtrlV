using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MovieApp.Ui.Navigation;
using MovieApp.Ui.ViewModels;

namespace MovieApp.Ui.Views;

/// <summary>
/// Hosts the application shell, navigation structure, and the top-level frame
/// where each requirement-driven feature page is loaded.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        NavigateToRoute(AppRouteResolver.Home);
    }

    public MainViewModel ViewModel { get; }

    public void NavigateToRoute(string tag)
    {
        var pageType = AppRouteResolver.ResolvePageType(tag);
        SyncSelectedNavigationItem(tag);

        if (ContentFrame.CurrentSourcePageType != pageType)
        {
            ContentFrame.Navigate(pageType);
        }
    }

    private void AppNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer?.Tag is string tag)
        {
            NavigateToRoute(tag);
        }
    }

    private void AlertsButton_Click(object sender, RoutedEventArgs e)
    {
        NavigateToRoute(AppRouteResolver.Notifications);
    }

    private void RewardsButton_Click(object sender, RoutedEventArgs e)
    {
        NavigateToRoute(AppRouteResolver.Rewards);
    }

    private void ReferralButton_Click(object sender, RoutedEventArgs e)
    {
        NavigateToRoute(AppRouteResolver.ReferralArea);
    }

    private void SyncSelectedNavigationItem(string tag)
    {
        var selectedItem = AppNavigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => string.Equals(item.Tag as string, tag, StringComparison.Ordinal));

        AppNavigationView.SelectedItem = selectedItem;
    }
}