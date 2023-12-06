using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetResultCOFDetailAuthenticatedRequest : Request {

        public GetResultCOFDetailAuthenticatedRequest( string resultCOFId, UserAuthentication credentials ) : base( "GetResultCourseOfFire", credentials ) {
            ResultCOFID = resultCOFId;
            this.RequiresCredentials = true;
        }

        public string ResultCOFID { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/resultcof/{ResultCOFID}"; }
        }
    }
}
