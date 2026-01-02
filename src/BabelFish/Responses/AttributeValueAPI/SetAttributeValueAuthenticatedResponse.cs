using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Requests.AttributeValueAPI;

namespace Scopos.BabelFish.Responses.AttributeValueAPI
{
    public class SetAttributeValueAuthenticatedResponse : Response<SetAttributeValueList>
    {
        private const string OBJECT_LIST_NAME = "attribute-value-responses";

        public SetAttributeValueAuthenticatedResponse( SetAttributeValueAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<SetAttributeValue> SetAttributeValues
        {
            get { return Value.SetAttributeValues; }
        }

        /// <summary>
        /// Retrieve an object of the SetAttribute requested
        /// </summary>
        /// <param name="AttributeValueSetName"></param>
        /// <returns>SetAttributeValue response object if found</returns>
        public SetAttributeValue GetAttributeValue(string AttributeValueSetName)
        {
            //loop AttributeValues and return the one object they ask for
            return SetAttributeValues.Where(x => x.AttributeValue == AttributeValueSetName).FirstOrDefault();
        }

        protected override void ConvertBodyToValue() {

            if (RestApiStatusCode == System.Net.HttpStatusCode.OK) {
                var rootElement = Body.RootElement;
                var setAttrValueListElement = rootElement.GetProperty( OBJECT_LIST_NAME );

                Value = G_STJ.JsonSerializer.Deserialize<SetAttributeValueList>( setAttrValueListElement, SerializerOptions.SystemTextJsonDeserializer );
            } else {
                Value = new SetAttributeValueList();
            }
            
        }
    }
}