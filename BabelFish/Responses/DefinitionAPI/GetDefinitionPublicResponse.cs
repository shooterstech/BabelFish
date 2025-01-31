using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using System.Text.Json;

namespace Scopos.BabelFish.Responses.DefinitionAPI
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
    public class GetDefinitionPublicResponse<T> : Response<T>
        where T : Definition, new() {

        public GetDefinitionPublicResponse( GetDefinitionPublicRequest request) : base() {
            this.Request = request;

            this.SetName = request.SetName;
            this.DefinitionType = request.DefinitionType;
        }

        public SetName SetName { get; private set; }

        public DefinitionType DefinitionType { get; private set; }

        public T Definition {
            get { return Value; }
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            JsonElement root = Body.RootElement;
            JsonElement definition = root.GetProperty( SetName.ToString() );
            Value = JsonSerializer.Deserialize<T>( definition, G_BF_STJ_CONV.SerializerOptions.APIClientSerializer );
        }

        /// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {
            //Definition files don't change often, so we can set the expiration time well into the future.
			return DateTime.UtcNow.AddDays(1);
		}
	}
}
