namespace Assignment.Businesses.Models
{
    public class Quest
    {
        #region Properties

        /// <summary>
        ///  A value derived from your configuration
        /// </summary>
        public int RateFromBet { get; set; }

        /// <summary>
        /// Value derived from your configuration
        /// </summary>
        public int LevelBonusRate { get; set; }

        /// <summary>
        /// The total quest points needed to complete a quest
        /// </summary>
        public int TotalPoint { get; set; }

        /// <summary>
        /// Quests can have n amount of milestones
        /// </summary>
        public Milestone[] Milestones { get; set; }

        #endregion

        #region Constructor

        public Quest()
        {
            Milestones = Array.Empty<Milestone>();
        }

        public Quest(Quest quest) : this()
        {
            RateFromBet = quest.RateFromBet;
            LevelBonusRate = quest.LevelBonusRate;
            TotalPoint = quest.TotalPoint;
            Milestones = quest.Milestones;
        }

        #endregion
    }
}
