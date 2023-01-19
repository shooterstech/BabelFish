using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.ScoposData {
    public class GetCupsOfCoffeeResponse :Response<CupsOfCoffee> {

        public GetCupsOfCoffeeResponse( GetCupsOfCoffeeRequest request ) {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public int CupsOfCoffeeConsumed {
            get { return Value.CupsOfCoffeeConsumed; }
        }
    }
}
