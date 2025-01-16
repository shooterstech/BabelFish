using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.APIClients;
using System.Text.Json;
using NLog;


namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {
    public class PatchScoreHistoryRequest : Request {

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
                    return new StringContent( JsonSerializer.Serialize( ScoreHistoryPatch ), Encoding.UTF8, "application/json" );
                } catch (Exception ex) {
                    //logger.Error( ex );
                    return new StringContent( "" );
                }
            }
        }

        
    }
}
