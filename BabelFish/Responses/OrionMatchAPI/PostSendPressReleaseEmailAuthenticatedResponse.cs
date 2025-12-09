using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class PostSendPressReleaseEmailAuthenticatedResponse : Response<SentPressReleaseEmailWrapper> {


        public PostSendPressReleaseEmailAuthenticatedResponse( PostSendPressReleaseEmailAuthenticatedRequest request ) : base( ) { 
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SentPressRelease SentPressRelease {
            get { return Value.SentPressRelease; }
        }
    }
}
