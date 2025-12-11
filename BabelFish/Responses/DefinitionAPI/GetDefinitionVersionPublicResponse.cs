using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;

namespace Scopos.BabelFish.Responses.DefinitionAPI {
    public class GetDefinitionVersionPublicResponse : Response<SparseDefinition> {

        /// <summary>
        /// The time, in minutes, that a GetDefinition API call cached value will be expired. 
        /// <para>Definitions don't change often, the default value 30 reflects this.</para>
        /// </summary>
        public static int CacheExpiryTime { get; set; } = 30;

        public GetDefinitionVersionPublicResponse( GetDefinitionVersionPublicRequest request ) {
            this.Request = request;

            this.SetName = request.SetName;
            this.DefinitionType = request.DefinitionType;
        }

        public SetName SetName { get; private set; }

        public DefinitionType DefinitionType { get; private set; }

        public SparseDefinition SparseDefinition {
            get {
                return Value;
            }
        }

        /// <inheritdoc/>
        protected internal override DateTime GetCacheValueExpiryTime() {
            return DateTime.UtcNow.AddMinutes( CacheExpiryTime );
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            G_STJ.JsonElement root = Body.RootElement;
            G_STJ.JsonElement definition = root.GetProperty( SetName.ToString() );
            Value = G_STJ.JsonSerializer.Deserialize<SparseDefinition>( definition, SerializerOptions.SystemTextJsonDeserializer );
        }
    }
}
