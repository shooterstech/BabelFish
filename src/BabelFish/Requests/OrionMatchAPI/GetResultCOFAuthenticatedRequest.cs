using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultCOFAuthenticatedRequest : GetResultCOFAbstractRequest {

        public GetResultCOFAuthenticatedRequest( string resultCOFId, UserAuthentication credentials ) : base( "GetResultCourseOfFire", resultCOFId, credentials ) {
        }
    }
}
