using System.Diagnostics;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ResultDocumentGenerator {

        #region Private Variables
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region Construcgtors, Initialization, and Factory Methods
        public ResultDocumentGenerator( MatchProject matchProject ) {
            MatchProject = matchProject;
        }
        #endregion

        #region Helper Properties
        public MatchProject MatchProject { get; private set; }
        #endregion

        #region Methods
        public async Task<ResultCOF> GenerateResultCOFAsync( string resultCOFID, string generativeEvent ) {

            var resultCOF = new ResultCOF();
            MatchParticipant matchParticipant;

            if (!MatchProject.TryGetMatchParticipantByResultCOFID( resultCOFID, out matchParticipant )) {
                return null;
            }

            //Look up the participant for this result COF ID. If the result COF ID is not known, return false
            CourseOfFireEntryIndividual entry;
            if (!MatchProject.TryGetCourseOfFireEntryByResultCOFID( resultCOFID, out entry )) {
                _logger.Warn( $"Could not find Course Of Fire Entry for ResultCOFID {resultCOFID} in match project {MatchProject.ProjectName} ({MatchProject.Match?.MatchID})" );
                return null;
            }

            //Look up the Course of Fire Structure for this course of fire entry. If the course of fire structure, which should not happen can not be found, return false
            CourseOfFireStructure cofStructure;
            if (!MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( entry.CourseOfFireId, out cofStructure )) {
                _logger.Warn( $"Could not find course of fire structure for course of fire entry with course of fire ID {entry.CourseOfFireId} in match project {MatchProject.ProjectName} ({MatchProject.Match?.MatchID})" );
            }

            var participant = matchParticipant.Participant;
            var courseOfFireId = entry.CourseOfFireId;
            var courseOfFireDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( courseOfFireDefinition );

            resultCOF.CourseOfFireDef = cofStructure.CourseOfFireDef;
            resultCOF.Creator = MatchProject.Creator;
            resultCOF.Delta = false; //We will include all EventScores and Shots, thus this is not a delta.
            resultCOF.FiringPointNumber = entry.SquaddingAssignment.ToString();
            resultCOF.GenerativeEvent = generativeEvent;
            resultCOF.JSONVersion = Helpers.Common.DATA_MODEL_VERSION;
            resultCOF.LastUpdated = DateTime.UtcNow;
            resultCOF.MatchID = MatchProject.Match.MatchID;
            resultCOF.LocalDate = MatchProject.ShotMapper.GetLastShot( resultCOFID, false )?.TimeScored ?? DateTime.Today;
            resultCOF.MatchLocation = MatchProject.Match.Location.ToString();
            resultCOF.MatchName = MatchProject.Match.Name;
            resultCOF.MatchType = MatchProject.Match.MatchType;
            resultCOF.OwnerId = MatchProject.Match.Visibility == VisibilityOption.PUBLIC ? MatchProject.Match.OwnerId : matchParticipant.UserID;
            resultCOF.Participant = await participant.CopyAsync( cofStructure );
            resultCOF.RemarkList = entry.RemarkList;
            resultCOF.ResultCOFID = resultCOFID;
            resultCOF.ScoreConfigName = cofStructure.ScoreConfigName;
            resultCOF.SquaddingAssignment = entry.SquaddingAssignment;
            // resultCOF.Status is set below, after .EventScores
            resultCOF.TargetCollectionName = cofStructure.TargetCollectionName;

            resultCOF.EventScores = await MatchProject.ShotMapper.GetEventScoresAsync( resultCOFID );
            resultCOF.Shots = await MatchProject.ShotMapper.GetShotsBySequenceAsync( resultCOFID, false );
            resultCOF.LastShot = MatchProject.ShotMapper.GetLastShot( resultCOFID, true );
            resultCOF.Status = resultCOF.EventScores[topLevelEvent.EventName].Status;

            //Need to figure out how to populate these later. For now, we will just set them to null.
            resultCOF.LiveDisplay = null;
            resultCOF.PostDisplay = null;

            //Will this work for teams ? 
            var projector = ProjectorOfScoresFactory.Create( cofStructure.ProjectorOfScores, courseOfFireDefinition );
            resultCOF.ProjectScores( projector );

            return resultCOF;

        }

        public async Task<ResultList> GenerateResultListAsync( ResultListAbbr resultListAbbr, string segmentGroupName ) {

            CourseOfFireStructure cofStructure;
            if (!this.MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( resultListAbbr.CourseOfFireId, out cofStructure )) {
                _logger.Warn( $"Could not find course of fire structure for course of fire ID {resultListAbbr.CourseOfFireId} in match project {MatchProject.ProjectName} ({MatchProject.Match?.MatchID})" );
                return null;
            }

            var courseOfFireDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var resultList = new ResultList();
            var metaData = new ResultListMetadata();

            metaData.Creator = MatchProject.Creator;
            metaData.EndDate = cofStructure.EndDate;
            metaData.LastUpdated = DateTime.UtcNow;
            metaData.MatchID = MatchProject.Match.MatchID;
            metaData.MatchLocation = MatchProject.Match.Location.ToString();
            metaData.OwnerId = MatchProject.Match.OwnerId;
            metaData.ProjectionMadeBy = cofStructure.ProjectorOfScores.ToString();
            metaData.ScoringSystems = new List<string>(); //TODO: Figure out how to populate this list
            metaData.ScoringSystemType = ScoringSystem.UNKNOWN; //TODO: Figure out how to populate this
            metaData.SegmentGroupName = segmentGroupName;
            metaData.StartDate = cofStructure.StartDate;

            resultList.Metadata[MatchProject.Match.MatchID] = metaData;

            resultList.CourseOfFireId = resultListAbbr.CourseOfFireId;
            resultList.CourseOfFireDef = cofStructure.CourseOfFireDef;
            resultList.EventName = resultListAbbr.EventName;
            resultList.MatchName = MatchProject.Match.Name;
            resultList.Primary = resultListAbbr.Primary;
            resultList.RankingRuleDef = resultListAbbr.RankingRuleDef;
            resultList.ResultListFormatDef = resultListAbbr.ResultListFormatDef;
            resultList.ScoreConfigName = resultListAbbr.ScoreConfigName;
            resultList.ResultName = resultListAbbr.ResultName;
            resultList.Team = resultListAbbr.Team;
            resultList.UserDefinedText = resultListAbbr.UserDefinedText;

            foreach (var mp in this.MatchProject.Participants) {
                if (mp.Entries.Any( e => e.CourseOfFireId == resultListAbbr.CourseOfFireId )
                    && AttributeFilterCalculator.Passes( resultListAbbr.AttributeFilter, mp )) {
                    var entry = (CourseOfFireEntryIndividual)mp.Entries.First( e => e.CourseOfFireId == resultListAbbr.CourseOfFireId );
                    var resultEvent = new ResultEvent();
                    resultEvent.Participant = await mp.Participant.CopyAsync( cofStructure );
                    resultEvent.RemarkList = entry.RemarkList;
                    resultEvent.ResultCOFID = entry.ResultCofId;
                    resultEvent.EventScores = await MatchProject.ShotMapper.GetEventScoresAsync( entry.ResultCofId );
                    resultEvent.LastShot = MatchProject.ShotMapper.GetLastShot( entry.ResultCofId, true );


                    resultList.Items.Add( resultEvent );

                }
            }

            //Need to set the ResultList status before we can sort, because the status can impact how the sorting is done, and we want to make sure that the sorting is done correctly.
            //For example, if the result list is INTERMEDIATE, then we want to make sure that the projected scores are used for sorting, and if it is OFFICIAL, then we want to make
            //sure that the actual scores are used for sorting.
            CalculateResultListStatus( resultList, cofStructure );

            /*  The code from Orion  
                var resultEngine = new ResultEngine( document, rankingRule );
                var teamMemberComparer = new CompareByRankingDirective( CourseOfFireDefinition, RankingDirective.GetDefault( document.EventName, document.ScoreConfigName ) );
                resultEngine.DisableScoreProjection = _disableScoreProjection;
                resultEngine.CompareResultList = ResultListSlidingWindow.GetResultList( document.ResultName );
                teamMemberComparer.ResultStatus = document.Status;
                teamMemberComparer.Projected = document.Status == ResultStatus.INTERMEDIATE ? true : false;

                var projectorOfScores = ProjectorOfScoresFactory.Create( match.ProjectorOfScores, CourseOfFireDefinition, teamMemberComparer );
                projectorOfScores.NumberOfTeamMembers = (uint)match.NumberTeamMembers;
                document.Metadata[match.MatchID.ToString()].ProjectionMadeBy = projectorOfScores.ProjectionMadeBy;

                await resultEngine.SortAsync( projectorOfScores, true );
            */

            var rankingRuleDefinition = await resultList.GetRankingRuleDefinitionAsync();
            var resultEngine = new ResultEngine( resultList, rankingRuleDefinition );
            resultEngine.DisableScoreProjection = cofStructure.DisableScoreProjection;
            resultEngine.CompareResultList = this.MatchProject.ResultListSlidingWindow.GetResultListToCompareAgainst( resultList );

            var projectorOfScores = ProjectorOfScoresFactory.Create( cofStructure.ProjectorOfScores, courseOfFireDefinition );
            await resultEngine.SortAsync( projectorOfScores, true );

            resultList.Projected = (resultList.Status == ResultStatus.INTERMEDIATE) && !resultEngine.DisableScoreProjection && (cofStructure.ProjectorOfScores != ProjectorOfScoresType.NULL);

            return resultList;
        }

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
        public static void CalculateResultListStatus( ResultList resultList, CourseOfFireStructure cofStructure ) {

            if (cofStructure.Official) {
                resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.OFFICIAL;
                return;
            }

            if (resultList.Items.Count == 0) {
                resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.FUTURE;
                return;
            }

            if (string.IsNullOrEmpty( resultList.EventName )) {
                Debug.Fail( "ResultList does not have an EventName. This should not happen, but if it does, we will just set the result list status to OFFICIAL." );
                resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.OFFICIAL;
                return;
            }

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

            if (allAreFuture) {
                resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.FUTURE;
                return;
            }

            if (oneIsIntermediate) {
                resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.INTERMEDIATE;
                return;
            }

            if (oneIsUnofficial) {
                resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.UNOFFICIAL;
                return;
            }

            resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = ResultStatus.OFFICIAL;
            return;

        }
        #endregion
    }
}
