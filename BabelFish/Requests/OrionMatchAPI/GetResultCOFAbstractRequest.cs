using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetResultCOFAbstractRequest : Request {

        public GetResultCOFAbstractRequest( string operationId, string resultCOFId ) : base( operationId ) { 
            ResultCOFID = resultCOFId; 
        }

        public GetResultCOFAbstractRequest( string operationId, string resultCOFId, UserAuthentication credentials ) : base( operationId, credentials ) {
            ResultCOFID = resultCOFId;
        }

        public static GetResultCOFAbstractRequest Factory( string resultCOFId, UserAuthentication credentials) {
            if ( credentials == null ) {
                return new GetResultCOFPublicRequest( resultCOFId );
            } else {
                return new GetResultCOFAuthenticatedRequest( resultCOFId, credentials );
            }
        }

        public string ResultCOFID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/resultcof/{ResultCOFID}"; }
        }
    }
}
