using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;

namespace Scopos.BabelFish.Responses.DefinitionAPI
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
    public class GetDefinitionPublicResponse<T> : Response<T>
        where T : Definition {

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
            Value = Body[SetName.ToString()].ToObject<T>();
        }
    }
}
