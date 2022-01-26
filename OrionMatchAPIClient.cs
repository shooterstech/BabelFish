using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.Requests;
using BabelFish.DataModel.OrionMatch;

namespace BabelFish {
    public class OrionMatchAPIClient : APIClient {

        private GetMatchResponse GetMatchResponse = new GetMatchResponse();

        public OrionMatchAPIClient(string xapikey) : base(xapikey) { }

        public async Task<Match> GetMatchDetail( GetMatchRequest requestParameters ) 
        {
            return await GetMatchResponse.GetMatchDetailResponse( requestParameters, XApiKey, ApiStage.ToString() ).ConfigureAwait(false);
        }

        public object GetResultList( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object GetSquaddingList( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object GetResultCourseOfFireDetail( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object GetMatchLocations( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object MatchSearch( object requestParameters ) {
            throw new NotImplementedException();
        }
    }
}
