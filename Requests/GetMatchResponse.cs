using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;
using BabelFish.Components;
using BabelFish.Components.Objects;

namespace BabelFish.Requests {
    public class GetMatchResponse {

        private AWSApi awsapi = new AWSApi();

        public async Task<Match> GetMatchDetailResponse(GetMatchRequest requestParameters, string xapikey, string environment) {

            this.Match = new Match();

            try
            {
                // Build specific API request object
                AWSRequestObject apirequest = new AWSRequestObject(new Dictionary<string, string>() { { "x-api-key", xapikey } }, "GET",
                                    environment, $"/match/{requestParameters.MatchID}");

                AWSResponseObject awsresponse = await awsapi.GetAsync(apirequest).ConfigureAwait(false);
                if (awsresponse.Errors.Count > 0)
                {
                    //Log something
                    //DrA 20220125: Consider how to represent api error conditions to customer
                }
                else
                {
                    //deserialize into Match
                }
            }
            catch (Exception ex)
            {

            }

            return Match;
        }

        public Match Match { get; set; }  
    }
}
