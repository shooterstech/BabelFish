using System.Collections.Concurrent;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// In the calculation of Rank Delta, comparing an athlete's rank now versus some previous time, we need to keep copies of the Result List 
    /// from the past in order to make this comparision. This class, ResultListSlidingWindow is in charge of keeping the past copies of the
    /// Result List, and then returning the appropirate one when asked. 
    /// <para>This class is intended to be a member of a <see cref="MatchProject"/>. It is written to be thread safe.</para>
    /// <para>This class works effectivley in one of two modes. Either "NOW" or "Sliding Window." With NOW which ever Result List the user
    /// specified last using .PushResultList() is the one that will be returned. With a Sliding Window, the youngest result list that is 
    /// older than the sliding windown time value (1, 3, or 5 minutes), is returned.</para>
    /// </summary>
    /// <remarks>
    /// The MODE is usually specified by the COURSE OF FIRE's Range Scripts, in the SegmentGroupCommand's .ResultEngineCompare property. </remarks>
    public class ResultListSlidingWindow {

        //Key is the Result List name concatenated with the course of fire Id. Value is the copy of the Result List to return the next time the user calls GetResultList().
        private ConcurrentDictionary<string, ResultList> _resultLists = new ConcurrentDictionary<string, ResultList>();

        //Key is the Result List name concatenated with the course of fire Id. Value is a queue used to store Result Lists for use in sliding winow calculations.
        private ConcurrentDictionary<string, ConcurrentQueue<ResultList>> _resultListsQueues = new ConcurrentDictionary<string, ConcurrentQueue<ResultList>>();

        //variables used to keep Mode thread safe.
        public object _modeMutex = new object();
        public ResultEngineCompareType _mode = ResultEngineCompareType.WINDOW_1_MINUTE;

        public ResultListSlidingWindow( MatchProject mp ) {
            this.MatchProject = mp;
        }

        /// <summary>
        /// Gets or Sets the Mode. 
        /// </summary>
        public ResultEngineCompareType Mode {
            get {
                lock (_modeMutex) {
                    return _mode;
                }
            }
            set {
                lock (_modeMutex) {
                    _mode = value;
                }
            }
        }

        /// <summary>
        /// Readonly backwards pointer to the Match Project that contains this ResultListSlidingWindow.
        /// </summary>
        public MatchProject MatchProject { get; private set; }

        /// <summary>
        /// When a Result List gets generated, the user should push the Result List into the ResultListSlidingWindow using this method.
        /// <para>When the MODE is NOW, the next time the user calls GetResultList() this Result List is returned.</para>
        /// <para>When the MODE is a sliding window (1, 3, or 5 mins), the youngest Result List that is older then the requested
        /// time span is returned.</para>
        /// </summary>
        /// <param name="resultList"></param>
        public void PushResultList( ResultList resultList ) {
            var dictionaryKey = $"{resultList.ResultName} {resultList.CourseOfFireId}";
            switch (Mode) {
                case ResultEngineCompareType.NOW:
                    //When the Mode is NOW, we can copy the Result List directly to _resultLists for immediate return on GetResultList().
                    _resultLists.AddOrUpdate( dictionaryKey, resultList, ( key, oldValue ) => resultList );
                    break;

                case ResultEngineCompareType.WINDOW_1_MINUTE:
                case ResultEngineCompareType.WINDOW_3_MINUTE:
                case ResultEngineCompareType.WINDOW_5_MINUTE:
                    //When the Mode is a sliding window, the strategy is to push the Result List into the queue. 

                    ConcurrentQueue<ResultList> queue;
                    //Make sure we have a queue for the specified result list
                    if (!_resultListsQueues.ContainsKey( dictionaryKey )) {
                        queue = new ConcurrentQueue<ResultList>();
                        _resultListsQueues.AddOrUpdate( dictionaryKey, queue, ( key, oldValue ) => queue );
                    }

                    //pull the queue we should push to. While this should always work, protect against it failing just in case
                    if (_resultListsQueues.TryGetValue( dictionaryKey, out queue )) {
                        queue.Enqueue( resultList );
                    }

                    //To avoid memory leaks, remove Result Lists that are too old.
                    DequeueToSize( dictionaryKey );

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Get the Result List to use during the next comparison, to calculate the RankDelta.
        /// </summary>
        /// <remarks>
        /// RankDelta is calculated by the ResultEngine. Its the last step in the SortAsync() method.</remarks>
        /// <param name="resultListName"></param>
        /// <returns></returns>
        public ResultList GetResultListToCompareAgainst( ResultList resultList ) {
            var dictionaryKey = $"{resultList.ResultName} {resultList.CourseOfFireId}";

            //Call DequeueToSize to make sure we have the correct result list to compare against.
            switch (Mode) {

                case ResultEngineCompareType.WINDOW_1_MINUTE:
                case ResultEngineCompareType.WINDOW_3_MINUTE:
                case ResultEngineCompareType.WINDOW_5_MINUTE:
                    DequeueToSize( dictionaryKey );

                    break;

                default:
                    //Doesn't need to be called for MODE == NOW
                    break;
            }

            //Return the Result List to the user.
            if (_resultLists.TryGetValue( dictionaryKey, out ResultList rl )) {
                return rl;
            }

            return null;
        }

        /// <summary>
        /// Private function to remove any Result Lists that are too old, according to the time span set in MODE.
        /// And to populate _resultLists with the current Result List to use in comparison.
        /// </summary>
        /// <param name="resultListName"></param>
        private void DequeueToSize( string dictionaryKey ) {

            ConcurrentQueue<ResultList> queue;
            ResultList resultList;
            double maxAgeInMinutes = 5;

            switch (Mode) {
                case ResultEngineCompareType.NOW:
                case ResultEngineCompareType.NONE:
                    return;

                case ResultEngineCompareType.WINDOW_1_MINUTE:
                    maxAgeInMinutes = 1;
                    break;
                case ResultEngineCompareType.WINDOW_3_MINUTE:
                    maxAgeInMinutes = 3;
                    break;
                case ResultEngineCompareType.WINDOW_5_MINUTE:
                default:
                    maxAgeInMinutes = 5;
                    break;
            }

            int myCount = 0; //This is a failsafe, so we don't get into an infinite loop 

            //pull the queue we should pull from. While this should always work, protect against it failing just in case
            if (_resultListsQueues.TryGetValue( dictionaryKey, out queue )) {
                while (queue.Count > 1 && queue.TryPeek( out resultList )) {
                    var ageInMinutes = (DateTime.UtcNow - resultList.LastUpdated).TotalMinutes;
                    if (ageInMinutes > maxAgeInMinutes) {
                        //Remove anything from the queue that is older than the timespan specified in the MODE.
                        queue.TryDequeue( out resultList );
                        //The last one we dequeue, will be the youngest result list that's older than the specified time span
                        _resultLists.AddOrUpdate( dictionaryKey, resultList, ( key, oldValue ) => resultList );
                    } else {
                        break;
                    }

                    if (myCount++ > 20)
                        break;
                }
            }
        }

        /// <summary>
        /// Removes all Result Lists.
        /// </summary>
        public void ClearCache() {
            _resultLists.Clear();
            _resultListsQueues.Clear();
        }
    }
}
