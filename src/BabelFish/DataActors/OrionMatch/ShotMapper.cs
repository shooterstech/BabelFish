using System.Collections.Concurrent;
using System.Diagnostics;
using Newtonsoft.Json;
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
    /// <para>Instances of ShotMapper are created by a <see cref="MatchProject"/>.</para>
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
    public class ShotMapper : IDisposable {

        #region Private Variables

        private Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _initializing = false;
        private FileInfo _shotLogFile;
        private volatile bool _threadsShouldDie = false;

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
        private ConcurrentDictionary<string, ConcurrentDictionary<float, ConcurrentDictionary<int, Shot>>> _allShots = new ConcurrentDictionary<string, ConcurrentDictionary<float, ConcurrentDictionary<int, Shot>>>();

        //Key is a result cof id, value is an in order list (by sequence) of the shots assigned to that result cof
        private ConcurrentDictionary<string, List<Shot>> _shotDictionary = new ConcurrentDictionary<string, List<Shot>>();
        //Key is a result cof id, value is an object used as a mutex for locking when updating List<Shot> for that result cof in the _shotDictionary
        private ConcurrentDictionary<string, object> _shotListMutexes = new ConcurrentDictionary<string, object>();

        //Instrumentation for loading shots.
        Helpers.Stopwatch _loadShotStopWatch = new Helpers.Stopwatch( "LoadShot()" );
        Helpers.Stopwatch _isInListOfShotsStopWatch = new Helpers.Stopwatch( "Is In List of Shots" );
        Helpers.Stopwatch _addToListOfShotsStopWatch = new Helpers.Stopwatch( "AddToListOfShots()" );
        Helpers.Stopwatch _addToAllShotsStopWatch = new Helpers.Stopwatch( "AddToAllShots()" );

        //Flag to suppress log messages warning about duplicate shot updates. Enable during initialization, disable otherwise.
        private bool _suppressDuplicateShotLogging = false;

        private ConcurrentQueue<Shot> writeToLogQueue = new ConcurrentQueue<Shot>();

        private enum ESTShotOperation { NEW, INSERT, DELETE, UPDATE, REDUNDANT, SIGHTER, NOTYETKNOWN, UNKNOWNCOMPETITOR };

        #endregion

        #region Constructors, Initialization, and Factories

        /// <summary>
        /// Constructor. ShotMappers are created by a MatchProject and do not persist outside of the MatchProject. 
        /// </summary>
        /// <param name="project">The MatchProject that this ShotMapper will track shots for.</param>
        internal ShotMapper( MatchProject project ) {
            this.MatchProject = project;
            this._shotLogFile = new FileInfo( Path.Combine( project.ProjectDirectory.FullName, "athenaShots.json" ) );

            // Start the log writing thread
            var logThread = new Thread( WriteToLogThread ) {
                IsBackground = true
            };
            logThread.Start();
        }
        #endregion

        #region Event Handlers

        EventHandler<EventArgs<Shot>> OnShotAdded;
        EventHandler<EventArgs<Shot>> OnShotUpdated;
        EventHandler<EventArgs<Shot>> OnShotDeleted;
        EventHandler<EventArgs<Shot>> OnSighterReceived;

        #endregion

        #region Data Model Properties
        //Of which there should be none since this class is a Data Actor (and not a Data Model)

        #endregion

        #region Helper Properties

        /// <summary>
        /// Returns the MatchProject that this ShotMapper is tracking shots for.
        /// </summary>
        public MatchProject MatchProject { get; private set; }

        /// <summary>
        /// Indicates if the ShotMapper should persist <see cref="Shot"/> to storage when they are received. If true, ShotMapper will
        /// not persist received shots to storage, but will still track them in memory and use them to calculate EventScores. This is
        /// useful for testing purposes, or for use cases where the consumer of the ShotMapper does not care about persistence (which would be rare).
        /// </summary>
        public bool InMemoryOnly { get; set; } = false;
        #endregion

        #region Public Methods
        /// <summary>
        /// Event Handler method to receive a shot as it is fired from an EST system.
        /// <para>The received shot will be loaded into the internal data strucutres and stored to persistance.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReceiveShot( object sender, ShotReceivedEventArgs e ) {
            var shot = e.Shot;

            LoadShot( e.Shot );
        }

        public void ReceiveShot( object sender, EventArgs<Shot> e ) {
            var shot = e.Value;
            LoadShot( shot );
        }

        /// <summary>
        /// Event Handler method to recieve a series of shots, as they are fired from an EST system.
        /// <para>The received shots will be loaded into the internal data strucutres and stored to persistance.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReceiveShotList( object sender, ShotListReceivedEventArgs e ) {
            foreach (var shot in e.ShotList.Shots) {
                LoadShot( shot );
            }
        }

        /// <summary>
        /// Returns a set of <see cref="Shot">Shots</see> fired by a participant for the passed in Result COF ID.
        /// <para>Key is the sequence number of the shot represented as a string.</para>
        /// <para>Value is the Shot.</para>
        /// <para>Intended to be used to populate the Shots dictionary in <see cref="IEventScores"/>.</para>
        /// <para>Returned dictionary is empty, if the passed in resultCOFID is not known.</para>
        /// </summary>
        public Dictionary<string, Shot> GetShots( string resultCOFID ) {

            //First get the mutex for the Shot List for this Result COF ID. If it doesn't exist, create it.
            var mutex = _shotListMutexes.GetOrAdd( resultCOFID, new object() );
            lock (mutex) {
                //Generate the dictionary of shots to return, if the resultCOFID is known.
                if (_shotDictionary.TryGetValue( resultCOFID, out var shots )) {
                    return shots.ToDictionary( s => s.Sequence.ToString(), s => s );
                }
            }

            //Return an empty dictionary if the resultCOFID is not known.
            return new Dictionary<string, Shot>();
        }

        /// <summary>
        /// Compiles the <see cref="EventScore"/> fired by a participant identified by the Result COF ID.
        /// Returns a dictionary organized by event name. Compiles the EventScores based on the passed in
        /// top level <see cref="EventComposite"/> and the shots tracked by the ShotMapper. Obviously, the
        /// passed in EventComposite should be generated from the same <see cref="CourseOfFire">COURSE OF FIRE</see> as was used to generate the Result COF. 
        /// <para>Key is the EventName (as defined by the COURSE OF FIRE).</para>
        /// <para>Value is the compiled EventScore.</para>
        /// <para>Intended to be used to populate the Shots dictionary in <see cref="IEventScores"/>.</para>
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <param name="topLevelEvent">The top level EventComposite from the <see cref="CourseOfFire">COURSE OF FIRE</see>.</param>
        /// <returns></returns>
        public Dictionary<string, EventScore> GetEventScores( string resultCOFID, EventComposite topLevelEvent ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the last <see cref="Shot"/> fired by a participant identified by the Result COF ID.
        /// <para>Intended to be used to populate the Shots dictionary in <see cref="IEventScores"/>.</para>
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>The last <see cref="Shot"/> fired by the participant, or null if no shots are found.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Shot? GetLastShot( string resultCOFID ) {

            //First get the mutex for the Shot List for this Result COF ID. If it doesn't exist, create it.
            var mutex = _shotListMutexes.GetOrAdd( resultCOFID, new object() );
            lock (mutex) {
                //Generate the dictionary of shots to return, if the resultCOFID is known.
                if (_shotDictionary.TryGetValue( resultCOFID, out var shots ) && shots.Count > 0) {
                    return shots.Last();
                }
            }

            //Return null if the resultCOFID is not known or an empty list
            return null;
        }
        #endregion

        #region Protected and Private Methods
        private void LoadShot( Shot shot ) {

            _loadShotStopWatch.Start();
            ESTShotOperation operation = ESTShotOperation.NOTYETKNOWN;

            var resultCOF = shot.ResultCOFID;
            string stageLabel = shot.StageLabel;
            string sequenceStr = shot.Sequence.ToString();
            int updateNumber = shot.Update;

            if (!MatchProject.TryGetParticipantByResultCOFID( shot.ResultCOFID, out var participant ))
                operation = ESTShotOperation.UNKNOWNCOMPETITOR;

            //Pull the list of ESTShots for this participant. If the result COF ID is not yet known, create a new list and add it to the dictionary.
            List<Shot> listOfShots;
            if (!_shotDictionary.TryGetValue( resultCOF, out listOfShots )) {
                listOfShots = new List<Shot>();
                _shotDictionary[resultCOF] = listOfShots;
                _shotListMutexes[resultCOF] = new object();
            }

            object listOfShotsMutex = _shotListMutexes[resultCOF];

            lock (listOfShotsMutex) {
                //Determine if the shot is already known
                _isInListOfShotsStopWatch.Start();
                Shot? existingESTShot = null;
                foreach (var existingShot in listOfShots) {
                    if (existingShot.Sequence == shot.Sequence) {
                        //This means the past in shot is known, so it has to be a REDUNDANT, UPDATE, or DELETE
                        existingESTShot = existingShot;

                        //Quickly check if this is a redundant shot
                        if (existingShot.Update >= shot.Update) {
                            //This is an redudnat shot
                            operation = ESTShotOperation.REDUNDANT;
                            _logger.Warn( $"Shot update submitted redundantly. Result COF ID: {existingShot.ResultCOFID}, Sequence: {existingShot.Sequence}, Update: {existingShot.Update}." );
                            break;
                        }

                        if (shot.IsADeletedShot) {
                            operation = ESTShotOperation.DELETE;
                            break;
                        }

                        operation = ESTShotOperation.UPDATE;
                        break;
                    }
                }
                _isInListOfShotsStopWatch.Stop();

                if (operation == ESTShotOperation.NOTYETKNOWN) {
                    //Has to either be a NEW or INSERT
                    if (shot.IsASighter) {
                        operation = ESTShotOperation.SIGHTER;
                    } else if (listOfShots.Count > 0
                               && listOfShots[listOfShots.Count - 1].Sequence < shot.Sequence) {
                        operation = ESTShotOperation.NEW;
                    } else {
                        operation = ESTShotOperation.INSERT;
                    }
                }

                //To release the lock as soon as possible, will perform the add/update/delete operation on the list of shots while still in the lock, but will perform the add to all shots and the logging outside of the lock.
                switch (operation) {
                    case ESTShotOperation.NEW:
                        AddToListOfShots( listOfShots, shot );
                        break;

                    case ESTShotOperation.INSERT:
                        AddToListOfShots( listOfShots, shot );
                        break;

                    case ESTShotOperation.DELETE:
                        DeleteShot( listOfShots, shot, participant );
                        break;

                    case ESTShotOperation.UPDATE:
                        UpdateShot( listOfShots, existingESTShot, shot );
                        break;

                    case ESTShotOperation.SIGHTER:
                        AddToListOfShots( listOfShots, shot );
                        break;

                    case ESTShotOperation.REDUNDANT:
                        break;

                    case ESTShotOperation.NOTYETKNOWN:
                        break;

                    case ESTShotOperation.UNKNOWNCOMPETITOR:
                        AddToListOfShots( listOfShots, shot );
                        break;
                }
            }

            switch (operation) {
                case ESTShotOperation.NEW:
                    LogShot( shot );
                    AddToAllShots( shot );
                    AddScoringSystem( shot );
                    _loadShotStopWatch.Stop();
                    if (!_initializing)
                        OnShotAdded?.Invoke( this, new EventArgs<Shot>( shot ) );
                    break;

                case ESTShotOperation.INSERT:
                    LogShot( shot );
                    AddToAllShots( shot );
                    _loadShotStopWatch.Stop();
                    if (!_initializing)
                        OnShotAdded?.Invoke( this, new EventArgs<Shot>( shot ) );
                    break;

                case ESTShotOperation.DELETE:
                    LogShot( shot );
                    AddToAllShots( shot );
                    _loadShotStopWatch.Stop();
                    if (!_initializing)
                        OnShotDeleted?.Invoke( this, new EventArgs<Shot>( shot ) );
                    break;

                case ESTShotOperation.UPDATE:
                    LogShot( shot );
                    AddToAllShots( shot );
                    _loadShotStopWatch.Stop();
                    if (!_initializing)
                        OnShotUpdated?.Invoke( this, new EventArgs<Shot>( shot ) );
                    break;

                case ESTShotOperation.SIGHTER:
                    LogShot( shot );
                    AddToAllShots( shot );
                    _loadShotStopWatch.Stop();
                    if (!_initializing)
                        OnSighterReceived?.Invoke( this, new EventArgs<Shot>( shot ) );
                    break;

                case ESTShotOperation.REDUNDANT:
                    AddToAllShots( shot );
                    _loadShotStopWatch.Stop();
                    break;

                case ESTShotOperation.NOTYETKNOWN:
                    Debug.Assert( false, "ShotMapper LoadShot() was unable to figure out what operation to take on the past in shot ... WHICH SHOULDN'T HAPPEN" );
                    _logger.Error( "Was unable to figure out what operation to take on the past in shot ... WHICH SHOULDN'T HAPPEN" );
                    _loadShotStopWatch.Stop(); ;
                    break;

                case ESTShotOperation.UNKNOWNCOMPETITOR:
                    LogShot( shot );
                    AddToListOfShots( listOfShots, shot );
                    AddToAllShots( shot );
                    _logger.Warn( $"Can not add shot to visTargets, as the result cof id ({shot.ResultCOFID}) is not known to this match. This is likely a shot that hit a Hospital firing point." );
                    _loadShotStopWatch.Stop();
                    break;
            }
        }

        private void LogShot( Shot shot ) {
            if (_initializing || this.InMemoryOnly)
                return;

            /*
             * Because multiple threads could be calling LogShot(), and as such multiple threads
             * trying to write to the log file at once, not to mention the backup thread reading
             * the file occasionally, need to try and protect the writer, and ensure ever shot
             * can get logged. The strategy is to first put the shot to log into a thread safe
             * queue. Then dequeue everything and try and write. If there is an issue when writing
             * re-queue the shot and wait for the next change to write. 
             */

            writeToLogQueue.Enqueue( shot );
        }

        private void WriteToLogThread() {

            /*
             * Because multiple threads could be calling LogShot(), and as such multiple threads
             * trying to write to the log file at once, not to mention the backup thread reading
             * the file occasionally, need to try and protect the writer, and ensure ever shot
             * can get logged. The strategy is to first put the shot to log into a thread safe
             * queue. Then dequeue everything and try and write. If there is an issue when writing
             * re-queue the shot and wait for the next change to write. 
             */

            while (!_threadsShouldDie) {
                if (writeToLogQueue.TryDequeue( out var shot )) {
                    try {
                        using (StreamWriter file = File.AppendText( _shotLogFile.FullName )) {
                            var shotSerialized = JsonConvert.SerializeObject( shot, Scopos.BabelFish.Helpers.SerializerOptions.NewtonsoftJsonSerializerOneLine );
                            file.WriteLine( shotSerialized );
                        }
                    } catch (Exception ex) {
                        writeToLogQueue.Enqueue( shot );
                        _logger.Error( "Could not write Shot value to log. Will retry." );
                        _logger.Error( JsonConvert.SerializeObject( shot, Scopos.BabelFish.Helpers.SerializerOptions.NewtonsoftJsonSerializerOneLine ) );
                        _logger.Error( ex );
                    }
                }

                Thread.Sleep( 100 );
            }
        }

        private void AddToAllShots( Shot shot ) {
            _addToAllShotsStopWatch.Start();
            ConcurrentDictionary<float, ConcurrentDictionary<int, Shot>> sequ;
            if (!_allShots.TryGetValue( shot.ResultCOFID, out sequ )) {
                sequ = new ConcurrentDictionary<float, ConcurrentDictionary<int, Shot>>();
                _allShots[shot.ResultCOFID] = sequ;
            }

            ConcurrentDictionary<int, Shot> upd;
            if (!sequ.TryGetValue( shot.Sequence, out upd )) {
                upd = new ConcurrentDictionary<int, Shot>();
                sequ[shot.Sequence] = upd;
            }

            if (!upd.ContainsKey( shot.Update )) {
                upd[shot.Update] = shot;
            } else {
                if (!_suppressDuplicateShotLogging) {
                    _logger.Warn( $"Did not update shot, as it sequence and update already exists. Result COF ID: {shot.ResultCOFID}, Sequence: {shot.Sequence}, Update: {shot.Update}." );
                }
            }
            _addToAllShotsStopWatch.Stop();
        }

        private void AddToListOfShots( List<Shot> listOfShots, Shot shot ) {

            _addToListOfShotsStopWatch.Start();
            if (listOfShots.Count > 0) {
                //This is a new shot that needs to be added to the shotList, and then potentially re-sort the shotlist
                var sequenceOfLastShotAddedToList = listOfShots[listOfShots.Count - 1].Sequence;
                var needToSort = shot.Sequence > sequenceOfLastShotAddedToList;
                listOfShots.Add( shot );

                if (needToSort) {
                    _shotDictionary[shot.ResultCOFID] = listOfShots.OrderBy( o => o.Sequence ).ToList();
                }
            } else {
                listOfShots.Add( shot );
            }
            _addToListOfShotsStopWatch.Stop();
        }

        private void DeleteShot( List<Shot> listOfShots, Shot shotToDelete, MatchParticipant competitor ) {
            Debug.Assert( shotToDelete.IsADeletedShot, "The shot passed to DeleteShot should be marked as a deleted shot." );

            //Deleted shots should remain in the listOfShots, but obviously marked as deleted
            for (int i = 0; i < listOfShots.Count; i++) {
                var existingShot = listOfShots[i];
                if (existingShot.Sequence == shotToDelete.Sequence) {
                    listOfShots[i] = shotToDelete;
                    break;
                }
            }
        }

        private void UpdateShot( List<Shot> listOfShots, Shot existingShot, Shot updatedShot ) {

            //Update the shot in the shotDictionary
            var index = listOfShots.IndexOf( existingShot );
            listOfShots[index] = updatedShot;
        }

        public void AddScoringSystem( Shot shot ) {
            if (shot.Meta != null) {
                var dict = (IDictionary<string, object>)shot.Meta;
                if (dict.ContainsKey( "ESTSystem" )) {
                    string estSystemName = dict["ESTSystem"]?.ToString();
                    this.MatchProject.SetScoringSystem( ScoringSystem.EST, estSystemName );
                }
            }
        }

        public void Dispose() {
            this._threadsShouldDie = true;
        }
        #endregion
    }
}
