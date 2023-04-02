using System.Net;
using Assignment.Apis.Constants;
using Assignment.Apis.Models;
using Assignment.Apis.Models.Exceptions;
using Assignment.Businesses.Services.Abstractions;

namespace Assignment.Apis.Services.Implementations;

public class QuestService : IQuestService
{
    #region Properties

    private readonly Quest _actualQuest;

    #endregion

    #region Constructor

    public QuestService(Quest quest)
    {
        _actualQuest = new Quest(quest);
        _actualQuest.Milestones = quest.Milestones.OrderBy(x => x.TotalPoint).ToArray();
    }

    #endregion

    #region Methods

    public virtual Task<int> CalculateBetPointAsync(int chipAmountBet, int playerLevel, CancellationToken cancellationToken = default)
    {
        var totalPoint = chipAmountBet * _actualQuest.RateFromBet +
                         playerLevel * _actualQuest.LevelBonusRate;

        return Task.FromResult(totalPoint);
    }

    public virtual Task<int> GetMaxPointAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_actualQuest.TotalPoint);
    }

    public virtual async Task<(int Milestone, int AwardedChip)> FindNearestMilestoneAsync(int totalPoint, CancellationToken cancellationToken = default)
    {
        for (var index = 0; index < _actualQuest.Milestones.Length; index++)
        {
            var milestone = _actualQuest.Milestones[index];
            if (milestone.TotalPoint < totalPoint)
                continue;

            return (index, milestone.Chips);
        }

        throw new BusinessException(HttpStatusCode.NotFound, ExceptionCodes.MilestoneNotFound);
    }

    public virtual Task<int> GetMaxMilestoneAsync(CancellationToken cancellationToken = default)
    {
        var index = _actualQuest.Milestones.Length - 1;
        if (index < 0)
            index = 0;
        return Task.FromResult(index);
    }

    public virtual async Task<int> GetMilestoneChipAsync(int milestone, CancellationToken cancellationToken = default)
    {
        if (milestone < 0 || milestone >= _actualQuest.Milestones.Length)
            throw new BusinessException(HttpStatusCode.NotFound, ExceptionCodes.MilestoneNotFound);

        return _actualQuest.Milestones[milestone].Chips;
    }

    #endregion
}