namespace Assignment.Cores.Models.Entities
{
    public class Player
    {
        #region Properties

        public Guid Id { get; private set; }

        /// <summary>
        /// Total points a player has.
        /// </summary>
        public int TotalPoints { get; set; }

        public int CurrentMilestone { get; set; }

        public int TotalChips { get; set; }

        #endregion

        #region Constructor

        public Player(Guid id)
        {
            Id = id;
        }

        #endregion
    }
}
