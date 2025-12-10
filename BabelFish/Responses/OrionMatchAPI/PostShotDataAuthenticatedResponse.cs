using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class PostShotDataAuthenticatedResponse : Response<PostShotDataWrapper> {


        public PostShotDataAuthenticatedResponse( PostShotDataAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the list of AcceptedShots.
        /// <para>Same as .Value.AcceptedShots.</para>
        /// </summary>
        public List<Shot> AcceptedShots {
            get { return Value.AcceptedShots; }
        }

        /// <summary>
        /// Facade function that returns the list of RejectedShots.
        /// <para>Same as .Value.RejectedShots.</para>
        /// </summary>
        public List<RejectedShot> RejectedShots {
            get { return Value.RejectedShots; }
        }
    }

    public class PostShotDataWrapper() : BaseClass {

        public List<Shot> AcceptedShots { get; set; }

        public List<RejectedShot> RejectedShots { get; set; }
    }

    public class RejectedShot : Shot {

        public List<string> RejectionMessages { get; set; }

    }
}
