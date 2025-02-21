using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.APIClients;


namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class PatchScoreHistoryRequest : Request {

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public PatchScoreHistoryRequest(UserAuthentication credentials ) : base( "PostScoreHistory", credentials ) {
            HttpMethod = HttpMethod.Put;
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public ScoreHistoryPostEntry ScoreHistoryPatch { get; set; } = new ScoreHistoryPostEntry();

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/athlete/score/history"; }
        }

        public override StringContent PostParameters {
            get {
                StringBuilder serializedJSON = new StringBuilder();
                try {
                    return new StringContent( G_NS.JsonConvert.SerializeObject( ScoreHistoryPatch, SerializerOptions.NewtonsoftJsonSerializer ), Encoding.UTF8, "application/json" );
                } catch (Exception ex) {
                    Logger.Error( ex );
                    return new StringContent( "" );
                }
            }
        }

        
    }
}
