using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Requests.AttributeValueAPI {
    public class GetAttributeValueAuthenticatedRequest : Request {

        public GetAttributeValueAuthenticatedRequest( UserAuthentication credentials ) : base( "GetAttributeValue", credentials ) {
            //NOTE: Because this request requires user credentials, we're only writing one constructor that includes parameters for crendentials.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// Attribute Names
        /// </summary>
        public List<SetName> AttributeNames = new List<SetName>();

        /// <summary>
        /// When making a get attribute value request, if the user (defined by the UserAuthentication) does not have a value for one of the 
        /// Attribute names the API call normall returns a 404 status code (not found). However, if ReturnDefaultValues is true, then 
        /// default values instead are returned.
        /// 
        /// ReturnDefaultValues is by default false
        /// </summary>
        public bool ReturnDefaultValues { get; set; } = false;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/users/attribute-value"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (AttributeNames.Count() == 0)
                    throw new RequestException( "Must have at least one Attribute Name." );

                //Convert the list of SetNames to ask for to a list of strings
                List<string> attributeNamesAsStrings = new List<string>();
                foreach (var attributeName in AttributeNames)
                    attributeNamesAsStrings.Add( attributeName.ToString() );

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add( "attribute-def", attributeNamesAsStrings );
                parameterList.Add( "return-default", new List<string>() { ReturnDefaultValues.ToString().ToLower() } );
                //NOTE: The Get Attribute Value API call also accepts a 'return-definiiton' parameter. Choosing not to include this as an option since BabelFish uses AttributeValueDefinitionFetcher to learn the definition of the Attribute.

                return parameterList;
            }
        }
    }
}