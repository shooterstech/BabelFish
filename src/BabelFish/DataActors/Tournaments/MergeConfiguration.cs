using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.Tournaments {

    /// <summary>
    /// Abstract class describing the configuration (or properties) that a Merge Method to use while merging result lists.
    /// <para>Most of the properties are concrete class specific.</para>
    /// </summary>
    public abstract class MergeConfiguration : IGetScoreFormatCollectionDefinition {

        /// <summary>
        /// Concrete class identifier. Its value will be the same value as the cooresponding
        /// MergeMethod class' .Method.
        /// </summary>
        public string Method { get; protected set; }

        /// <summary>
        /// The SCORE FORMAT COLLECTION definition to us while displaying scores for this MergedResultList
        /// </summary>
        public SetName ScoreFormatCollectionDef { get; set; } = SetName.Parse( "v1.0:orion:Standard Score Formats" );

        /// <summary>
        /// The ScoreConfigName to use, within the SCORE FORMAT COLLECTION, , while displaying scores for this MergedResultList
        /// </summary>
        public string ScoreConfigName { get; set; } = "Decimal";

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( ScoreFormatCollectionDef );
        }
    }
}
