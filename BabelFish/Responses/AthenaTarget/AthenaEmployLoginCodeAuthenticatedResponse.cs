using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaTarget;
using Scopos.BabelFish.Requests.AthenaTarget;

namespace Scopos.BabelFish.Responses.AthenaTarget {
    public class AthenaEmployLoginCodeAuthenticatedResponse : Response<ESTUnitLogin> {

        public AthenaEmployLoginCodeAuthenticatedResponse( AthenaEmployLoginCodeAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public ESTUnitLogin EmployLoginCodeResponse {
            get { return Value; }
        }
    }
}
