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
            resultCOF.Shots = await MatchProject.ShotMapper.GetShotsBySequenceAsync( resultCOFID );
            resultCOF.LastShot = MatchProject.ShotMapper.GetLastShot( resultCOFID, true );
            resultCOF.Status = resultCOF.EventScores[topLevelEvent.EventName].Status;

            //Need to figure out how to populate these later. For now, we will just set them to null.
            resultCOF.LiveDisplay = null;
            resultCOF.LiveTopic = null;
            resultCOF.PostDisplay = null;

            //Will this work for teams ? 
            var projector = ProjectorOfScoresFactory.Create( cofStructure.ProjectorOfScores, courseOfFireDefinition );
            resultCOF.ProjectScores( projector );

            return resultCOF;

        }
        #endregion
    }
}
