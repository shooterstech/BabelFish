using Scopos.BabelFish.APIClients;
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
        /// <summary>
        /// Generates a compiled <see cref="ResultCOF"/> object for the given <see cref="CourseOfFireEntryIndividual"/> entry.
        /// </summary>
        /// <param name="entry">The course of fire entry for which to generate the result.</param>
        /// <param name="generativeEvent">The event that triggered the generation of the result. For example "ShotDetected"</param>
        /// <returns>The generated <see cref="ResultCOF"/> object.</returns>
        public async Task<ResultCOF> GenerateResultCOFAsync( CourseOfFireEntryIndividual entry, string generativeEvent ) {

            var resultCOF = new ResultCOF();
            var resultCOFID = entry.ResultCofId;
            var matchParticipant = entry.MatchParticipant;

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
            resultCOF.OutOfCompetition = entry.OutOfCompetition;
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

        /// <summary>
        /// Overridden method to handle both individual and team CourseOfFireEntries. Generates a ResultEvent that may be used
        /// to populate a ResultList.
        /// <para>It is generally preferred to call <see cref="GenerateResultListAsync(ResultListAbbr, string)"/> directly.</para>
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ResultEvent> GenerateResultEntryAsync( CourseOfFireEntry entry ) {
            if (entry is CourseOfFireEntryIndividual) {
                return await GenerateResultEntryAsync( (CourseOfFireEntryIndividual)entry );
            } else if (entry is CourseOfFireEntryTeam) {
                return await GenerateResultEntryAsync( (CourseOfFireEntryTeam)entry );
            } else {
                throw new ArgumentException( $"Unknown type of CourseOfFireEntry: {entry.GetType()}" );
            }
        }

        /// <summary>
        /// Asynchronously generates a new ResultEvent entry based on the specified individual course of fire entry for use in a ResultList.
        /// <para>It is generally preferred to call <see cref="GenerateResultListAsync(ResultListAbbr, string)"/> directly.</para>
        /// </summary>
        /// <param name="entry">The individual course of fire entry containing participant and scoring information used to generate the
        /// result event. Cannot be null and must reference a valid match participant.</param>
        /// <returns>A ResultEvent object populated with data from the provided course of fire entry.</returns>
        /// <exception cref="BackwardsPointerException">Thrown if the provided entry does not reference a valid match participant.</exception>
        public async Task<ResultEvent> GenerateResultEntryAsync( CourseOfFireEntryIndividual entry ) {

            var resultEvent = new ResultEvent();
            var resultCOFID = entry.ResultCofId;
            var matchParticipant = entry.MatchParticipant;

            if (entry.MatchParticipant is null) {
                throw new BackwardsPointerException( $"CourseOfFireEntry with course of fire ID {entry.CourseOfFireId} does not have a reference to its MatchParticipant. Likely occured because the entry was created outside of a MatchProject." );
            }

            //Look up the Course of Fire Structure for this course of fire entry. If the course of fire structure, which should not happen can not be found, return false
            CourseOfFireStructure cofStructure;
            if (!MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( entry.CourseOfFireId, out cofStructure )) {
                _logger.Warn( $"Could not find course of fire structure for course of fire entry with course of fire ID {entry.CourseOfFireId} in match project {MatchProject.ProjectName} ({MatchProject.Match?.MatchID})" );
            }

            var participant = matchParticipant.Participant;
            var courseOfFireId = entry.CourseOfFireId;

            resultEvent.LastUpdated = DateTime.UtcNow;
            resultEvent.MatchID = MatchProject.Match.MatchID;
            resultEvent.LocalDate = MatchProject.ShotMapper.GetLastShot( resultCOFID, false )?.TimeScored ?? DateTime.Today;
            resultEvent.Participant = await participant.CopyAsync( cofStructure );
            //resultEvent.Participant.TeamName = entry.
            resultEvent.RemarkList = entry.RemarkList;
            resultEvent.ResultCOFID = resultCOFID;
            resultEvent.EventScores = await MatchProject.ShotMapper.GetEventScoresAsync( resultCOFID );
            resultEvent.LastShot = MatchProject.ShotMapper.GetLastShot( resultCOFID, true );

            // NOTE: Not setting Shots or SquaddingAssignment, as this is not part of a serialized ResultList.
            // NOTE: Do not need to project scores, as GenerateResultListAsync() will do so instead.

            return resultEvent;

        }

        /// <summary>
        /// Asynchronously generates a new ResultEvent entry based on the specified team course of fire entry for use in a ResultList.
        /// <para>It is generally preferred to call <see cref="GenerateResultListAsync(ResultListAbbr, string)"/> directly.</para>
        /// </summary>
        /// <param name="entry">The team course of fire entry containing participant and scoring information used to generate the
        /// result event. Cannot be null and must reference a valid match participant.</param>
        /// <returns>A ResultEvent object populated with data from the provided course of fire entry.</returns>
        /// <exception cref="BackwardsPointerException">Thrown if the provided entry does not reference a valid match participant.</exception>
        public async Task<ResultEvent> GenerateResultEntryAsync( CourseOfFireEntryTeam entry ) {

            var resultEvent = new ResultEvent();
            var matchParticipant = entry.MatchParticipant;

            //Look up the Course of Fire Structure for this course of fire entry. If the course of fire structure, which should not happen can not be found, return false
            CourseOfFireStructure cofStructure;
            if (!MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( entry.CourseOfFireId, out cofStructure )) {
                _logger.Warn( $"Could not find course of fire structure for course of fire entry with course of fire ID {entry.CourseOfFireId} in match project {MatchProject.ProjectName} ({MatchProject.Match?.MatchID})" );
            }

            var courseOfFireDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( courseOfFireDefinition );
            var resultStatusCalculator = new ResultStatusCalculator( cofStructure );

            var participant = (Team)matchParticipant.Participant;
            var courseOfFireId = entry.CourseOfFireId;

            resultEvent.LastUpdated = DateTime.UtcNow;
            resultEvent.MatchID = MatchProject.Match.MatchID;
            resultEvent.Participant = await participant.CopyAsync( cofStructure );
            resultEvent.RemarkList = entry.RemarkList;
            resultEvent.TeamMembers = new List<ResultEvent>();
            foreach (var tm in entry.TeamMembers) {
                CourseOfFireEntry teamMemberEntry;
                if (tm.MatchParticipant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out teamMemberEntry )) {
                    resultEvent.TeamMembers.Add( await this.GenerateResultEntryAsync( teamMemberEntry ) );
                }
            }

            // Sort the Team Members.
            var rankingRuleDefinition = await DefinitionCache.GetRankingRuleDefinitionAsync( topLevelEvent.RankingRuleMapping.GetRankingRuleDef( cofStructure.ScoreConfigName ) );
            var teamMemberList = resultEvent.TeamMembers.Cast<IEventScores>().ToList();
            ResultEngine.Sort( teamMemberList, rankingRuleDefinition, courseOfFireDefinition );
            resultEvent.TeamMembers = teamMemberList.Cast<ResultEvent>().ToList();
            resultEvent.EventScores = new Dictionary<string, EventScore>();

            // Calculate the score and status for the team events.
            foreach (var @event in topLevelEvent.GetEvents( true, true, true, true, true, false, true )) {
                resultEvent.EventScores[@event.EventName] = new EventScore();
                resultStatusCalculator.ClearEventScores();
                // Sums the scores of the contributing team members.
                for (int i = 0; i < Math.Min( cofStructure.NumberOfTeamMembers, resultEvent.TeamMembers.Count( item => !item.OutOfCompetition ) ); i++) {
                    var teamMemberResultEvent = resultEvent.TeamMembers[i];
                    if (teamMemberResultEvent.EventScores.TryGetValue( @event.EventName, out var teamMemberEventScore )) {
                        resultEvent.EventScores[@event.EventName].Score += teamMemberEventScore.Score;
                    }
                }


                for (int i = 0; i < resultEvent.TeamMembers.Count; i++) {
                    var teamMemberResultEvent = resultEvent.TeamMembers[i];
                    //All members of a team (even the crappy shooters) contribute to the team's Result Status.
                    resultStatusCalculator.AddEventScores( teamMemberResultEvent );

                    // Within a team, while team members are sorted according to their score, they do not have a value for Rank or RankOrder.
                    teamMemberResultEvent.Rank = 0;
                    teamMemberResultEvent.RankOrder = 0;
                }
                resultEvent.EventScores[@event.EventName].Status = resultStatusCalculator.Calculate( @event.EventName );
            }

            // resultEvent.Status = ??
            // resultEvent.LocalDate = ??

            // NOTE: Not setting Shots or SquaddingAssignment, as this is not part of a serialized ResultList.
            // NOTE: Do not need to project scores, as GenerateResultListAsync() will do so instead.

            return resultEvent;
        }

        /// <summary>
        /// Generates a compiled ResultList object based on the passed in ResultListAbbr. This is the main method that should be called to generate result list.
        /// </summary>
        /// <param name="resultListAbbr">The abbreviated result list containing the necessary configuration information to generate the full result list.</param>
        /// <param name="segmentGroupName">The name of the <see cref="SegmentGroup"/> that the <see cref="CourseOfFire"/> <see cref="RangeScript"/>
        /// is currently on. Value is not required.</param>
        /// <returns>A Task representing the asynchronous operation, with a ResultList object as the result.</returns>
        /// <exception cref="CourseOfFireStructureNotFoundException">Thrown if the CourseOfFireStructure associated with the provided ResultListAbbr cannot be found within the MatchProject.</exception>"
        public async Task<ResultList> GenerateResultListAsync( ResultListAbbr resultListAbbr, string segmentGroupName ) {

            CourseOfFireStructure cofStructure;
            if (!this.MatchProject.Match.MatchStructure.TryGetCourseOfFireStructure( resultListAbbr.CourseOfFireId, out cofStructure )) {
                var msg = $"Could not find course of fire structure for course of fire ID {resultListAbbr.CourseOfFireId} in match project {MatchProject.ProjectName} ({MatchProject.Match?.MatchID})";
                throw new CourseOfFireStructureNotFoundException( msg, _logger );
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
            metaData.ScoringTechnology = cofStructure.MetaData.ScoringTechnology;
            metaData.SegmentGroupName = string.IsNullOrEmpty( segmentGroupName ) ? string.Empty : segmentGroupName;
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
                if (mp.IsTeam == resultListAbbr.Team
                    && mp.Entries.Any( e => e.CourseOfFireId == resultListAbbr.CourseOfFireId )
                    && AttributeFilterCalculator.Passes( resultListAbbr.AttributeFilter, mp )) {
                    var entry = mp.Entries.First( e => e.CourseOfFireId == resultListAbbr.CourseOfFireId );
                    var resultEvent = await this.GenerateResultEntryAsync( entry );

                    resultList.Items.Add( resultEvent );
                }
            }

            //Need to set the ResultList status before we can sort, because the status can impact how the sorting is done, and we want to make sure that the sorting is done correctly.
            //For example, if the result list is INTERMEDIATE, then we want to make sure that the projected scores are used for sorting, and if it is OFFICIAL, then we want to make
            //sure that the actual scores are used for sorting.
            CalculateResultListStatus( resultList, cofStructure );

            var rankingRuleDefinition = await resultList.GetRankingRuleDefinitionAsync();
            var resultEngine = new ResultEngine( resultList, rankingRuleDefinition );
            resultEngine.DisableScoreProjection = cofStructure.DisableScoreProjection;
            resultEngine.CompareResultList = this.MatchProject.ResultListSlidingWindow.GetResultListToCompareAgainst( resultList );

            var projectorOfScores = ProjectorOfScoresFactory.Create( cofStructure.ProjectorOfScores, courseOfFireDefinition );
            projectorOfScores.NumberOfTeamMembers = (uint)cofStructure.NumberOfTeamMembers;
            await resultEngine.SortAsync( projectorOfScores, true );

            resultList.Projected = (resultList.Status == ResultStatus.INTERMEDIATE) && !resultEngine.DisableScoreProjection && (cofStructure.ProjectorOfScores != ProjectorOfScoresType.NULL);

            // After sorting, need to push the completed ResultList to the sliding window, so that it can be used for comparison when the next ResultList is generated.
            this.MatchProject.ResultListSlidingWindow.PushResultList( resultList );

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

            ResultStatusCalculator calculator = new ResultStatusCalculator( cofStructure );

            foreach (var re in resultList.Items) {
                calculator.AddEventScores( re );
            }

            var status = calculator.Calculate( resultList.EventName );
            resultList.Metadata[cofStructure.MatchStructure.Match.MatchID].Status = status;

        }
        #endregion
    }
}
