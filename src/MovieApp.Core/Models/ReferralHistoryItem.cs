using System;

namespace MovieApp.Core.Models;

public record ReferralHistoryItem(
    string FriendName,
    string EventTitle,
    DateTime UsedAt)
{
    public string FormattedDate => UsedAt.ToString("g");
}
