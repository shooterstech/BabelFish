using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// Concrete class implementation of a MergeMethod that counts the number of scores from each participant.
    /// <para>The Count method does not emphasize scores, rather it emphasizes participation.</para>
    /// </summary>
    public class CountMethod : MergeMethod {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static string COUNT_EVENT_SCORE_NAME = "Count";

        #region Constructors, factory methods, and initializations
        /// <summary>
        /// Public constructor.
        /// </summary>
        public CountMethod( ResultListMergerEngine tournamentMerger, CountMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {
            this.TopLevelEventname = ResultEvent.KeyForResultCofScore( ResultListMergerEngine.Container.MatchId, COUNT_EVENT_SCORE_NAME );
        }

        /// <inheritdoc />
        public override async Task InitializeAsync() {
        }

        #endregion

        #region Data Model Properties
        // Should be empty as this is a Data Actor
        #endregion

        #region Helper Properties

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

        /// <inheritdoc />
        public override List<ResultListField> AdditionalFields => base.AdditionalFields;

        /// <inheritdoc />
        public override List<ResultListDisplayColumn> AdditionalDisplayColumns => base.AdditionalDisplayColumns;

        /// <inheritdoc />
        public CountMethodConfiguration MergeConfiguration {
            get {
                return (CountMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override string TopLevelHeaderText {
            get {
                return "Count";
            }
        }


        #endregion
    }
}
