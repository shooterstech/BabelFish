using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Scopos.BabelFish.DataActors.OrionMatch {
#pragma warning restore IDE0130 // Namespace does not match folder structure

    /// <summary>
    /// An IComparer implementation for EventScore objects. In other words, this class may be used to sort EventScore object. 
    /// </summary>
    public class CompareEventScore : IComparer<EventScore>, IGetScoreFormatCollectionDefinition {

        /// <summary>
        /// The SCORE FORMAT COLLECTION definition to use to look up how to compare (and sort) EventScore instances.
        /// </summary>
        public SetName ScoreFormatCollectionDef { get; private set; }

        /// <summary>
        /// The ScoreConfigName, within the SCORE FORMAT COLLECTION (specified by the property ScoreFormatCollectionDef) to use to compare
        /// (and sort) EvnetScore instances.
        /// </summary>
        public string ScoreConfigName { get; private set; }

        /// <summary>
        /// Property indicatging if the EventScores will be sorted ascending or descending.
        /// </summary>
        public SortBy SortBy { get; private set; } = SortBy.DESCENDING;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scoreConfigDef"></param>
        /// <param name="scoreConfigName"></param>
        public CompareEventScore( SetName scoreformatCollectionDef, string scoreConfigName ) {
            ScoreFormatCollectionDef = scoreformatCollectionDef;
            ScoreConfigName = scoreConfigName;
        }

        /// <inheritdoc/>
        public int Compare( EventScore x, EventScore y ) {
            int compare = 0;

            //TODO: Make this method generic so it can support any ScoreFormatCollection

            switch (this.ScoreConfigName) {
                case "Integer":
                case "Conventional":
                    compare = x.Score.I.CompareTo( y.Score.I );
                    if (compare == 0)
                        compare = x.Score.X.CompareTo( y.Score.X );
                    break;

                case "Decimal":
                    compare = x.Score.D.CompareTo( y.Score.D );
                    break;

                default:
                    throw new NotImplementedException();

            }

            if (SortBy == SortBy.ASCENDING)
                return compare;
            else
                return -1 * compare;
        }

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( ScoreFormatCollectionDef );
        }
    }
}
