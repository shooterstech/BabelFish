using ShootersTech.DataModel.Definitions;

namespace ShootersTech.Responses.DefinitionAPI
{
    public class GetDefinitionResponse<T> : Response<T>
    {

        public GetDefinitionResponse(SetName setName, Definition.DefinitionType definitionType) {
            this.SetName = setName;
            this.DefinitionType = definitionType;
        }

        public SetName SetName { get; private set; }

        public Definition.DefinitionType DefinitionType { get; private set; }

        public T Definition {
            get { return Value; }
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            Value = Body[SetName.ToString()].ToObject<T>();
        }
    }
}
