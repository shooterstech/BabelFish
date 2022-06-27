using ShootersTech.DataModel.Definitions;

namespace ShootersTech.Responses.DefinitionAPI
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
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
