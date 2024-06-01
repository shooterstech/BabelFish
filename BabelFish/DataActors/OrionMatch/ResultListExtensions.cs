using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public static class ResultListExtensions {

        /// <summary>
        /// Determines, but does not set, the Status of the ResultList. Rules for it are:
        /// * If the match's status is OFFICIAL, so is the ResultList.
        /// * If not participants are in the ResultList then the status if FUTURE
        /// * If all participant's event status is FUTURE, so is the ResultList
        /// * If one or more participant's event status is INTERMEDIATE, so is the ResultList
        /// * If one or more participant's event status is UNOFFICIAL, and no one's is INTERMEDIATE, then result status is UNOFFICIAL
        /// * Default case is, result status is equal to the match's status
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="matchStatus"></param>
        /// <returns></returns>
        public static ResultStatus CalculateResultListStatus( this ResultList resultList, ResultStatus matchStatus ) {

            if (matchStatus == ResultStatus.OFFICIAL)
                return ResultStatus.OFFICIAL;

            if (resultList.Items.Count == 0)
                return ResultStatus.FUTURE;

            if (string.IsNullOrEmpty( resultList.EventName ))
                return matchStatus;

            bool allAreFuture = true;
            bool oneIsIntermediate = false;
            bool oneIsUnofficial = false;

            EventScore eventScore;
            foreach( var re in  resultList.Items ) {
                if ( re.EventScores.TryGetValue( resultList.EventName, out eventScore ) ) {
                    allAreFuture &= (eventScore.Status == ResultStatus.FUTURE);
                    oneIsIntermediate |= (eventScore.Status == ResultStatus.INTERMEDIATE);
                    oneIsUnofficial |= (eventScore.Status == ResultStatus.UNOFFICIAL);
                }
            }

            if (allAreFuture) { return ResultStatus.FUTURE; }

            if (oneIsIntermediate) {  return ResultStatus.INTERMEDIATE; }

            if (oneIsUnofficial) {  return ResultStatus.UNOFFICIAL; }

            return resultList.Status;
        }
    }
}
