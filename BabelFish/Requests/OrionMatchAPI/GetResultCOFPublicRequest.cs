using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultCOFPublicRequest : GetResultCOFAbstractRequest {

        public GetResultCOFPublicRequest( string resultCOFId ) : base( "GetResultCourseOfFire", resultCOFId ) {

        }
    }
}
