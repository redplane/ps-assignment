using System.Net;
using Assignment.Apis.Constants;
using Assignment.Apis.Models;
using Assignment.Apis.Models.Exceptions;
using Assignment.Businesses.Models;
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

    public virtual Task<Milestone> GetMilestoneByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0 || id >= _actualQuest.Milestones.Length)
            throw new BusinessException(HttpStatusCode.NotFound, ExceptionCodes.MilestoneNotFound);

        var milestone = _actualQuest.Milestones[id];
        return Task.FromResult(milestone);
    }

    public virtual Task<(Milestone Instance, int Index)> GetByPointAsync(int totalPoint, CancellationToken cancellationToken = default)
    {
        var lastIndex = -1;
        for (var index = 0; index < _actualQuest.Milestones.Length; index++)
        {
            var instance = _actualQuest.Milestones[index];
            if (instance.TotalPoint > totalPoint)
                break;

            lastIndex = index;
        }
        
        if (lastIndex < 0)
            throw new BusinessException(HttpStatusCode.NotFound, ExceptionCodes.MilestoneNotFound);

        return Task.FromResult<(Milestone, int)>((_actualQuest.Milestones[lastIndex], lastIndex));
    }

    public virtual Task<int> GetLastMilestoneIdAsync(CancellationToken cancellationToken = default)
    {
        var index = _actualQuest.Milestones.Length - 1;
        if (index < 0)
            index = 0;
        return Task.FromResult(index);
    }
    

    #endregion
}