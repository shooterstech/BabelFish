using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.GetSetAttributeValueAPI {
    public class GetAttributeValueRequest : Request {

        public GetAttributeValueRequest(UserCredentials credentials) : base( "GetAttributeValue", credentials ) {
            //NOTE: Because this request requires user credentials, we're only writing one constructor that includes parameters for crendentials.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// Attribute Names
        /// </summary>
        public List<string> AttributeNames = new List<string>();

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/users/attribute-value"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (AttributeNames.Count() == 0)
                    throw new GetOrionMatchRequestException( "Must have at least one Attribute Name." );

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add( "attribute-def", AttributeNames );

                return parameterList;
            }
        }
    }
}