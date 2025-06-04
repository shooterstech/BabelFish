using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
	public static class ResultListSlidingWindow  {

		private static ConcurrentDictionary<string, ResultList> _resultLists = new ConcurrentDictionary<string, ResultList>();


		public static ResultListCompareType Mode { get; set; } = ResultListCompareType.NOW;

		public static void PushResultList( ResultList resultList)  {
			_resultLists.AddOrUpdate( resultList.ResultName, resultList, ( key, oldValue ) => resultList );
		}

		public static ResultList GetResultList( string resultListName ) {
			if ( _resultLists.TryGetValue( resultListName, out ResultList resultList ) )
				{ return resultList; }

			return null;
		} 

		public static void ClearCache() {
			_resultLists.Clear();
		}
	}
}
