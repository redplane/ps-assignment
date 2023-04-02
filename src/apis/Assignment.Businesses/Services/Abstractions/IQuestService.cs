namespace Assignment.Businesses.Services.Abstractions;

public interface IQuestService
{
    #region Methods

    Task<int> CalculateBetPointAsync(int chipAmountBet, int playerLevel, CancellationToken cancellationToken = default);

    Task<int> GetMaxPointAsync(CancellationToken cancellationToken = default);

    Task<(int Milestone, int AwardedChip)> FindNearestMilestoneAsync(int totalPoint, CancellationToken cancellationToken = default);

    Task<int> GetMaxMilestoneAsync(CancellationToken cancellationToken = default);

    Task<int> GetMilestoneChipAsync(int milestone, CancellationToken cancellationToken = default);

    #endregion
}