using Xunit;

namespace MovieApp.Infrastructure.Tests;

public sealed class EventSqlQueriesTests
{
    [Fact]
    public void EventProjection_IncludesEventTypeColumn()
    {
        var queryFile = ReadRepoFile("src", "MovieApp.Infrastructure", "EventSqlQueries.cs");

        Assert.Contains("public const string Projection", queryFile);
        Assert.Contains("EventType", queryFile);
        Assert.Contains("WHERE Id = @id;", queryFile);
    }

    [Fact]
    public void SqlEventRepository_UsesSharedProjectionForAllSelectQueries()
    {
        var repositoryFile = ReadRepoFile("src", "MovieApp.Infrastructure", "SqlEventRepository.cs");

        Assert.Contains("new SqlCommand(EventSqlQueries.SelectAll, connection)", repositoryFile);
        Assert.Contains("new SqlCommand(EventSqlQueries.SelectByType, connection)", repositoryFile);
        Assert.Contains("new SqlCommand(EventSqlQueries.SelectById, connection)", repositoryFile);
        Assert.Contains("MapEvent", repositoryFile);
    }

    private static string ReadRepoFile(params string[] pathSegments)
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);

        while (currentDirectory is not null
            && !File.Exists(Path.Combine(currentDirectory.FullName, "MovieApp.sln")))
        {
            currentDirectory = currentDirectory.Parent;
        }

        Assert.NotNull(currentDirectory);

        var filePath = Path.Combine([currentDirectory!.FullName, .. pathSegments]);
        return File.ReadAllText(filePath);
    }
}
