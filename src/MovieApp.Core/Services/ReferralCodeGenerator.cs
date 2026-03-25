using System;

namespace MovieApp.Core.Services;

public sealed class ReferralCodeGenerator : IReferralCodeGenerator
{
    public string Generate(string username, int userId)
    {
        var year = DateTime.UtcNow.Year;
        return $"{username.ToUpperInvariant()}{year}-{userId}";
    }
}
