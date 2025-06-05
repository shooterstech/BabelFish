using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
	public static class ResultListSlidingWindow  {

		//Key is the Result List name. Value is the copy of the Result List to use in the "NOW" comparison.
		private static ConcurrentDictionary<string, ResultList> _resultLists = new ConcurrentDictionary<string, ResultList>();

		private static ConcurrentDictionary<string, ConcurrentQueue<ResultList>> _resultListsQueues = new ConcurrentDictionary<string, ConcurrentQueue<ResultList>>();


		public static ResultEngineCompareType Mode { get; set; } = ResultEngineCompareType.NOW;

		public static void PushResultList( ResultList resultList)  {
			switch( Mode) {
				case ResultEngineCompareType.NOW:
					_resultLists.AddOrUpdate( resultList.ResultName, resultList, ( key, oldValue ) => resultList );
					break;

				case ResultEngineCompareType.WINDOW_1_MINUTE:
				case ResultEngineCompareType.WINDOW_3_MINUTE:
				case ResultEngineCompareType.WINDOW_5_MINUTE:
					ConcurrentQueue<ResultList> queue;
					//Make sure we have a queue for the specified result list
					if (!_resultListsQueues.ContainsKey( resultList.ResultName )) {
						queue = new ConcurrentQueue<ResultList>();
						_resultListsQueues.AddOrUpdate( resultList.ResultName, queue, ( key, oldValue ) => queue );
					}

					//pull the queue we should push to. While this should always work, protect against it failing just in case
					if ( _resultListsQueues.TryGetValue( resultList.ResultName, out queue ) ) {
						queue.Enqueue( resultList );
					}

					DequeueToSize( resultList.ResultName );

					break;

				default:
					break;
			}
		}

		public static ResultList GetResultList( string resultListName ) {

			ResultList resultList;
			switch (Mode) {
				case ResultEngineCompareType.NOW:
					if (_resultLists.TryGetValue( resultListName, out resultList )) { 
						return resultList; 
					}
					break;

				case ResultEngineCompareType.WINDOW_1_MINUTE:
				case ResultEngineCompareType.WINDOW_3_MINUTE:
				case ResultEngineCompareType.WINDOW_5_MINUTE:
					ConcurrentQueue<ResultList> queue;

					DequeueToSize( resultListName );

					//pull the queue we should push to. While this should always work, protect against it failing just in case
					if (_resultListsQueues.TryGetValue( resultListName, out queue )
						&& queue.TryPeek( out resultList )) {
						return resultList;
					}

					break;

				default:
					break;
			}

			return null;
		}

		private static void DequeueToSize( string resultListName ) {

			ConcurrentQueue<ResultList> queue;
			ResultList resultList;
			double maxAgeInMinutes = 5;

			switch (Mode) {
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
			if (_resultListsQueues.TryGetValue( resultListName, out queue )) {
				while (queue.Count > 1 && queue.TryPeek( out resultList )) {
					var ageInMinutes = (DateTime.UtcNow - resultList.LastUpdated).TotalMinutes;
					if (ageInMinutes > maxAgeInMinutes) {
						queue.TryDequeue( out resultList );
					} else {
						break;
					}

					if (myCount++ > 20)
						break;
				}
			}
		}

		public static void ClearCache() {
			_resultLists.Clear();
			_resultListsQueues.Clear();
		}
	}
}
