using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetResultCOFPublicResponse : GetResultCOFAbstractResponse {

        public GetResultCOFPublicResponse( GetResultCOFPublicRequest request ) : base() {
            this.Request = request;
        }
    }
}
