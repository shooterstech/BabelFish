using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {
    public class AverageMethod : MergeMethod {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static string HIGH_EVENT_SCORE_NAME = "High Score";

        public AverageMethod( ResultListMergerEngine tournamentMerger, AverageMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

            this.TopLevelEventname = ResultEvent.KeyForResultCofScore( ResultListMergerEngine.Container.MatchId, "Average" );
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
        public override bool Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();
            int count = 0;

            List<EventScore> listOfScores = new List<EventScore>();
            foreach (var resultListMember in ResultListMergerEngine.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );

                if (re.ResultCofScores.TryGetValue( key, out EventScore eventScore )) {

                    if (eventScore.Score != null && !eventScore.Score.IsZero) {
                        //Don't include this score if it is a DNF and the configureation says not to use DNFs
                        if (MergeConfiguration.ExcludeDNFFromAverage
                            && re.RemarkList.IsShowingParticipantRemark( ParticipantRemark.DNF )) {
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

            re.ResultCofScores[this.TopLevelEventname] = mergedEventScore;
            if (MergeConfiguration.AddHighScoreEvent && listOfScores.Count > 0 && !listOfScores[0].Score.IsZero) {
                re.EventScores[ResultEvent.KeyForResultCofScore( ResultListMergerEngine.Container.MatchId, HIGH_EVENT_SCORE_NAME )] = listOfScores[0];
            }

            // Return a value indicating if this ResultEvent should be included with the final merged Result List
            return count >= MergeConfiguration.RequiredNumberOfScores;
        }


        /// <inheritdoc />
        public override List<ResultListField> AdditionalFields {
            get {
                List<ResultListField> additionalFields = base.AdditionalFields;
                if (MergeConfiguration.AddHighScoreEvent) {
                    additionalFields.Add( new ResultListField() {
                        FieldName = AverageMethod.HIGH_EVENT_SCORE_NAME,
                        Method = ResultFieldMethod.SCORE,
                        Source = new FieldSource() {
                            Name = ResultEvent.KeyForResultCofScore( ResultListMergerEngine.Container.MatchId, HIGH_EVENT_SCORE_NAME ),
                            ScoreFormat = "Events"
                        }
                    } );
                }
                return additionalFields;
            }
        }

        /// <inheritdoc />
        public override List<ResultListDisplayColumn> AdditionalDisplayColumns {
            get {
                List<ResultListDisplayColumn> additionalColummsn = base.AdditionalDisplayColumns;

                if (MergeConfiguration.AddHighScoreEvent) {
                    additionalColummsn.Add( new ResultListDisplayColumn() {
                        Header = AverageMethod.HIGH_EVENT_SCORE_NAME,
                        Body = $"{{{AverageMethod.HIGH_EVENT_SCORE_NAME}}}",
                        BodyValues = new List<ResultListCellValue>() {
                            new ResultListCellValue() {
                                Text = $"{{{AverageMethod.HIGH_EVENT_SCORE_NAME}}}"
                            }
                        },
                        ClassSet = new List<ClassSet>() { new ClassSet() {
                            Name = "rlf-col-event",
                            ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                        }}
                    } );
                }
                return additionalColummsn;
            }
        }

        /// <inheritdoc />
        public override string TopLevelHeaderText {
            get {
                return "Average";
            }
        }
    }
}
