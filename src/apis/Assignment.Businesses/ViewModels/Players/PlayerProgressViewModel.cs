namespace Assignment.Businesses.ViewModels.Players;

public class PlayerProgressViewModel
{
    #region Properties

    public int QuestPointsEarned { get; set; }

    public int TotalQuestPercentCompleted { get; set; }

    public MilestoneViewModel[]? MilestonesCompleted { get; set; }

    #endregion
}