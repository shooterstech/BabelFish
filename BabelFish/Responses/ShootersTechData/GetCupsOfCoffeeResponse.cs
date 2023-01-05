using ShootersTech.BabelFish.DataModel.ShootersTechData;
using ShootersTech.BabelFish.Requests.ShootersTechData;
using ShootersTech.BabelFish.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Responses.ShootersTechData {
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
