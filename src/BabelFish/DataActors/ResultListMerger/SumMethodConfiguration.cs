using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {


    /// <summary>
    /// <see cref="MergeConfiguration"/> specific for a <see cref="MergedResultList"/> of type <see cref="MergeMethodType.SUM"/>.
    /// </summary>
    public class SumMethodConfiguration : MergeConfiguration {


        /// <summary>
        /// Public constructor.
        /// <para>Unless you are a deserializer, rarely would you construct an instance of SumMethodConfiguration directly. Instead instances
        /// are creaed as part of the <see cref="MergedResultList.CreateAsync(IMergedResultListContainer, string, MergeMethodType)"/> method.</para>
        /// </summary>
        public SumMethodConfiguration() : base() {

            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */
            this.Method = DataModel.OrionMatch.MergeMethodType.SUM;
        }

        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public bool IncludeHighScoreEvent { get; set; } = false;

        /// <summary>
        /// If true, an event representing each particiants average score will be included.
        /// </summary>
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public bool IncludeAverageScoreEvent { get; set; } = false;

        /// <summary>
        /// Parameter that sets how many of the top scores to sum up towards a participant's overall aggregate.
        /// <para>A value of 0 (the default) means to sum all scores.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 13, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int CountTopScores { get; set; } = 0;
    }
}
