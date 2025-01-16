using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Scopos.BabelFish.Responses.DefinitionAPI {
    public class GetDefinitionVersionPublicResponse : Response<SparseDefinition> {

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
        protected override void ConvertBodyToValue() {
            JsonElement root = Body.RootElement;
            JsonElement definition = root.GetProperty( SetName.ToString() );
            Value = JsonSerializer.Deserialize<SparseDefinition>( definition, SerializerOptions.APIClientSerializer );
        }
    }
}
