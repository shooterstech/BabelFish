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
            foreach (var re in resultList.Items) {
                if (re.EventScores.TryGetValue( resultList.EventName, out eventScore )) {
                    allAreFuture &= (eventScore.Status == ResultStatus.FUTURE);
                    oneIsIntermediate |= (eventScore.Status == ResultStatus.INTERMEDIATE);
                    oneIsUnofficial |= (eventScore.Status == ResultStatus.UNOFFICIAL);
                }
            }

            if (allAreFuture) { return ResultStatus.FUTURE; }

            if (oneIsIntermediate) { return ResultStatus.INTERMEDIATE; }

            if (oneIsUnofficial) { return ResultStatus.UNOFFICIAL; }

            return resultList.Status;
        }


        /// <summary>
        /// Checks for the unusual condition that the returned result list is ordered by projected rank, 
        /// but the status is UNOFFICIAL or OFFICIAL. This could happen if a user turns Orion off, 
        /// before having all scores turned in, and we're past the end date of the match.
        /// </summary>
        public static void ReSortIfOfficial( this ResultList resultList ) {

            if (resultList is not null
                && resultList.Projected &&
                (resultList.Status == ResultStatus.UNOFFICIAL || resultList.Status == ResultStatus.OFFICIAL)) {
                var sorter = new CompareResultByRank( CompareResultByRank.CompareMethod.RANK_ORDER, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
                resultList.Items.Sort( sorter );

                resultList.Projected = false;
            }
        }
    }
}
