using Assignment.Businesses.Models;

namespace Assignment.Businesses.Services.Abstractions;

public interface IQuestService
{
    #region Methods

    Task<int> CalculateBetPointAsync(int chipAmountBet, int playerLevel, CancellationToken cancellationToken = default);

    Task<Milestone> GetMilestoneByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<(Milestone Instance, int Index)> GetByPointAsync(int totalPoint, CancellationToken cancellationToken = default);

    Task<int> GetLastMilestoneIdAsync(CancellationToken cancellationToken = default);

    Task<int> GetMaxPointAsync(CancellationToken cancellationToken = default);

    #endregion
}