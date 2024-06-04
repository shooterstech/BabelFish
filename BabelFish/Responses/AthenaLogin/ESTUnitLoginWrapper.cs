using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.AthenaLogin;

namespace Scopos.BabelFish.Responses.AthenaLogin {
    public class ESTUnitLoginWrapper : BaseClass {


        public ESTUnitLogin ESTUnitLogin = new ESTUnitLogin();

        public override string ToString() {
            return $"EST Unit Login for {ESTUnitLogin.ThingName}";
        }
    }
}
