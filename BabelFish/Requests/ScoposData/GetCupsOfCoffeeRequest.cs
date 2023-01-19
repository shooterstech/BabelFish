using Scopos.BabelFish.Requests.OrionMatchAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ScoposData {
    public class GetCupsOfCoffeeRequest : Request {

        public GetCupsOfCoffeeRequest() : base( "GetCoffee" ) { }

        /// <inheritdoc />
        public override string RelativePath  {
            get { return $"/coffee"; }
        }
    }
}
