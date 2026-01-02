using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Abstract class to hide the difference between an Public and Authenticated API call.
    /// </summary>
    public abstract class GetTournamentAbstractResponse : Response<TournamentWrapper> {

        public GetTournamentAbstractResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Tournament Tournament {
            get { return Value.Tournament; }
        }

        /// <inheritdoc />
        protected internal override DateTime GetCacheValueExpiryTime() {

            try {
                //if today is between start/end then timeout is 30 sec, else, make is 5 min
                if (DateTime.Today >= Tournament.StartDate && DateTime.Today <= Tournament.EndDate) {
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
