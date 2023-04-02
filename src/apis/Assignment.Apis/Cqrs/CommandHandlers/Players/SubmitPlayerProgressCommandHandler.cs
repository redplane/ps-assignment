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
        var maxQuestPoint = await _questService.GetMaxPointAsync(cancellationToken);
        var maxMilestone = await _questService.GetMaxMilestoneAsync(cancellationToken);
        if (player.TotalPoints > maxQuestPoint || player.CurrentMilestone >= maxMilestone)
            throw new BusinessException(HttpStatusCode.Forbidden, ExceptionCodes.AlreadyCompletedQuest);

        // How many points can player receive ?
        var totalRewardedPoint =
            await _questService.CalculateBetPointAsync(command.ChipAmountBet, command.PlayerLevel,
                cancellationToken);

        // If player point exceeds max point of a quest, set the point to the max.
        var totalPlayerPoint = player.TotalPoints + totalRewardedPoint;
        var totalPlayerChip = player.TotalChips;
        var completedPercentage = 0;
        var nextMilestone = player.CurrentMilestone;
        var completedMilestones = new LinkedList<MilestoneViewModel>();

        if (totalPlayerPoint > maxQuestPoint)
        {
            totalPlayerPoint = maxQuestPoint;
            nextMilestone = await _questService.GetMaxMilestoneAsync(cancellationToken);
            completedPercentage = 100;
        }
        else
        {
            completedPercentage = (int) Math.Round((decimal) totalRewardedPoint * 100 / maxQuestPoint,
                MidpointRounding.AwayFromZero);
            var nearestMilestone = await _questService.FindNearestMilestoneAsync(totalPlayerPoint, cancellationToken);
            if (nextMilestone < player.CurrentMilestone)
                throw new BusinessException(HttpStatusCode.InternalServerError, ExceptionCodes.InvalidNewMilestone);
            nextMilestone = nearestMilestone.Milestone;
        }

        // Sum all the chips from milestones that player archived.
        for (var milestone = player.CurrentMilestone + 1; milestone <= nextMilestone; milestone++)
        {
            var awardedChip = await _questService.GetMilestoneChipAsync(milestone, cancellationToken);
            totalPlayerChip += awardedChip;
            completedMilestones.AddLast(new MilestoneViewModel
            {
                MilestoneIndex = milestone,
                ChipsAwarded = awardedChip
            });
        }

        await _playerService.UpdateAsync(player.Id, totalPlayerPoint, nextMilestone, totalPlayerChip,
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