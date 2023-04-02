namespace Assignment.Businesses.Models
{
    public class Milestone
    {
        #region Properties

        /// <summary>
        /// Point which will be rewarded to user when milestone is completed.
        /// </summary>
        public int TotalPoint { get; set; }

        /// <summary>
        /// Chip which will be rewarded to user when milestone is completed.
        /// </summary>
        public int Chips { get; set; }

        #endregion
    }
}
