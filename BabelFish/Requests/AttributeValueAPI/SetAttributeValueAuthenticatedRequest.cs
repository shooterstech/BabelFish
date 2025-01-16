using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.Text.Json;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using NLog;

namespace Scopos.BabelFish.Requests.AttributeValueAPI {
    public class SetAttributeValueAuthenticatedRequest : Request {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public SetAttributeValueAuthenticatedRequest( UserAuthentication credentials ) : base( "SetAttributeValue", credentials ) {
            HttpMethod = HttpMethod.Post;
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public List<AttributeValueDataPacket> AttributeValuesToUpdate { get; set; } = new List<AttributeValueDataPacket>();

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/users/attribute-value"; }
        }

        public override StringContent PostParameters {
            get {
                StringBuilder serializedJSON = new StringBuilder();
                try {
                    throw new NotImplementedException();
                    /*
                    JObject json = new JObject();
                    JObject attributeValuesJson = new JObject();
                    json.Add( "attribute-values", attributeValuesJson );
                    foreach (var attributeValueToUpdate in AttributeValuesToUpdate) {
                        attributeValuesJson.Add( attributeValueToUpdate.AttributeValue.SetName.ToString(), attributeValueToUpdate.ToJToken() );
                    }

                    return new StringContent( JsonConvert.SerializeObject( json ), Encoding.UTF8, "application/json" );
                    */
                } catch (Exception ex) {
                    logger.Error( ex );
                    return new StringContent( "" );
                }
            }
        }
    }

}