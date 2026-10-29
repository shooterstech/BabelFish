using System.Collections.Concurrent;
using System.Diagnostics;
using Newtonsoft.Json;
using Scopos.BabelFish.DataActors.Definitions;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

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

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _initializing = false;
        private FileInfo _shotLogFile;
        private volatile bool _threadsShouldDie = false;
        private DateTime _clearLastShotBeforeThisUTCTime = DateTime.MinValue;

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

        /// <summary>
        /// Reads the MatchProject's athenaShots log file line by line, and for each line,
        /// deserializes it into a Shot object and loads it into the ShotMapper using the LoadShot method.
        /// <para>Expected to be called by the MatchProject during it's load from file.</para>
        /// </summary>
        public void LoadFromFile() {
            var filePath = Path.Combine( MatchProject.ProjectDirectory.FullName, "athenaShots.json" );
            if (!File.Exists( filePath ))
                return;

            // Turn off writing to the athenaShots log file while we are loading in shots from the athenaShots log file to avoid us writing redundant shot updates
            _initializing = true;

            // Read the athenaShots log file line by line, and for each line, deserialize it into a Shot object and load it into the ShotMapper using the LoadShot method.
            foreach (var line in File.ReadLines( filePath )) {
                if (string.IsNullOrWhiteSpace( line ))
                    continue;
                try {
                    var shot = System.Text.Json.JsonSerializer.Deserialize<Shot>( line, SerializerOptions.SystemTextJsonDeserializer );
                    if (shot != null)
                        LoadShot( shot );
                } catch (Exception ex) {
                    _logger.Error( ex, $"Failed to deserialize shot from line: {line}" );
                }
            }

            // Reenable writing to the athenaShots log file now that we have finished loading in shots from the athenaShots log file
            _initializing = false;
        }
        #endregion

        #region Events

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
        /// Interface to recieve a complete list of shots for a given Result COF ID from an external source. This is intended to be used for
        /// receiving shots from non-EST systems, such as manual entry or Orion's VIS.
        ///
        /// <para>The expectation is that the completeShotList passed into
        /// this method is the complete and up to date list of shots for the given Result COF ID at the time of the call, and should replace any
        /// existing shots for that Result COF ID that the ShotMapper currently has. The ShotMapper will validate that all shots in the
        /// completeShotList have the same Result COF ID as the resultCofId parameter, and that all shots have a Sequence number greater than 0 and that
        /// there are no duplicate Sequence numbers. If any shot in the completeShotList fails validation, it will be skipped and not added to the ShotMapper,
        /// but the rest of the shots in the completeShotList will still be processed. Shots received through this method will be marked with
        /// a custom attribute "EXTERNALLY_SCORED" to indicate that they were received from an external source and not through the normal EST shot receiving process.
        /// </para>
        /// <para>ShotMapper does not persist (save to file) Shots passed in via ReceiveExternallyScoredShots(). Caller is responsible for saving
        /// these shots to persistant storage. </para>
        /// </summary>
        /// <param name="resultCofId"></param>
        /// <param name="completeShotList"></param>
        public void ReceiveExternallyScoredShots( string resultCofId, List<Shot> completeShotList ) {

            //Clear the shots that we currently have for this result COF ID, both in the _allShots and the _shotDictionary
            if (_allShots.TryGetValue( resultCofId, out var shotsForResultCofId )) {
                shotsForResultCofId.Clear();
            }
            if (_shotDictionary.TryGetValue( resultCofId, out var shotListForResultCofId )) {
                shotListForResultCofId.Clear();
            }

            List<Shot> validatedShots = new List<Shot>();
            HashSet<float> sequenceNumbers = new HashSet<float>();
            foreach (var shot in completeShotList) {
                if (shot.ResultCOFID != resultCofId) {
                    var msg = $"Received shot with Result COF ID {shot.ResultCOFID} in ReceiveExternallyScoredShots for Result COF ID {resultCofId}. This should not happen, as all shots in the completeShotList should have the same Result COF ID as the one passed into the method.";
                    Debug.Fail( msg );
                    _logger.Error( msg );
                    continue;
                }

                if (shot.Sequence <= 0) {
                    var msg = $"Received shot with Sequence {shot.Sequence} in ReceiveExternallyScoredShots for Result COF ID {resultCofId}. This should not happen, as all shots in the completeShotList should have a Sequence greater than 0.";
                    Debug.Fail( msg );
                    _logger.Error( msg );
                    continue;
                } else if (sequenceNumbers.Contains( shot.Sequence )) {
                    var msg = $"Received multiple shots with the same Sequence {shot.Sequence} in ReceiveExternallyScoredShots for Result COF ID {resultCofId}. This should not happen, as all shots in the completeShotList should have unique Sequence numbers.";
                    Debug.Fail( msg );
                    _logger.Error( msg );
                    continue;
                } else {
                    sequenceNumbers.Add( shot.Sequence );
                }

                shot.AddAttribute( Shot.EXTERNALLY_SCORED );
                validatedShots.Add( shot );
            }

            var mutex = _shotListMutexes.GetOrAdd( resultCofId, new object() );
            lock (mutex) {
                _shotDictionary[resultCofId] = validatedShots.OrderBy( o => o.Sequence ).ToList();
            }

            foreach (var shot in validatedShots) {
                _allShots.GetOrAdd( resultCofId, new ConcurrentDictionary<float, ConcurrentDictionary<int, Shot>>() )
                    .GetOrAdd( shot.Sequence, new ConcurrentDictionary<int, Shot>() )
                    .AddOrUpdate( shot.Update, shot, ( updateNumber, existingShot ) => shot );
            }
        }

        /// <summary>
        /// Returns a set of <see cref="Shot">Shots</see> fired by a participant for the passed in Result COF ID.
        /// <para>Key is the sequence number of the shot represented as a string.</para>
        /// <para>Value is the Shot.</para>
        /// <para>Intended to be used to populate the Shots dictionary in <see cref="IEventScores"/>.</para>
        /// <para>Returned dictionary is empty, if the passed in resultCOFID is not known.</para>
        /// </summary>
        public async Task<Dictionary<string, Shot>> GetShotsBySequenceAsync( string resultCOFID, bool includeSighters = true ) {

            var shotDictionaryToReturn = new Dictionary<string, Shot>();
            CourseOfFireEntryIndividual entry;
            if (!MatchProject.TryGetCourseOfFireEntryByResultCOFID( resultCOFID, out entry )) {
                return shotDictionaryToReturn;
            }

            //Look up the Course of Fire Structure for this course of fire entry. If the course of fire structure, which should not happen can not be found, return an empty dictionary.
            CourseOfFireStructure cofStructure;
            if (!MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( entry.CourseOfFireId, out cofStructure )) {
                Debug.Fail( $"Was not able to find Course of Fire Structure for Course of Fire Entry with Result COF ID {resultCOFID}. This should not happen, as the Course of Fire Entry should not have been able to be created without a Course of Fire Structure." );
                return shotDictionaryToReturn;
            }

            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );

            // We will need to look up stages based on the StageLabel.
            // Key is the StageLabel, value is the list of leafs (singular events) under the stage.
            Dictionary<string, Queue<EventComposite>> stageLabelLookup = new Dictionary<string, Queue<EventComposite>>();
            var tempQueue = new Queue<EventComposite>();
            foreach (var singular in topLevelEvent.GetAllSingulars()) {
                if (!stageLabelLookup.TryGetValue( singular.StageLabel, out tempQueue )) {
                    tempQueue = new Queue<EventComposite>();
                    stageLabelLookup[singular.StageLabel] = tempQueue;
                }
                tempQueue.Enqueue( singular );
            }

            //First get the mutex for the Shot List for this Result COF ID. If it doesn't exist, create it.
            var mutex = _shotListMutexes.GetOrAdd( resultCOFID, new object() );
            lock (mutex) {
                //Generate the dictionary of shots to return, if the resultCOFID is known.
                if (_shotDictionary.TryGetValue( resultCOFID, out var shots )) {

                    foreach (var shot in shots) {
                        if (!includeSighters && shot.IsASighter) {
                            continue;
                        }

                        shotDictionaryToReturn[shot.Sequence.ToString()] = shot;

                        var stageLabel = shot.StageLabel;
                        if ((stageLabelLookup.TryGetValue( stageLabel, out Queue<EventComposite> queue )
                            && queue.Count > 0)) {
                            var singular = queue.Dequeue();
                            shot.EventName = singular.EventName;
                        } else {
                            shot.EventName = string.Empty;
                        }
                    }

                    return shotDictionaryToReturn;
                }
            }

            //Return an empty dictionary if the resultCOFID is not known.
            return new Dictionary<string, Shot>();
        }

        public async Task<Dictionary<string, Shot>> GetShotsByEventNameAsync( string resultCOFID ) {

            var shotsBySequence = await this.GetShotsBySequenceAsync( resultCOFID );
            var shotsByEventName = new Dictionary<string, Shot>();
            foreach (var shot in shotsBySequence.Values) {
                if (!string.IsNullOrEmpty( shot.EventName )) {
                    shotsByEventName[shot.EventName] = shot;
                }
            }
            return shotsByEventName;
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
        public async Task<Dictionary<string, EventScore>> GetEventScoresAsync( string resultCOFID ) {

            // Run GetShotsByEventNameAsync as a task that we will await later. Hopefully spending things up a bit.
            var shotsByEventNameTask = this.GetShotsByEventNameAsync( resultCOFID );

            //Look up the participant for this result COF ID. If the result COF ID is not known, return an empty dictionary.
            CourseOfFireEntryIndividual entry;
            var eventScores = new Dictionary<string, EventScore>();
            if (!MatchProject.TryGetCourseOfFireEntryByResultCOFID( resultCOFID, out entry )) {
                return eventScores;
            }

            //Look up the Course of Fire Structure for this course of fire entry. If the course of fire structure, which should not happen can not be found, return an empty dictionary.
            CourseOfFireStructure cofStructure;
            if (!MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( entry.CourseOfFireId, out cofStructure )) {
                Debug.Fail( $"Was not able to find Course of Fire Structure for Course of Fire Entry with Result COF ID {resultCOFID}. This should not happen, as the Course of Fire Entry should not have been able to be created without a Course of Fire Structure." );
                return eventScores;
            }

            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var scoreFormatCollectionDefinition = await cofDefinition.GetScoreFormatCollectionDefinitionAsync();
            var scoreConfigName = cofStructure.ScoreConfigName;
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );
            var shotsByEventName = await shotsByEventNameTask;
            var remarkList = entry.RemarkList;

            // Calculate the Score for the standard Event Tree
            CalculateScore( eventScores, shotsByEventName, topLevelEvent, scoreFormatCollectionDefinition, scoreConfigName );

            foreach (var externalEvent in EventComposite.FindExternalEvents( cofDefinition )) {
                CalculateScore( eventScores, shotsByEventName, externalEvent.Value, scoreFormatCollectionDefinition, scoreConfigName );
            }

            CheckForRemarks( eventScores, entry );
            await CalculateEventStatusAsync( eventScores, entry );
            await CalculateEventAndStageStyleAsync( eventScores, entry, cofStructure );

            return eventScores;
        }

        /// <summary>
        /// Returns the last <see cref="Shot"/> fired by a participant identified by the Result COF ID.
        /// <para>Intended to be used to populate the Shots dictionary in <see cref="IEventScores"/>.</para>
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>The last <see cref="Shot"/> fired by the participant, or null if no shots are found.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Shot? GetLastShot( string resultCOFID, bool filterLastShotByTimeScored = false ) {

            //First get the mutex for the Shot List for this Result COF ID. If it doesn't exist, create it.
            var mutex = _shotListMutexes.GetOrAdd( resultCOFID, new object() );
            lock (mutex) {
                //Generate the dictionary of shots to return, if the resultCOFID is known.
                if (_shotDictionary.TryGetValue( resultCOFID, out var shots ) && shots.Count > 0) {
                    var lastShot = shots.Last();

                    //If the user didn't ask us to filter the last shot by time scored, then we will just return the last shot.
                    if (!filterLastShotByTimeScored) {
                        return lastShot;
                    }

                    // We will do two filtering checks to see if we return the lasst shot or not.
                    // first, if the last shot was scored since the _clearLastShotBeforeThisUTCTime. This is to handle the case that Range Control clears the last shot values due to a Segment Group change.
                    // Second, if the last shot was fired within five minutes.
                    if ((lastShot.TimeScored.ToUniversalTime() > this._clearLastShotBeforeThisUTCTime)
                        && (lastShot.TimeScored.ToUniversalTime() > DateTime.UtcNow.AddMinutes( -5 ))) {
                        return lastShot;
                    }
                }
            }

            //Return null if the resultCOFID is not known or an empty list
            return null;
        }
        #endregion

        #region Protected and Private Methods
        private Score CalculateScore( Dictionary<string, EventScore> eventScores,
            Dictionary<string, Shot> shotsByEventName,
            EventComposite eventComponent,
            ScoreFormatCollection scoreFormatCollectionDefinition,
            string scoreConfigName ) {

            // Test if the score is already calculated for this event. This is to handle the case where an event is a child of multiple parent events (which would happen for an Event outside the Event Tree)
            if (eventScores.TryGetValue( eventComponent.EventName, out var existingEventScore )) {
                return existingEventScore.Score;
            }

            if (eventComponent.EventType == EventtType.SINGULAR) {
                //NOTE: As this is a shot, we dont' add it to the eventScores dictionary.
                if (shotsByEventName.TryGetValue( eventComponent.EventName, out var shot )) {
                    var score = shot.Score;
                    score.NumShotsFired = 1;
                    return score;
                } else {
                    //This means there were no shots fired for this singular event, so we will return a zero score.
                    return new Score();
                }
            } else {
                CalculationVariableScoreComponent calculationVariable;
                switch (eventComponent.Calculation) {
                    case EventCalculation.SUM:
                        Score summation = new Score();
                        for (int i = 0; i < eventComponent.Children.Count; i++) {
                            var child = eventComponent.Children[i];

                            // Determine how the child's score shold be added into the summation. Which is determiend by the CalculationVariable at the same index as the child.
                            // If there is not a CalculationVariable at the same index as the child, or if the CalculationVariable is not of type CalculationVariableScoreComponent,
                            // then we will default to adding the child's score into the summation using ScoreComponent.S.
                            if (eventComponent.CalculationVariables != null
                                && eventComponent.CalculationVariables.Count > i
                                && eventComponent.CalculationVariables[i] is CalculationVariableScoreComponent) {
                                calculationVariable = (CalculationVariableScoreComponent)eventComponent.CalculationVariables[i];
                            } else {
                                calculationVariable = new CalculationVariableScoreComponent() { Value = ScoreComponent.S };
                            }

                            //Find out the score for the child, and add it into the summation according to the CalculationVariable.
                            var childScore = CalculateScore( eventScores, shotsByEventName, child, scoreFormatCollectionDefinition, scoreConfigName );
                            summation.Add( childScore, calculationVariable.Value );
                        }

                        eventScores[eventComponent.EventName] = new EventScore() {
                            Score = summation,
                            EventName = eventComponent.EventName,
                            EventType = eventComponent.EventType,
                            ScoreFormatted = Helpers.StringFormatting.FormatScore( scoreFormatCollectionDefinition, scoreConfigName, eventComponent.ScoreFormat, summation ),
                            NumShotsFired = summation.NumShotsFired
                        };
                        return summation;

                    case EventCalculation.AVERAGE:
                        Debug.Fail( $"Have not implemented calculation for EventCalculation type {eventComponent.Calculation}. Likely because I haven't gotten around to it." );
                        throw new NotImplementedException();
                        break;

                    default:
                        Debug.Fail( $"Have not implemented calculation for EventCalculation type {eventComponent.Calculation}. Likely because this method is deprecated." );
                        throw new NotImplementedException();
                        break;

                }
            }
        }

        private async Task CalculateEventStatusAsync( Dictionary<string, EventScore> eventScores, CourseOfFireEntryIndividual entry ) {
            if (entry.MatchParticipant is null) {
                var msg = $"Course of Fire Entry with Result COF ID {entry.ResultCofId} does not have a Match Participant assigned. This should not happen, as a Match Participant should have been assigned before the Course of Fire Entry was created.";
                Debug.Fail( msg );
                _logger.Error( msg );
                return;
            }

            if (entry.MatchParticipant.Project is null) {
                var msg = $"Match Participant with Participant ID {entry.MatchParticipant.ParticipantID} does not have a Match Project assigned. This should not happen, as the Match Participant should have been assigned to a Match Project before the Course of Fire Entry was created.";
                Debug.Fail( msg );
                _logger.Error( msg );
                return;
            }

            var participant = entry.MatchParticipant;
            var project = participant.Project;
            var match = project.Match;
            CourseOfFireStructure cofStructure;
            if (!match.MatchStructure.TryGetCourseOfFireStructure( entry.CourseOfFireId, out cofStructure )) {
                var msg = $"Was not able to find Course of Fire Structure for Course of Fire Entry with Result COF ID {entry.ResultCofId}. This should not happen, as the Course of Fire Entry should not have been able to be created without a Course of Fire Structure.";
                Debug.Fail( msg );
                _logger.Error( msg );
                return;
            }
            var cofIsOfficial = cofStructure.Official;
            var remarkList = entry.RemarkList;
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );
            var lastShot = this.GetLastShot( entry.ResultCofId );

            foreach (var es in eventScores) {
                var eventName = es.Key;
                var eventScore = es.Value;

                //If the COF's status is official, then so to are all evetns
                if (cofIsOfficial) {
                    eventScore.Status = ResultStatus.OFFICIAL;
                    continue;
                }

                if ((lastShot != null && (DateTime.UtcNow - lastShot.TimeScored.ToUniversalTime()).TotalHours > 1.0) ||
                     (remarkList.HasNonCompletionRemark)) {
                    eventScore.Status = ResultStatus.UNOFFICIAL;
                    continue;
                }

                //If shots have not been fired yet, then status if future
                var numberOfShotsFired = eventScore.NumShotsFired;
                if (numberOfShotsFired == 0) {
                    eventScore.Status = ResultStatus.FUTURE;
                    continue;
                }

                var @event = topLevelEvent.FindEventComposite( eventName );
                if (@event != null) {
                    var numberOfShotsExpected = @event.GetAllSingulars().Count();

                    //if the number of shots fired is equal to expected number of shots
                    if (numberOfShotsFired >= numberOfShotsExpected) {
                        eventScore.Status = ResultStatus.UNOFFICIAL;
                        continue;
                    } else if (numberOfShotsFired > 0) {
                        //if shots have been fired, but not yet complete
                        eventScore.Status = ResultStatus.INTERMEDIATE;
                        continue;
                    }
                }

                //I dont' thinnk we would ever get here, but if we do, we will default to official status.
                eventScore.Status = ResultStatus.OFFICIAL;
            }
        }

        /// <summary>
        /// Checks if the CourseOfFireEntry has any remarks that would impact the EventScores, such as a DSQ remark.
        /// If such a remark is found, the EventScores are updated accordingly, such as making the score zero for a DSQ remark.
        /// </summary>
        /// <param name="eventScores"></param>
        /// <param name="entry"></param>
        private void CheckForRemarks( Dictionary<string, EventScore> eventScores, CourseOfFireEntryIndividual entry ) {
            if (entry.RemarkList.IsShowingParticipantRemark( ParticipantRemark.DSQ )) {
                foreach (var es in eventScores) {
                    es.Value.Score.MakeScoreZero();
                    es.Value.ScoreFormatted = string.Empty; //Choosing not to format the score using StringFormatting.FormatScore().
                }
            }
        }

        private async Task CalculateEventAndStageStyleAsync( Dictionary<string, EventScore> eventScores,
            CourseOfFireEntryIndividual entry,
            CourseOfFireStructure cofStructure ) {

            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var eventAndStageStyleMappingDefinition = await cofDefinition.GetEventAndStageStyleMappingDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );
            var targetCollectionName = cofStructure.TargetCollectionName;
            var calculator = new EventAndStageStyleMappingCalculation( eventAndStageStyleMappingDefinition );

            // The COF's RequiredAttribute is the one to use to look up and Participant's Attribute Value Applelation.
            var attrSetName = cofDefinition.RequiredAttributeDef;

            var attrValue = await entry.MatchParticipant.Participant.GetAttributeValueAsync( attrSetName, entry.CourseOfFireId );
            var attrValueappellation = attrValue?.AttributeValue.AttributeValueAppellation ?? string.Empty;

            foreach (var es in eventScores) {
                var eventName = es.Key;
                var eventScore = es.Value;
                if (eventScore.EventType == EventtType.EVENT) {
                    var eventMapping = topLevelEvent.FindEventComposite( eventName ).EventStyleMapping;
                    var eventStyleDef = calculator.GetEventStyleDef( attrValueappellation, targetCollectionName, eventMapping );
                    eventScore.EventStyleDef = eventStyleDef;
                } else if (eventScore.EventType == EventtType.STAGE) {
                    var stageMapping = topLevelEvent.FindEventComposite( eventName ).StageStyleMapping;
                    var stageStyleDef = calculator.GetStageStyleDef( attrValueappellation, targetCollectionName, stageMapping );
                    eventScore.StageStyleDef = stageStyleDef;
                }
            }
        }

        private void LoadShot( Shot shot ) {

            _loadShotStopWatch.Start();
            ESTShotOperation operation = ESTShotOperation.NOTYETKNOWN;

            var resultCOF = shot.ResultCOFID;
            string stageLabel = shot.StageLabel;
            string sequenceStr = shot.Sequence.ToString();
            int updateNumber = shot.Update;

            if (!MatchProject.TryGetMatchParticipantByResultCOFID( shot.ResultCOFID, out var participant ))
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
                    if (!_initializing) {
                        OnShotAdded?.Invoke( this, new EventArgs<Shot>( shot ) );
                        OnSighterReceived?.Invoke( this, new EventArgs<Shot>( shot ) );
                    }
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

            do {
                while (writeToLogQueue.TryDequeue( out var shot )) {
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

                if (!_threadsShouldDie) {
                    Thread.Sleep( 100 );
                }

            } while (!_threadsShouldDie);
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
            var resultCofId = shot.ResultCOFID;
            if (this.MatchProject.TryGetCourseOfFireEntryByResultCOFID( resultCofId, out var entry )) {

                if (shot.Meta != null) {
                    var dict = (IDictionary<string, object>)shot.Meta;
                    if (dict.ContainsKey( "ESTSystem" )) {
                        string estSystemName = dict["ESTSystem"]?.ToString();
                        this.MatchProject.SetScoringTechnology( entry.CourseOfFireId, ScoringSystem.EST, estSystemName );
                    } else if (dict.ContainsKey( "TargetReadingMachine" )) {
                        string scoringSystemName = dict["TargetReadingMachine"]?.ToString();
                        this.MatchProject.SetScoringTechnology( entry.CourseOfFireId, ScoringSystem.TARGET_READING_MACHINE, scoringSystemName );
                    } else if (dict.ContainsKey( "Manual" )) {
                        this.MatchProject.SetScoringTechnology( entry.CourseOfFireId, ScoringSystem.MANUAL, null );
                    } else {
                        this.MatchProject.SetScoringTechnology( entry.CourseOfFireId, ScoringSystem.UNKNOWN, null );
                    }
                }
            }
        }

        /// <summary>
        /// Event Handler for the Range Control when there is a segment group change.
        /// <para>The intent is to clear last shot after each segment group change. Which will be more clear online to spectators.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        public void SegmentGroupChanged( object sender, EventArgs<SegmentGroup> ea ) {
            _clearLastShotBeforeThisUTCTime = DateTime.UtcNow;
        }

        public void Dispose() {
            this._threadsShouldDie = true;
        }
        #endregion
    }
}
