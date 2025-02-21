using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.AttributeValueAPI {
    public class SetAttributeValueAuthenticatedRequest : Request {

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
                //var json = G_NS.JsonConvert.SerializeObject( AttributeValuesToUpdate, SerializerOptions.NewtonsoftJsonSerializer );
                
                JObject json = new JObject();
                try {
                    JObject attributeValuesJson = new JObject();
                    json.Add( "attribute-values", attributeValuesJson );
                    foreach (var attributeValueToUpdate in AttributeValuesToUpdate) {
                        var avJsonAsString = G_NS.JsonConvert.SerializeObject( attributeValueToUpdate, SerializerOptions.NewtonsoftJsonSerializer );
                        attributeValuesJson.Add( attributeValueToUpdate.AttributeValue.SetName.ToString(), JObject.Parse( avJsonAsString ) );
                    }
                } catch (Exception ex) {
                    ;
                }
                return new StringContent( JsonConvert.SerializeObject( json ), Encoding.UTF8, "application/json" );

            }
        }
    }

}