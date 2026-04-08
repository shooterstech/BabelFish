using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// The purpose of the ShotMapper class is to:
    /// <list type="bullet">
    /// <item><description>Receive <see cref="Shot">Shots</see> as they are fired from an EST system.</description></item>
    /// <item><description>Store received EST Shots to a persistance data file.</description></item>
    /// <item><description>When an instance is instantiated, load existing shots from the data file</description></item>
    /// <item><description>Generate an updated EventScores and Shots dictionary for a given Result COF.</description></item>
    /// <item><description>Recieve <see cref="Shot">Shots</see> from non-EST systems. These shots are unique as they are not persisted but do contribute to EventScores..</description></item>
    /// <item><description></description></item>
    /// <item><description></description></item>
    /// </list>
    /// <para>
    /// Concepts:
    /// <list type="bullet">
    /// <item><description>There should be one ShotMapper per <see cref="MatchProject"/>.</description></item>
    /// <item><description>ShotMapper tracks <see cref="Shot">Shots</see> (and EventScores) on a per Result COF Id basis. It does not know track Participants.</description></item>
    /// <item><description>"EventScores" is a dictionary of (key) event names and (value) <see cref="EventScore"/>,their corresponding scores.</description></item>
    /// <item><description></description></item>
    /// </list>
    /// </para>
    /// </summary>
    public class ShotMapper {

        #region Private Variables

        /*
         * These next two dictionaries _allShots and _shotDictionary will nearly contain the same data.
         * _allShots contains 'ALL' shot data, which includes all updates to all shots. 
         * _shotDictionary contains the 'current' shot data, which is the latest update for each shot and is ordered by sequence number.
         *
         * The Keys to _allShots are as follows, in order;
         * Keys will be as follows, in order;
         * Result COF ID
         * Sequence Number in order
         * Update Number in order
         * Ending value is single shot data.
         */
        private Dictionary<string, Dictionary<float, Dictionary<int, Shot>>> _allShots = new Dictionary<string, Dictionary<float, Dictionary<int, Shot>>>();

        //Key is a result cof id, value is an in order list (by sequence) of the shots assigned to that result cof
        private Dictionary<string, List<Shot>> _shotDictionary = new Dictionary<string, List<Shot>>();

        #endregion

        #region Constructors, Initialization, and Factories

        #endregion

        #region Events

        #endregion

        #region Data Model Properties
        //Of which there should be none since this class is a Data Actor (and not a Data Model)

        #region

        #region Helper Properties

        #endregion

        #region Methods
        /// <summary>
        /// Event Handler method to receive a shot as it is fired from an EST system.
        /// <para>The received shot will be loaded into the internal data strucutres and stored to persistance.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReceiveShot( object sender, ShotReceivedEventArgs e ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event Handler method to recieve a series of shots, as they are fired from an EST system.
        /// <para>The received shots will be loaded into the internal data strucutres and stored to persistance.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReceiveShotList( object sender, ShotListReceivedEventArgs e ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a set of <see cref="Shot">Shots</see> fired by a participant for the past in Result COF ID.
        /// <para>Key is the sequence number of the shot represented as a string.</para>
        /// <para>Value is the Shot.</para>
        /// <para>Intended to be used to populate the Shots dictionary in <see cref="IEventScores"/>.</para>
        /// </summary>
        public Dictionary<string, Shot> GetShots( string resultCOFID ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compiles the <see cref="EventScore"/> fired by a participant identified by the Result COF ID.
        /// Returns a dictionary organized by event name. Compiles the EventScores based on the passed in
        /// top level <see cref="EventComposite"/> and the shots tracked by the ShotMapper. Obviously, the
        /// passed in EventComposite should be generated from the same <see cref="CourseOfFire">COURSE OF FIRE</see> as was used to generate the Result COF. 
        /// <para>Key is the EventName (as defined by the COURSE OF FIRE).</para>
        /// <para>Value is the compiled EventScore.</para>
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <param name="topLevelEvent">The top level EventComposite from the <see cref="CourseOfFire">COURSE OF FIRE</see>.</param>
        /// <returns></returns>
        public Dictionary<string, EventScore> GetEventScores( string resultCOFID, EventComposite topLevelEvent ) {
            throw new NotImplementedException();
        }
        #endregion
    }
}
