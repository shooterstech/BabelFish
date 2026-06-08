using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {
    public class ParticipationCountMethodConfiguration : MergeConfiguration {

        #region Constructors, factory methods, and initializations

        public ParticipationCountMethodConfiguration() : base() {

            this.Method = DataModel.OrionMatch.MergeMethodType.EVENT_COUNT;
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

        /// <summary>
        /// The ScoreConfigName to use, within the SCORE FORMAT COLLECTION, while displaying scores for this MergedResultList.
        /// Overrides the base ScoreConfigName to default to "ScoreFormatted", which is a special value that indicates the <see cref="EventScore.ScoreFormatted"/>
        /// value should be used instead of the ScoreFormat string defined in the ScoreConfig.
        /// <para>ScoreConfigName is overridden in this manner because it is likely that the many different COUrSES OF FIRE included in the Merged Result List
        /// members will have different score configu values. Some may want decimal, some may want integer, some may want hit / miss. By overridding the
        /// ScoreConfigName to the special use case "ScoreFormatted" we allow the COURSE OF FIRE to decide.</para>
        /// <para>Setting this value has no effect.</para>
        /// </summary>
        public override string ScoreConfigName { get => FieldSource.SCORE_CONFIG_NAME_SCORE_FORMATTED; set => base.ScoreConfigName = value; }

        /// <summary>
        /// Overridden to always return v1.0:orion:Standard Score Formats. This value is held constant because the 
        /// </summary>
        public override SetName ScoreFormatCollectionDef { get => SetName.Parse( "v1.0:orion:Standard Score Formats" ); set => base.ScoreFormatCollectionDef = value; }

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
