using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// Abstract class describing the configuration (or properties) that a Merge Method to use while merging result lists.
    /// <para>Most of the properties are concrete class specific.</para>
    /// </summary>
    public abstract class MergeConfiguration :
        IGetScoreFormatCollectionDefinition,
        ICheckSum {

        #region Data Model Properties
        /// <summary>
        /// Concrete class identifier. Its value will be the same value as the cooresponding
        /// MergeMethod class' .Method.
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public MergeMethodType Method { get; protected set; }

        /// <summary>
        /// The SCORE FORMAT COLLECTION definition to us while displaying scores for this MergedResultList
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public SetName ScoreFormatCollectionDef { get; set; } = SetName.Parse( "v1.0:orion:Standard Score Formats" );

        /// <summary>
        /// The ScoreConfigName to use, within the SCORE FORMAT COLLECTION, , while displaying scores for this MergedResultList
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string ScoreConfigName { get; set; } = "Decimal";

        #endregion

        #region Helper Properties

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as this is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( ScoreFormatCollectionDef );
        }

        /// <inheritdoc />
        public abstract ulong CalculateChecksum();

        #endregion
    }
}
