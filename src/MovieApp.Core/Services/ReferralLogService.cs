using MovieApp.Core.Repositories;

namespace MovieApp.Core.Services;

public sealed class ReferralLogService : IReferralLogService
{
    private readonly IAmbassadorRepository _ambassadorRepository;

    public ReferralLogService(IAmbassadorRepository ambassadorRepository)
    {
        _ambassadorRepository = ambassadorRepository;
    }

    public async Task LogReferralUsageAsync(string referralCode, int friendId, int eventId, CancellationToken cancellationToken = default)
    {
        var ambassadorId = await _ambassadorRepository.GetUserIdByReferralCodeAsync(referralCode, cancellationToken);
        if (ambassadorId.HasValue)
        {
            await _ambassadorRepository.AddReferralLogAsync(ambassadorId.Value, friendId, eventId, cancellationToken);
            await _ambassadorRepository.TryApplyRewardAsync(ambassadorId.Value, cancellationToken);
        }
    }
}
