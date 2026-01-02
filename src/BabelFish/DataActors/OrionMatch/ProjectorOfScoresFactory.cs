using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    
    public enum ProjectorOfScoresType {

        /// <summary>
        /// The NULL value Projector of Scores does not project scores at all.
        /// </summary>
        [Description( "Null" )]
        [EnumMember( Value = "Null" )]
        NULL,

        /// <summary>
        /// Project scores using an athletes average shot fired.
        /// </summary>
        [Description( "Average Shot Fired" )]
        [EnumMember( Value = "Average Shot Fired" )]
        AVERAGE_SHOT_FIRED,

        /// <summary>
        /// Project scores using an athletes score history and average shot fired.
        /// </summary>
        [Description( "Score History" )]
        [EnumMember( Value = "Score History" )]
        SCORE_HISTORY
    }

    public static class ProjectorOfScoresFactory {

        public static ProjectorOfScores Create( ProjectorOfScoresType type, CourseOfFire courseOfFire ) {

            switch (type) {
                case ProjectorOfScoresType.NULL:
                    return new ProjectScoresByNull( courseOfFire );

                default:
                case ProjectorOfScoresType.AVERAGE_SHOT_FIRED:
                    return new ProjectScoresByAverageShotFired( courseOfFire );

                case ProjectorOfScoresType.SCORE_HISTORY:
                    return new ProjectScoresByScoreHistory( courseOfFire );
            }

        }

        public static ProjectorOfScores Create( ProjectorOfScoresType type, CourseOfFire courseOfFire, CompareByRankingDirective teamMemberComparer ) {

            switch (type) {
                case ProjectorOfScoresType.NULL:
                    return new ProjectScoresByNull( courseOfFire );

                default:
                case ProjectorOfScoresType.AVERAGE_SHOT_FIRED:
                    return new ProjectScoresByAverageShotFired( courseOfFire, teamMemberComparer );

                case ProjectorOfScoresType.SCORE_HISTORY:
                    return new ProjectScoresByScoreHistory( courseOfFire, teamMemberComparer );
            }

        }

    }
}
