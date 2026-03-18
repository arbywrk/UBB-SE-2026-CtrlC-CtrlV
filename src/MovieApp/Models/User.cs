namespace MovieApp.Models;

public sealed class User
{
    public required int Id { get; init; }

    public required string AuthProvider { get; init; }

    public required string AuthSubject { get; init; }

    public required string Username { get; init; }

    public string StableId => $"{AuthProvider}:{AuthSubject}";
}
