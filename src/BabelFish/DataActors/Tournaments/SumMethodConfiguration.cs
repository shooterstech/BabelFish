namespace Scopos.BabelFish.DataActors.Tournaments {

    /// <summary>
    /// Concrete MergeConfiguration class for the SumMethod result list merger.
    /// </summary>
    public class SumMethodConfiguration : MergeConfiguration {

        /// <summary>
        /// Constructor
        /// </summary>
        public SumMethodConfiguration() : base() {

            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */
            this.Method = "Sum";
        }

        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        public bool IncludeHighScoreEvent { get; set; } = false;

        /// <summary>
        /// If true, an event representing each particiants average score will be included.
        /// </summary>
        public bool IncludeAverageScoreEvent { get; set; } = false;

        /// <summary>
        /// Parameter that sets how many of the top scores to sum up towards a participant's overall aggregate.
        /// <para>A value of 0 (the default) means to sum all scores.</para>
        /// </summary>
        public int CountTopScores { get; set; } = 0;
    }
}
