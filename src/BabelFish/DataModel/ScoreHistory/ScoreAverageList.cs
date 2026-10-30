using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Primarily represents the data returned by a GetScoreAverage API call. Has a helper method <see cref="TryGetAverageScore"/>
    /// That will attempt to computer the average score for a given athlete, event style, and date.
    /// </summary>
    public class ScoreAverageList : ITokenItems<ScoreAverageBase> {

        public ScoreAverageList() { }

        public List<ScoreAverageBase> Items { get; set; } = new List<ScoreAverageBase>();

        /// <inheritdoc />
		[JsonConverter( typeof( NextTokenConverter ) )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        public override string ToString() {
            return $"ScoreAverage with {Items.Count} items";
        }

        /// <summary>
        /// Calculates an aggregate average score for a user on a specific date by combining matching stage-style
        /// averages from the score history with the event style components.
        /// </summary>
        /// <remarks>Aggregates are weighted by component shot counts. By convention, <c>J</c>, <c>K</c>,
        /// and <c>L</c> store integer, decimal, and X aggregates.</remarks>
        /// <param name="userId">Identifier of the user whose average score is requested.</param>
        /// <param name="eventStyleSimpleCof">Event style definition that provides the stage-style components and shot counts used to weight the
        /// aggregate.</param>
        /// <param name="onThisDate">Date used to select score-average entries.</param>
        /// <param name="averageScore">When this method returns, contains the aggregated score values from matching entries; otherwise contains
        /// default score values.</param>
        /// <returns><see langword="true"/> when at least one matching score-average entry is found and included; otherwise, <see
        /// langword="false"/>.</returns>
        public bool TryGetAverageScore( string userId, SimpleCOF eventStyleSimpleCof, DateTime onThisDate, out Score averageScore ) {
            averageScore = new Score();
            int countOfScores = 0;

            foreach (var scoreAverageBase in Items) {

                if (scoreAverageBase is ScoreAverageStageStyleEntry) {

                    var scoreAverageEntry = (ScoreAverageStageStyleEntry)scoreAverageBase;
                    if (scoreAverageEntry.UserID == userId && scoreAverageEntry.StartDate.Date == onThisDate.Date) {
                        foreach (var component in eventStyleSimpleCof.Components) {
                            if (component.StageStyleDef.Equals( scoreAverageEntry.StageStyleDef )) {
                                //J, K, L are (by tradition) used to hold the integer, decimal, and X averages.
                                averageScore.J += (int)Math.Round( scoreAverageEntry.ScoreAverage.I * component.Shots, 0 );
                                averageScore.K += scoreAverageEntry.ScoreAverage.D * component.Shots;
                                averageScore.L += (int)Math.Round( scoreAverageEntry.ScoreAverage.X * component.Shots, 0 );

                                countOfScores++;
                            }
                        }
                    }
                }
            }

            return countOfScores > 0;
        }

    }
}
