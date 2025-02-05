using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class PostScoreHistoryRequest : Request {

        /// <inheritdoc />
        public PostScoreHistoryRequest(UserAuthentication credentials ) : base( "PostScoreHistory", credentials ) {
            HttpMethod = HttpMethod.Post;
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public ScoreHistoryPostEntry ScoreHistoryPost { get; set; } = new ScoreHistoryPostEntry();

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/history"; }
        }

        public override StringContent PostParameters {
            get {
                StringBuilder serializedJSON = new StringBuilder();
                try {
                    return new StringContent( G_NS.JsonConvert.SerializeObject( ScoreHistoryPost, SerializerOptions.NewtonsoftJsonSerializer ), Encoding.UTF8, "application/json" );
                } catch (Exception ex) {
                    //logger.Error( ex );
                    return new StringContent( "" );
                }
            }
        }        
    }
}
