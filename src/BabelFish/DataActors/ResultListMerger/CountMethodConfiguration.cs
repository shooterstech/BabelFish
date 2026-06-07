using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {
    public class CountMethodConfiguration : MergeConfiguration {

        #region Constructors, factory methods, and initializations

        public CountMethodConfiguration() : base() {

            this.Method = DataModel.OrionMatch.MergeMethodType.COUNT;
        }

        #endregion

        #region Data Model Properties
        /// <summary>
        /// Parameter that sets how order the participants.
        /// </summary>
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public CountMergeMethodSortOption SortBy { get; set; } = CountMergeMethodSortOption.COUNT;

        /// <summary>
        /// Parameter that sets how many scores the participant must have to be included in the average.
        /// If they have fewer scores than this number, then they will not be included in the ranking.
        /// <para>A value of 1 (the default) means they only have to have 1 score to be included.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 14, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int RequiredNumberOfScores { get; set; } = 1;
        #endregion

        #region Methods

        /// <inheritdoc />
        public override ulong CalculateChecksum() {
            var combined = $"{SortBy}|{RequiredNumberOfScores}";
            return Helpers.Common.Md5ToUlong( combined );
        }

        #endregion
    }
}
