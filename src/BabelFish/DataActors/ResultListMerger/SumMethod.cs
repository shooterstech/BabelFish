using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// Concrete class implementation of a MergeMethod that sums the total of a series of scores from each participant.
    /// </summary>
    public class SumMethod : MergeMethod {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tournamentMerger"></param>
        /// <param name="configuration"></param>
        public SumMethod( ResultListMergerEngine tournamentMerger, SumMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

            this.TopLevelEventname = ResultEvent.KeyForResultCofScore( ResultListMergerEngine.Container.MatchId, "Aggregate" );
        }

        /// <inheritdoc />
        public override async Task InitializeAsync() {
        }

        /// <summary>
        /// Gets the MergeConfiguration instance in use.
        /// </summary>
        public SumMethodConfiguration MergeConfiguration {
            get {
                return (SumMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override bool Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();

            List<EventScore> listOfScores = new List<EventScore>();
            foreach (var resultListMember in ResultListMergerEngine.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );

                if (re.ResultCofScores.TryGetValue( key, out EventScore eventScore ) && eventScore.Score != null) {

                    listOfScores.Add( eventScore );

                }
            }

            //Order the listOfScores
            CompareEventScore comparer = new CompareEventScore( MergeConfiguration.ScoreFormatCollectionDef, MergeConfiguration.ScoreConfigName );
            listOfScores.Sort( comparer );

            //Take the top n number of scores according to MergeConfiguration (if it even specified a value).
            int takeTheseNumberOfScores = listOfScores.Count;
            if (MergeConfiguration.CountTopScores > 0 && takeTheseNumberOfScores > MergeConfiguration.CountTopScores)
                takeTheseNumberOfScores = MergeConfiguration.CountTopScores;

            //Sum the scores
            for (int i = 0; i < takeTheseNumberOfScores; i++) {
                var eventScore = listOfScores[i];
                mergedEventScore.Score += eventScore.Score;

                if (eventScore.Projected != null) {
                    if (eventScore.Projected.IsZero) {
                        mergedEventScore.Projected += eventScore.Score;
                    } else {
                        mergedEventScore.Projected += eventScore.Projected;
                    }
                }
            }

            re.ResultCofScores[this.TopLevelEventname] = mergedEventScore;

            //return true to indicate this ResultEvent should be included with the final merged Result List
            return true;
        }
    }
}
