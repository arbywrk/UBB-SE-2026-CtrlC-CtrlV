using MovieApp.Ui.Navigation;
using MovieApp.Ui.Views;
using Xunit;

namespace MovieApp.Ui.Tests;

public sealed class AppRouteResolverTests
{
    [Fact]
    public void ResolvePageType_ReturnsMyEventsPage_ForMyEventsTag()
    {
        var result = AppRouteResolver.ResolvePageType(AppRouteResolver.MyEvents);

        Assert.Equal(typeof(MyEventsPage), result);
    }

    [Fact]
    public void ResolvePageType_ReturnsEventManagementPage_ForEventManagementTag()
    {
        var result = AppRouteResolver.ResolvePageType(AppRouteResolver.EventManagement);

        Assert.Equal(typeof(EventManagementPage), result);
    }

    [Fact]
    public void ResolvePageType_ReturnsHomePage_ForUnknownTag()
    {
        var result = AppRouteResolver.ResolvePageType("UnknownRoute");

        Assert.Equal(typeof(HomePage), result);
    }
}