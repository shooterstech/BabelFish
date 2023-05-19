using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.AthenaLogin {
    public class ESTUnitLoginUser {

        /// <summary>
        /// GUID formatted user id of the logged in user.
        /// </summary>
        public string UserID { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }
    }
}
