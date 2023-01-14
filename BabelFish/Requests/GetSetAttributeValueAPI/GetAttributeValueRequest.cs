using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Requests.GetSetAttributeValueAPI
{
    public class GetAttributeValueRequest : Request
    {

        /// <summary>
        /// Public constructor. 
        /// User is encouraged (really you need to do this) to set the Request Properties at time of construction.
        /// </summary>
        public GetAttributeValueRequest() {
            // Internally always set authentication required
            WithAuthentication = true;
        }

    /// <summary>
    /// Attribute Names
    /// </summary>
    public List<string> AttributeNames = new List<string>();

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/users/attribute-value"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                if (AttributeNames.Count() == 0)
                    throw new GetOrionMatchRequestException("Must have at least one Attribute Name.");

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("attribute-def", AttributeNames);

                return parameterList;
            }
        }
    }
}