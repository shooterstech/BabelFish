using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;

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
            this.WriteToFileSystemCacheOnSuccess = true;
        }

        public SetName SetName { get; private set; }

        public DefinitionType DefinitionType { get; private set; }

        public T Definition {
            get { return Value; }
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            //Only deserialize if hte status code was OK. Skip trying, if it was anythnig else, like, not found.
            //There is an argument to be made that we should throw a Attribute
            if (this.StatusCode == System.Net.HttpStatusCode.OK) {
                G_STJ.JsonElement root = Body.RootElement;
                G_STJ.JsonElement definition = root.GetProperty( SetName.ToString() );
                Value = G_STJ.JsonSerializer.Deserialize<T>( definition, SerializerOptions.SystemTextJsonDeserializer );
            }
        }

        /// <inheritdoc />
		protected internal override DateTime GetCacheValueExpiryTime() {
            //Definition files don't change often, so we can set the expiration time well into the future.
			return DateTime.UtcNow.AddDays(1);
		}
	}
}
