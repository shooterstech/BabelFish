using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {

    /// <summary>
    /// Concrete class implementation of a MergeMethod that sums the total of a series of scores from each participant.
    /// </summary>
    public class SumMethod : MergeMethod {

        /// <summary>
        /// Each MergeMethod concrete class has a unique identifier. It is used in the serialization of MergedResultLists instances
        /// to identify how the Merged Result List should be calculated.
        /// </summary>
        public const string IDENTIFIER = "Sum";

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tournamentMerger"></param>
        /// <param name="configuration"></param>
        public SumMethod( TournamentMerger tournamentMerger, SumMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

            this.TopLevelEventname = "Aggregate";
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
        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();

            List<EventScore> listOfScores = new List<EventScore>();
            foreach (var resultListMember in TournamentMerger.ResultListsMembers) {
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

            re.ResultCofScores[ResultEvent.KeyForResultCofScore( TournamentMerger.Tournament.TournamentId, this.TopLevelEventname )] = mergedEventScore;
        }
    }
}
