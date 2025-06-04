using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime;
using NLog;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// ResultEngine sorts a ResultList
    /// </summary>
    public class ResultEngine {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Uses the default RankingRule
        /// </summary>
        /// <param name="resultList"></param>
        public ResultEngine(ResultList resultList) {
            this.ResultList = resultList;
            this.RankingRule = null; //RankingRule to be learned during the .LearnDefaultRankingRule() method.
        }

        /// <summary>
        /// Overrides the default RankingRule for the ResultList.
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="rankingRule"></param>
        public ResultEngine( ResultList resultList, RankingRule rankingRule ) {
            this.ResultList = resultList;
            this.RankingRule = rankingRule;
        }

        /// <summary>
        /// Helper function, to be called after construction and before sorting, to learn the 
        /// default RankingRule to use. If a RankingRule was passed in during the Constructor
        /// then this function simply returns.
        /// </summary>
        /// <exception cref="ScoposException">
        /// Thrown if the function could not learn the Course of Fire or Ranking Rule definitions. Either because
        /// the values could not be parsed, don't exists as definitions, or networking errors.
        /// </exception>
        private async Task LearnDefaultRankingRuleAsync() {

            SetName cofSetName;
            SetName rankingRuleSetName;

            //Load the Course of Fire definition
            if (SetName.TryParse( this.ResultList.CourseOfFireDef, out cofSetName )) {
                this.CourseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync( cofSetName );
            } else {
                string msg = $"Could not parse the Course of Fire SetName '{this.ResultList.CourseOfFireDef}'.";
                throw new ScoposException( msg, logger );
            }

            //If the user passed in the Ranking Rule, we don't need to look it up
            if (this.RankingRule != null)
                return;

            //First try and use the RankingRule that's listed in the ResultList
            if (!string.IsNullOrEmpty( this.ResultList.RankingRuleDef ) && SetName.TryParse( this.ResultList.RankingRuleDef, out rankingRuleSetName )) {
                this.RankingRule = await DefinitionCache.GetRankingRuleDefinitionAsync( rankingRuleSetName );
                logger.Info( $"Ranking Rule '{rankingRuleSetName}' will be used to sort Result List '{this.ResultList.Name}', learned by reading the .RankingRuleDef value in the Result List." );
                return;
            }

            //If that didn't work, try and use the RankingRule that's listed in the Coruse of fire's Event
            foreach (var @event in this.CourseOfFire.Events) {
                if (@event.EventName == this.ResultList.EventName) {
                    if (!string.IsNullOrEmpty( @event.RankingRuleDef ) && SetName.TryParse( @event.RankingRuleDef, out rankingRuleSetName )) {
                        this.RankingRule = await DefinitionCache.GetRankingRuleDefinitionAsync( rankingRuleSetName );
                        logger.Info( $"Ranking Rule '{rankingRuleSetName}' will be used to sort Result List '{this.ResultList.Name}', learned by reading the .RankingRuleDef value in the Event '{@event.EventName}'." );

                        return;
                    }
                }
            }

            //And if that didn't work, the user is pretty much screwed.
            //Guess we'll try and generate a Ranking Rule defintion based on the Event Name ... which is pretty clever is I do say so myself.
            this.RankingRule = RankingRule.GetDefault( this.ResultList.EventName, this.ResultList.ScoreConfigName );
            logger.Warn( $"A default / generic Ranking Rule will be used to sort Result List '{this.ResultList.Name}', dynamically generated based on EventName '{this.ResultList.EventName}' and Score Config Name '{this.ResultList.ScoreConfigName}'." );

        }

        public ResultList ResultList { get; private set; }

        public CourseOfFire CourseOfFire { get; private set; }

        public RankingRule RankingRule { get; private set; }

        public ResultList CompareResultList { get; set; } = null;

        /// <summary>
        /// Enables or disables projecting scores and ranking participants by their projection. 
        /// This value is practically set by the COURSE OF FIRE Range Scrip's Command.ResultEngine.
        /// A value of true (to disable) only has an effect if score projection would otherwise
        /// be used (e.g. result list status is INTERMEDIATE). 
        /// </summary>
        public bool DisableScoreProjection { get; set; } = false;

        /// <summary>
        /// Sorts the ResultLists's Items array using each participant's absolute score and the specified
        /// RankingRule definition.
        /// 
        /// If the status of the ResultList is INTERMEDIATE then it also
        /// calculates Projected scores usiing the passed in ProjectorOfScores. It then calculates the projected 
        /// rank based on the Projected Scores.
        /// 
        /// In most cases, the ResultList.Items array remains sorted by absolute score. However, if the Status
        /// is INTERMEDIATE and the parameter listAccordingToProjectedScores is true, then .Items is sorted
        /// using the projected scores.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ScoposException">
        /// Thrown if the function could not learn the Course of Fire or Ranking Rule definitions. Either because
        /// the values could not be parsed, don't exists as definitions, or networking errors.
        /// </exception>
        public async Task SortAsync( ProjectorOfScores ps, bool listAccordingToProjectedScores ) {

            await LearnDefaultRankingRuleAsync();

            //Perform a ranking using absolute scores
            foreach (var directive in this.RankingRule.RankingRules) {

                List<ResultEvent> listToSort;
                var appliesTo = directive.GetAppliesToStartAndCount( this.ResultList.Items.Count );
                int start = appliesTo.Item1;
                int count = appliesTo.Item2;
                int rank = 0;
                int rankOrder = 0;
                ResultEvent lastResultEventSeen = null;

                if (count > 0) {
                    //Slice out the portion of the .Items list we are to sort
                    listToSort = this.ResultList.Items.GetRange( start, count );

                    var comparer = new CompareByRankingDirective( this.CourseOfFire, directive );
                    comparer.ResultStatus = this.ResultList.Status;

                    //Sort the list, which uses both the Ranking Directives .Rules and .ListOnly
                    listToSort.Sort( comparer );

                    //Assign the rank and rank order
                    foreach (var resultEvent in listToSort) {
                        if (lastResultEventSeen == null) {
                            //First item in the list
                            rank = start + 1;
                            rankOrder = rank;
                            resultEvent.Rank = rank;
                            resultEvent.RankOrder = rankOrder;
                        } else if (!comparer.Equals( resultEvent, lastResultEventSeen )) {
                            //increment the rank order, and set it to equal the rank
                            rankOrder++;
                            rank = rankOrder;
                            resultEvent.Rank = rank;
                            resultEvent.RankOrder = rankOrder;
                        } else {
                            // increment the rank order, but keep the same value for rank
                            rankOrder++;
                            resultEvent.Rank = rank;
                            resultEvent.RankOrder = rankOrder;
                        }

                        lastResultEventSeen = resultEvent;
                    }

                    //Apply the newly sorted Result Events back to the .Items array
                    for (int index = start; index < start + count; index++) {
                        this.ResultList.Items[index] = listToSort[index - start];
                    }
                }
            }

            //Project scores and perform a ranking by Projected Score
            //if the result list's status is INTERMEDIATE (and not FUTURE, UNOFFICIAL, or OFFICIAL), and the user has not turned it off.
            if (this.ResultList.Status == ResultStatus.INTERMEDIATE && ! DisableScoreProjection) {


                //Project (predict) the scores of athlets at the end of the match.

                //There should be exactly 1 .metadata in the Result list. But will guard against errors.
                if (this.ResultList.Metadata.Count > 0)
                    this.ResultList.Metadata.Values.First().ProjectionMadeBy = ps.ProjectionMadeBy;

                await ps.InitializeAsync( this.ResultList.Items.ToList<IEventScoreProjection>() );
                foreach (var item in this.ResultList.Items) {
                    //Do not project scores for anyone who has a DNS, DNF, DSQ, or ELIMINATED
                    if (!item.Participant.RemarkList.HasNonCompletionRemark)
                        item.ProjectScores( ps );
                }

                //When we are sorting by projected scores, we only will ever use the first Ranking Directive (which gets applied to all Result Events)
                RankingDirective directive;
                if (this.RankingRule.RankingRules.Count > 0)
                    directive = this.RankingRule.RankingRules[0];
                else
                    directive = RankingDirective.GetDefault( this.ResultList.EventName, this.ResultList.ScoreConfigName );

                //Make a shallow copy of the list
                var appliesTo = directive.GetAppliesToStartAndCount( this.ResultList.Items.Count );
                int start = appliesTo.Item1;
                int count = appliesTo.Item2;
                int rank = 0;
                int rankOrder = 0;
                ResultEvent lastResultEventSeen = null;
                List<ResultEvent> listToSort = this.ResultList.Items.GetRange( rank, count );

                var comparer = new CompareByRankingDirective( this.CourseOfFire, directive );
                comparer.ResultStatus = this.ResultList.Status;
                comparer.Projected = true;

                //Sort the list, which uses both the Ranking Directives .Rules and .ListOnly
                listToSort.Sort( comparer );

                //Assign the rank and rank order
                foreach (var resultEvent in listToSort) {
                    if (lastResultEventSeen == null) {
                        //First item in the list
                        rank = start + 1;
                        rankOrder = rank;
                        resultEvent.ProjectedRank = rank;
                        resultEvent.ProjectedRankOrder = rankOrder;
                    } else if (!comparer.Equals( resultEvent, lastResultEventSeen )) {
                        //increment the rank order, and set it to equal the rank
                        rankOrder++;
                        rank = rankOrder;
                        resultEvent.ProjectedRank = rank;
                        resultEvent.ProjectedRankOrder = rankOrder;
                    } else {
                        // increment the rank order, but keep the same value for rank
                        rankOrder++;
                        resultEvent.ProjectedRank = rank;
                        resultEvent.ProjectedRankOrder = rankOrder;
                    }

                    lastResultEventSeen = resultEvent;
                }

                //Only list the projected scores in order, if the caller asked us to
                if (listAccordingToProjectedScores) {
                    this.ResultList.Items = listToSort;
                    this.ResultList.Projected = true;
                }

            } else {
                //If the results are UNOFFICIAL or OFFICIAL, use the ProjectScoreByNull to clear our any residue projected scores.
                var nullProjectorOfScores = new ProjectScoresByNull( this.CourseOfFire );
                if (this.ResultList.Metadata.Count > 0)
                    this.ResultList.Metadata.Values.First().ProjectionMadeBy = nullProjectorOfScores.ProjectionMadeBy;

                this.ResultList.Projected = false;
                foreach (var item in this.ResultList.Items) {
                    item.ProjectScores( nullProjectorOfScores );
                }

                foreach (var resultEvent in this.ResultList.Items) {
                    resultEvent.ProjectedRank = 0;
                    resultEvent.ProjectedRankOrder = 0;
                }
            }

            //Calculate the RankDelta and ProjectedRankDelta
            ResultEvent compare;
            if (this.CompareResultList != null 
                && this.CompareResultList.ResultName == this.ResultList.ResultName
                && this.ResultList.Status == ResultStatus.INTERMEDIATE ) {
                foreach (var re in this.ResultList.Items) {
                    if (this.CompareResultList.TryGetByResultCOFID( re.ResultCOFID, out compare )) {
                        if (this.ResultList.Projected) {
                            re.ProjectedRankDelta = compare.ProjectedRank - re.ProjectedRank;
                        }
                        re.RankDelta = compare.Rank - re.Rank;
                    }
                }
                this.ResultList.Metadata.First().Value.CompareResultListLastUpdated = this.CompareResultList.LastUpdated;
            }
        }

    }
}
