using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.ScoposData {
    public class GetCupsOfCoffeePublicResponse : Response<CupsOfCoffee> {

        public GetCupsOfCoffeePublicResponse( GetCupsOfCoffeePublicRequest request ) {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public int CupsOfCoffeeConsumed {
            get { return Value.CupsOfCoffeeConsumed; }
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            return DateTime.UtcNow.AddMinutes( 15 );

        }
    }
}
