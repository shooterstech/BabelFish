using System;
using System.Collections.Generic;
using System.Text;
using NLog.Filters;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Abstract base class, for classes that try to predict the final scores a participant will 
    /// finish with, based on the scores they already shot.
    /// 
    /// Each concrete class that implements ProjectorOfScores will have its own algorithm for
    /// making the predicted scores.
    /// </summary>
    public abstract class ProjectorOfScores {

        /// <summary>
        /// Uses a default TeamMemberComparer based on the top level event in the course of fire and Decimal ScoreConfigName.
        /// </summary>
        /// <param name="courseOfFire"></param>
        public ProjectorOfScores( CourseOfFire courseOfFire ) {
            this.CourseOfFire = courseOfFire;

            //The top level event should (better be) the only Event Type == EVENT.
            this.TopLevelEvent = EventComposite.GrowEventTree( this.CourseOfFire );

            this.TeamMemberComparer = new CompareByRankingDirective( this.CourseOfFire, RankingDirective.GetDefault( this.TopLevelEvent.EventName, this.CourseOfFire.ScoreConfigDefault ) );
        }

        public ProjectorOfScores( CourseOfFire courseOfFire, CompareByRankingDirective teamMemberComparer ) {
            this.CourseOfFire = courseOfFire;

            //The top level event should (better be) the only Event Type == EVENT.
            this.TopLevelEvent = EventComposite.GrowEventTree( this.CourseOfFire );

            this.TeamMemberComparer = teamMemberComparer;
        }

        public CourseOfFire CourseOfFire { get; private set; }

        public EventComposite TopLevelEvent { get; private set; }

        /// <summary>
        /// The x-api-key to use when making scopos REST API calls.
        /// </summary>
        /// <see cref="https://support.scopos.tech/index.html?rest-api.html"/>
        public string XAPIKey { get; set; } = "";

        public APIStage APIStage { get; set; } = APIStage.PRODUCTION;

        /// <summary>
        /// The comparer instance to use when ranking team members within a team, which will decide which team members's 
        /// projected scores to count when calculating the team's projected score.
        /// </summary>
        public CompareByRankingDirective TeamMemberComparer { get; set; }

        /// <summary>
        /// If projecting scores for a team event, this is the number of team members the contribute to the team score.
        /// The default value is 4.
        /// </summary>
        public uint NumberOfTeamMembers { get; set; } = 4;

        /// <summary>
        /// Calculates projected scores for the passed in IEventScoreProjection. Stores
        /// the projected scores in the EventScore's .Projected Score instance.
        /// </summary>
        /// <param name="projection"></param>
        public abstract void ProjectAthleteScores( IEventScoreProjection projection );

        /// <summary>
        /// Default algorithm to project scores of both athletes and teams. Made virtual so a concrete
        /// ProjectorOfScores could implement their own team calculations.
        /// </summary>
        /// <param name="projection"></param>
        public virtual void ProjectEventScores( IEventScoreProjection projection ) {

            if (projection.Participant is Team) {
                //Project the scores of each team member. Note that this is a recursive call.
                var teamMembers = projection.GetTeamMembersAsIEventScoreProjection();
                foreach (var tm in teamMembers ) {
                    tm.ProjectScores( this );
                }

                //Now sort the team members
                teamMembers.Sort( TeamMemberComparer );

                //Set the list of team members back to the ResultEvent (which is an IEventScoreProjection)
                projection.SetTeamMembersFromIEventScoreProjection( teamMembers );

                //With the list of team members sorted, lets project the scores of each event for the team, between the EVENT and STAGES
                ProjectTeamScores( projection, TopLevelEvent, 0 );

            } else {
                //.Participant is an Individual
                ProjectAthleteScores( projection );
            }
        }

        /// <summary>
        /// Method to initialize internal parameters for the ProjectorOfScores. Default implementation is not to do anything.
        /// </summary>
        /// <param name="listOfParticipants"></param>
        public virtual async Task InitializeAsync( List<IEventScoreProjection> listOfParticipants ) { }

        public virtual void ProjectTeamScores( IEventScoreProjection teamToProject, EventComposite eventToProject, int recusionDepth ) {

            //Calculate the .Projected score for the EventComposite we got called for.
            //NOTE that the team's .Projected score is simply the sum of team member's .ProjectedScore.
            //If someone want's a different implementation, they should write their own concrete ProjectorOfScore and override this method :P
            EventScore teamEventScore, teamMemberEventScore;
            int teamMemberCount = 0;
            if ( teamToProject.EventScores.TryGetValue( eventToProject.EventName, out teamEventScore ) ) {
                teamEventScore.Projected = new DataModel.Athena.Score();

                foreach( var tm in teamToProject.GetTeamMembersAsIEventScoreProjection() ) {

                    if (teamMemberCount < NumberOfTeamMembers && tm.EventScores.TryGetValue( eventToProject.EventName, out teamMemberEventScore) ) {
                        teamEventScore.Projected.I += teamMemberEventScore.Projected.I;
                        teamEventScore.Projected.X += teamMemberEventScore.Projected.X;
                        teamEventScore.Projected.D += teamMemberEventScore.Projected.D;
                        teamEventScore.Projected.S += teamMemberEventScore.Projected.S;
                    }

                    teamMemberCount++;
                }
            }

            //Now repeat this process, recursively, until we reach a STAGE or recusionDepth > 2
            if (eventToProject.EventType == EventtType.STAGE || recusionDepth > 2)
                return;

            foreach( var childEvent in eventToProject.Children ) {
                ProjectTeamScores( teamToProject, childEvent, recusionDepth + 1 );
            }
        }

        /// <summary>
        /// When generating a Projected Result List, the Result List needs to identify who/what
        /// made the projection. This string prepresents the concrete class that made the projection
        /// and shold populate that Result List value.
        /// </summary>
        public abstract string ProjectionMadeBy { get; }
    }
}
