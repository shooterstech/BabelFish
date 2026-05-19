using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {


    /// <summary>
    /// <see cref="MergeConfiguration"/> specific for a <see cref="MergedResultList"/> of type <see cref="MergeMethodType.AVERAGE"/>.
    /// </summary>
    public class AverageMethodConfiguration : MergeConfiguration {


        /// <summary>
        /// Public constructor.
        /// <para>Unless you are a deserializer, rarely would you construct an instance of AverageMethodConfiguration directly. Instead instances
        /// are creaed as part of the <see cref="MergedResultList.CreateAsync(IMergedResultListContainer, string, MergeMethodType)"/> method.</para>
        /// </summary>
        public AverageMethodConfiguration() : base() {

            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */

            this.Method = DataModel.OrionMatch.MergeMethodType.AVERAGE;
        }

        #region Data Model Properties
        /// <summary>
        /// If true, an event representing each participants high score will be included.
        /// </summary>
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public bool AddHighScoreEvent { get; set; } = false;

        /// <summary>
        /// If true, scores with a DNF are excluded when calculating a participant's average.
        /// </summary>
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public bool ExcludeDNFFromAverage { get; set; } = false;

        /// <summary>
        /// Parameter that sets how many of the top scores to count towards a participant's average.
        /// <para>A value of 0 (the default) means to count all scores.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 13, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int CountTopScores { get; set; } = 0;

        /// <summary>
        /// Parameter that sets how many scores the participant must have to be included in the average.
        /// If they have fewer scores than this number, then they will not be included in the average and will not be ranked.
        /// <para>A value of 1 (the default) means they only have to have 1 score to be included.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 14, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int RequiredNumberOfScores { get; set; } = 1;
        #endregion

        #region Methods

        public override ulong CalculateChecksum() {
            var combined = $"{Method}|{AddHighScoreEvent}|{ExcludeDNFFromAverage}|{CountTopScores}|{RequiredNumberOfScores}";
            return Helpers.Common.Md5ToUlong( combined );
        }

        #endregion 
    }
}
