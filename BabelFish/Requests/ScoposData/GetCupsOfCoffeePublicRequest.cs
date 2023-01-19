using Scopos.BabelFish.Requests.OrionMatchAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoposData {
    public class GetCupsOfCoffeePublicRequest : Request {

        public GetCupsOfCoffeePublicRequest() : base( "GetCoffee" ) { }

        /// <inheritdoc />
        public override string RelativePath  {
            get { return $"/coffee"; }
        }
    }
}
