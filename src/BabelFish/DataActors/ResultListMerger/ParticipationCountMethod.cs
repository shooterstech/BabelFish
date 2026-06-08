using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// Concrete class implementation of a MergeMethod that counts the number of scores from each participant.
    /// <para>The Count method does not emphasize scores, rather it emphasizes participation.</para>
    /// </summary>
    public class ParticipationCountMethod : MergeMethod {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static string COUNT_EVENT_SCORE_NAME = "Event Count";

        #region Constructors, factory methods, and initializations
        /// <summary>
        /// Public constructor.
        /// </summary>
        public ParticipationCountMethod( ResultListMergerEngine tournamentMerger, ParticipationCountMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {
            this.TopLevelEventname = ResultEvent.KeyForResultCofScore( ResultListMergerEngine.Container.MatchId, COUNT_EVENT_SCORE_NAME );
            this.CanGenerateRankingRule = true;
        }

        /// <inheritdoc />
        public override async Task InitializeAsync() {
        }

        #endregion

        #region Data Model Properties
        // Should be empty as this is a Data Actor
        #endregion

        #region Helper Properties

        /// <inheritdoc />
        public override List<ResultListField> AdditionalFields => base.AdditionalFields;

        /// <inheritdoc />
        public override List<ResultListDisplayColumn> AdditionalDisplayColumns => base.AdditionalDisplayColumns;

        /// <inheritdoc />
        public ParticipationCountMethodConfiguration MergeConfiguration {
            get {
                return (ParticipationCountMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override string TopLevelHeaderText {
            get {
                return "Event Count";
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override bool Merge( ResultEvent re ) {
            EventScore countEventScore = new EventScore();
            int count = 0;
            foreach (var resultListMember in ResultListMergerEngine.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );
                if (re.ResultCofScores.TryGetValue( key, out EventScore eventScore ) && eventScore.Score != null) {
                    count++;
                }
            }
            countEventScore.Score.I = count;
            re.ResultCofScores[TopLevelEventname] = countEventScore;
            return true;
        }

        public override RankingRule GenerateRankingRule() {
            var rr = new RankingRule();
            rr.SetDefaultValues();
            var rankingRule = rr.RankingRules[0];
            rankingRule.Rules.Clear();

            switch (this.MergeConfiguration.SortBy) {
                case CountMergeMethodSortOption.ALPHABETICAL:

                    rankingRule.Rules.Add( new TieBreakingRuleParticipantAttribute() {
                        SortOrder = SortBy.ASCENDING,
                        Source = TieBreakingRuleParticipantAttributeSource.FamilyName
                    } );

                    rankingRule.Rules.Add( new TieBreakingRuleParticipantAttribute() {
                        SortOrder = SortBy.ASCENDING,
                        Source = TieBreakingRuleParticipantAttributeSource.GivenName
                    } );

                    rankingRule.Rules.Add( new TieBreakingRuleParticipantAttribute() {
                        SortOrder = SortBy.ASCENDING,
                        Source = TieBreakingRuleParticipantAttributeSource.DisplayName
                    } );
                    break;

                case CountMergeMethodSortOption.COUNT:
                    rankingRule.Rules.Add( new TieBreakingRuleScore() {
                        SortOrder = SortBy.DESCENDING,
                        Method = TieBreakingRuleMethod.SCORE,
                        EventName = COUNT_EVENT_SCORE_NAME
                    } );

                    rankingRule.Rules.Add( new TieBreakingRuleParticipantAttribute() {
                        SortOrder = SortBy.ASCENDING,
                        Source = TieBreakingRuleParticipantAttributeSource.FamilyName
                    } );

                    rankingRule.Rules.Add( new TieBreakingRuleParticipantAttribute() {
                        SortOrder = SortBy.ASCENDING,
                        Source = TieBreakingRuleParticipantAttributeSource.GivenName
                    } );
                    break;

                default:
                    throw new NotImplementedException( $"The sort option {this.MergeConfiguration.SortBy} is not implemented for the {nameof( ParticipationCountMethod )}." );
            }

            return rr;
        }


        #endregion
    }
}
