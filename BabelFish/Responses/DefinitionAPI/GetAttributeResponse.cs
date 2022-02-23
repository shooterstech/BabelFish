using BabelFish.DataModel.Definitions;

namespace BabelFish.Responses.DefinitionAPI
{
    public class GetAttributeResponse : Response<AttributeWrapper>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public BabelFish.DataModel.Definitions.Attribute Attribute
        {
            get
            {
                return Value.Attribute;
            }
        }  
    }
}
