using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.Athena.AbstractEST;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PostShotDataAuthenticatedRequest : Request {

        public PostShotDataAuthenticatedRequest( UserAuthentication credentials, MatchID matchId ) : base( "PostShotData", credentials ) {
            HttpMethod = HttpMethod.Post;
        }

        /// <summary>
        /// The Orion MatchId.
        /// </summary>
        public MatchID MatchId { get; set; }

        /// <summary>
        /// The list of shots to submit to this Orion match.
        /// </summary>
        public List<Shot> Shots { get; set; } = new List<Shot>();

        /// <inheritdoc />
        public override string RelativePath {
            get {
                return $"/match/{MatchId}/shot";
            }
        }

        /// <inheritdoc />
        public override StringContent PostParameters {
            get {

                PostShotDataWrapper shotDataWrapper = new PostShotDataWrapper();
                shotDataWrapper.Shots = this.Shots;

                var shotsJsonAsString = G_NS.JsonConvert.SerializeObject( shotDataWrapper, SerializerOptions.NewtonsoftJsonSerializer );
                return new StringContent( G_NS.JsonConvert.SerializeObject( shotsJsonAsString ), Encoding.UTF8, "application/json" );

            }
        }

        /// <summary>
        /// Helper class to format the request object submitted to PostShotData.
        /// </summary>
        private class PostShotDataWrapper {

            public List<Shot> Shots { get; set; }

            /* 
             * The public PostShotData allows (and requires) the parameter SharedKey. 
             * However, the use of this is deprecated, in favor of the athenticated call which
             * authorizes submiting shot data based on the user's role in the match.
             */
        }
    }
}
