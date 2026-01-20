using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class AverageMethod : MergeMethod {

        /// <summary>
        /// Each MergeMethod concrete class has a unique identifier. It is used in the serialization of MergedResultLists instances
        /// to identify how the Merged Result List should be calculated.
        /// </summary>
        public const string IDENTIFIER = "Average";

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AverageMethod( TournamentMerger tournamentMerger, AverageMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

            this.TopLevelEventname = "Average";
        }

        /// <inheritdoc />
        public override async Task InitializeAsync() {

        }


        /// <summary>
        /// Gets the MergeConfiguration instance in use.
        /// </summary>
        public AverageMethodConfiguration MergeConfiguration {
            get {
                return (AverageMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();
            int count = 0;

            List<EventScore> listOfScores = new List<EventScore>();
            foreach (var resultListMember in TournamentMerger.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );

                if (re.ResultCofScores.TryGetValue( key, out EventScore eventScore )) {

                    if (eventScore.Score != null && !eventScore.Score.IsZero) {
                        //Don't include this score if it is a DNF and the configureation says not to use DNFs
                        if (MergeConfiguration.ExcludeDNFFromAverage
                            && eventScore.Participant.RemarkList.IsShowingParticipantRemark( ParticipantRemark.DNF )) {
                            continue;
                        }

                        listOfScores.Add( eventScore );
                    }
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
                count++;

                if (eventScore.Projected != null) {
                    if (eventScore.Projected.IsZero) {
                        mergedEventScore.Projected += eventScore.Score;
                    } else {
                        mergedEventScore.Projected += eventScore.Projected;
                    }
                }
            }

            //Calculate the average
            mergedEventScore.Score /= count;
            mergedEventScore.Projected /= count;

            re.ResultCofScores[ResultEvent.KeyForResultCofScore( TournamentMerger.Tournament.TournamentId, this.TopLevelEventname )] = mergedEventScore;
        }
    }
}
