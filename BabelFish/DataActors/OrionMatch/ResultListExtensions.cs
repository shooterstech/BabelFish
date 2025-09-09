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

            if (matchStatus == ResultStatus.OFFICIAL)// || (DateTime.Now - resultList.LastUpdated).TotalHours < 1)
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


        /// <summary>
        /// Checks for the unusual condition that the returned result list is ordered by projected rank, 
        /// but the status is UNOFFICIAL or OFFICIAL. This could happen if a user turns Orion off, 
        /// before having all scores turned in, and we're past the end date of the match.
        /// </summary>
        public static void ReSortIfOfficial(this ResultList resultList) {

            if (resultList is not null
                && resultList.Projected &&
                (resultList.Status == ResultStatus.UNOFFICIAL || resultList.Status == ResultStatus.OFFICIAL)) {
                var sorter = new CompareResultByRank( CompareResultByRank.CompareMethod.RANK_ORDER, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
                resultList.Items.Sort( sorter );

                resultList.Projected = false;
            }
        }

        /// <summary>
        /// Determines the status of an Event for an Athlete. This method does not work for Teams
        /// </summary>
        /// <param name="eventScore"></param>
        /// <param name="matchStatus"></param>
        /// <param name="lastShot"></param>
        /// <param name="numberOfShotsFired"></param>
        /// <param name="topLevelEvent"></param>
        /// <param name="eventName"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public static void SetEventStatus( this EventScore eventScore, ResultStatus matchStatus, Scopos.BabelFish.DataModel.Athena.Shot.Shot lastShot, int numberOfShotsFired, Scopos.BabelFish.DataModel.Definitions.EventComposite topLevelEvent, string eventName, RemarkList remarkList)
        {
            //If the match's status is official, then so to are all evetns
            if (matchStatus == ResultStatus.OFFICIAL)
            {
                eventScore.Status = ResultStatus.OFFICIAL;
                return;
            }

            if ((lastShot != null && (DateTime.Now - lastShot.TimeScored).TotalHours > 1.0) ||
                 (remarkList.HasNonCompletionRemark)) {
                eventScore.Status = ResultStatus.UNOFFICIAL;
                return;
            }

            //If shots have not been fired yet, then status if future
            if (numberOfShotsFired == 0)
            {
                eventScore.Status = ResultStatus.FUTURE;
                return;
            }

            var @event = topLevelEvent.FindEventComposite(eventName);
            if (@event != null)
            {
                var numberOfShots = @event.GetAllSingulars().Count();

                //if the number of shots fired is equal to expected number of shots
                if (numberOfShotsFired >= numberOfShots)
                {
                    eventScore.Status = ResultStatus.UNOFFICIAL;
                    return;
                }
                else if (numberOfShotsFired > 0)
                {
                    //if shots have been fired, but not yet complete
                    eventScore.Status = ResultStatus.INTERMEDIATE;
                    return;
                }
            }

            eventScore.Status = matchStatus;
            return;
        }
    }
}
