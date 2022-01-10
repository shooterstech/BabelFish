using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.Requests;

namespace BabelFish {
    public class OrionMatchAPIClient : APIClient {

        public OrionMatchAPIClient(string xapikey) : base(xapikey) { }

        public GetMatchResponse GetMatch( GetMatchRequest requestParameters ) {

            /*
             * Magic happens here
             */

            return new GetMatchResponse();
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
