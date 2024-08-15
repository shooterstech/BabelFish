using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Responses;

namespace Scopos.BabelFish.Responses.Athena
{
    public class AthenaLogoutSessionAuthenticatedResponse : Response<ESTUnitLogoutWrapper>
    {

        public AthenaLogoutSessionAuthenticatedResponse(AthenaLogoutSessionAuthenticatedRequest request) : base()
        {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public ESTUnitLogout ESTUnitLogout
        {
            get { return Value.ESTUnitLogout; }
        }
    }
}
