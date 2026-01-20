namespace Scopos.BabelFish.DataActors.Tournaments {

    /// <summary>
    /// Concrete MergeConfiguration class for the AverageMethod result list merger.
    /// </summary>
    public class AverageMethodConfiguration : MergeConfiguration {

        /// <summary>
        /// Constructor
        /// </summary>
        public AverageMethodConfiguration() : base() {

            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */

            this.Method = "Average";
        }

        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        public bool AddHighScoreEvent { get; set; } = false;

        /// <summary>
        /// If true, scores with a DNF are excluded when calculating a participant's average.
        /// </summary>
        public bool ExcludeDNFFromAverage { get; set; } = false;

        /// <summary>
        /// Parameter that sets how many of the top scores to count towards a participant's average.
        /// <para>A value of 0 (the default) means to count all scores.</para>
        /// </summary>
        public int CountTopScores { get; set; } = 0;
    }
}
