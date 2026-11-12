using System.Diagnostics;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ResultStatusCalculator {

        /// <summary>
        /// The length of time to wait before considering an INTERMEDIATE result to be stale and should instead be considered UNOFFICIAL.
        /// <para>Currently set to 2 hours.</para>
        /// </summary>
        public static readonly TimeSpan INTERMEDIATE_STATUS_TIMEOUT = new TimeSpan( 2, 0, 0 );

        private List<IEventScores> _eventScores = new List<IEventScores>();

        public ResultStatusCalculator( CourseOfFireStructure courseOfFire ) {
            CourseOfFireStructure = courseOfFire;
        }

        public CourseOfFireStructure CourseOfFireStructure { get; set; }

        public void AddEventScores( IEventScores eventScores ) {
            _eventScores.Add( eventScores );
        }

        public void ClearEventScores() {
            _eventScores.Clear();
        }

        public ResultStatus Calculate( string eventName ) {

            if (this.CourseOfFireStructure.Official)
                return ResultStatus.OFFICIAL;

            if (this._eventScores.Count == 0)
                return ResultStatus.FUTURE;

            if (string.IsNullOrEmpty( eventName )) {
                Debug.Fail( "Got passed in an empty event name. This should never happen." );
                return ResultStatus.OFFICIAL;
            }

            bool allAreFuture = true;
            bool oneIsIntermediate = false;
            bool oneIsUnofficial = false;

            EventScore eventScore;
            foreach (IEventScores item in _eventScores) {
                if (item.EventScores.TryGetValue( eventName, out eventScore )) {
                    allAreFuture &= (eventScore.Status == ResultStatus.FUTURE);
                    oneIsIntermediate |= (eventScore.Status == ResultStatus.INTERMEDIATE && (DateTime.UtcNow - item.LastUpdated) <= INTERMEDIATE_STATUS_TIMEOUT);
                    oneIsUnofficial |= (eventScore.Status == ResultStatus.UNOFFICIAL);
                }
            }

            if (allAreFuture)
                return ResultStatus.FUTURE;
            else if (oneIsIntermediate)
                return ResultStatus.INTERMEDIATE;
            else if (oneIsUnofficial)
                return ResultStatus.UNOFFICIAL;
            else
                return ResultStatus.OFFICIAL;
        }
    }
}
