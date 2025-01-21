using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using System.Text.Json;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using NLog;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.Requests.AttributeValueAPI {
    public class SetAttributeValueAuthenticatedRequest : Request {

        private static Logger Logger = LogManager.GetCurrentClassLogger();

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
                    var json = JsonSerializer.Serialize( AttributeValuesToUpdate, SerializerOptions.APIClientSerializer );
                    return new StringContent( json, Encoding.UTF8, "application/json" );
                } catch (Exception ex) {
                    Logger.Error( ex );
                    return new StringContent( "" );
                }
            }
        }
    }

}