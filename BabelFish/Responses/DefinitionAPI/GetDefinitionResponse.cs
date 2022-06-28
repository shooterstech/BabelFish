using ShootersTech.DataModel.Definitions;
using ShootersTech.Requests.DefinitionAPI;

namespace ShootersTech.Responses.DefinitionAPI
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
    public class GetDefinitionResponse<T> : Response<T>
        where T : new() {

        public GetDefinitionResponse( GetDefinitionRequest request) : base() {
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
