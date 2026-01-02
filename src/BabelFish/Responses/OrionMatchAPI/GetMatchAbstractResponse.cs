using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Abstract class to hide the difference between an Public and Authenticated API call.
    /// </summary>
    public abstract class GetMatchAbstractResponse : Response<MatchWrapper> {

        public GetMatchAbstractResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Match Match {
            get { return Value.Match; }
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                //if today is between start/end then timeout is 30 sec, else, make is 5 min
                if (DateTime.Today >= Match.StartDate && DateTime.Today <= Match.EndDate) {
                    return DateTime.UtcNow.AddSeconds( 30 );
                } else {
                    return DateTime.UtcNow.AddMinutes( 5 );
                }
            } catch (Exception ex) {
                //Likely will never get here, if so, likely from a very old match.
                return DateTime.UtcNow.AddMinutes( 5 );
            }
        }
    }
}
