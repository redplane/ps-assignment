using System.Net;
using Assignment.Apis.Constants;
using Assignment.Apis.Models.Exceptions;
using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Businesses.ViewModels;
using Assignment.Businesses.ViewModels.Players;
using MediatR;

namespace Assignment.Apis.Cqrs.CommandHandlers.Players;

public class SubmitPlayerProgressCommandHandler : IRequestHandler<SubmitPlayerProgressCommand, PlayerProgressViewModel>
{
    #region Constructor

    public SubmitPlayerProgressCommandHandler(IPlayerService playerService, IQuestService questService)
    {
        _playerService = playerService;
        _questService = questService;
    }

    #endregion

    #region Methods

    public virtual async Task<PlayerProgressViewModel> Handle(SubmitPlayerProgressCommand command,
        CancellationToken cancellationToken)
    {
        // Get player info.
        var player = await _playerService.GetByIdAsync(command.PlayerId, cancellationToken);
        if (player == null)
            throw new BusinessException(HttpStatusCode.NotFound, ExceptionCodes.UserNotFound);

        // Player already completed the quest.
        var lastMilestoneId = await _questService.GetLastMilestoneIdAsync(cancellationToken);
        var lastMilestone = await _questService.GetMilestoneByIdAsync(lastMilestoneId, cancellationToken);
        var maxPoint = await _questService.GetMaxPointAsync(cancellationToken);
        if (player.TotalPoints >= maxPoint)
            throw new BusinessException(HttpStatusCode.Forbidden, ExceptionCodes.AlreadyCompletedQuest);

        // How many points can player receive ?
        var totalRewardedPoint =
            await _questService.CalculateBetPointAsync(command.ChipAmountBet, command.PlayerLevel,
                cancellationToken);

        // If player point exceeds max point of a quest, set the point to the max.
        var totalPlayerPoint = player.TotalPoints + totalRewardedPoint;
        var totalPlayerChip = player.TotalChips;
        int completedPercentage;
        int nextMilestoneId;
        var completedMilestones = new LinkedList<MilestoneViewModel>();
        
        if (totalPlayerPoint >= maxPoint)
        {
            totalPlayerPoint = maxPoint;
            nextMilestoneId = lastMilestoneId;
            completedPercentage = 100;
        }
        else
        {
            completedPercentage = (int) Math.Round((decimal) totalPlayerPoint * 100 / maxPoint,
                MidpointRounding.AwayFromZero);
            try
            {
                var targetMilestone = await _questService.GetByPointAsync(totalPlayerPoint, cancellationToken);
                nextMilestoneId = targetMilestone.Index;
            }
            catch
            {
                nextMilestoneId = -1;
            }
        }

        // Enough point to go to the next milestone.
        if (player.CurrentMilestone < nextMilestoneId)
        {
            // Sum all the chips from milestones that player archived.
            for (var milestoneId = player.CurrentMilestone + 1; milestoneId <= nextMilestoneId; milestoneId++)
            {
                var milestone = await _questService.GetMilestoneByIdAsync(milestoneId, cancellationToken);
                totalPlayerChip += milestone.Chips;
                completedMilestones.AddLast(new MilestoneViewModel
                {
                    MilestoneIndex = milestoneId,
                    ChipsAwarded = milestone.Chips
                });
            }
        }
        
        await _playerService.UpdateAsync(player.Id, totalPlayerPoint, nextMilestoneId, totalPlayerChip,
            cancellationToken);
        return new PlayerProgressViewModel
        {
            TotalQuestPercentCompleted = completedPercentage,
            QuestPointsEarned = totalRewardedPoint,
            MilestonesCompleted = completedMilestones.ToArray()
        };
    }

    #endregion

    #region Properties

    private readonly IPlayerService _playerService;

    private readonly IQuestService _questService;

    #endregion
}