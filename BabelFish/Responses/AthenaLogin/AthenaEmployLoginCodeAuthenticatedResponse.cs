using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;
using Scopos.BabelFish.Requests.AthenaLogin;

namespace Scopos.BabelFish.Responses.AthenaLogin {
    public class AthenaEmployLoginCodeAuthenticatedResponse : Response<ESTUnitLoginWrapper> {

        public AthenaEmployLoginCodeAuthenticatedResponse( AthenaEmployLoginCodeAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public ESTUnitLogin ESTUnitLogin {
            get { return Value.ESTUnitLogin; }
        }
    }
}
