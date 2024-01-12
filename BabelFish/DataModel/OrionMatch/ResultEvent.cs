﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultEvent {

        public ResultEvent() {
            Score = new Scopos.BabelFish.DataModel.Athena.Score();
            Children = new List<ResultEventChild>();
            //Purposefully set TeamMemebers to null so if it is an individual the attribute doesn't get added into the JSON
            TeamMembers = null;
        }

        public string DisplayName { get; set; }

        [Obsolete("Field is being replaced with the ScoreFormatCollectionDef and ScoreConfigName values. ScoreFormatCollectionDef is found using the CoruseOfFireDef")]
        public string ScoreFormat { get; set; }
        
        public string ResultCOFID { get; set; }

        /// <summary>
        /// Score. If the Preliminary result list is requested, Score will be the predicted score based on the athlete's score history and shots taken in the current match.
        /// </summary>
        [Obsolete( "Replaced with EventScores" )]
        public Scopos.BabelFish.DataModel.Athena.Score Score { get; set; }

        public int Rank { get; set; }

        /// <summary>
        /// Contains the participants scores for the child events directly under this event. This is not a complete tree, for a complete
        /// tree look up the ResultCOF using the ResultCOFID.
        /// </summary>
        [Obsolete( "Replaced with EventScores" )]
        public List<ResultEventChild> Children { get; set; } = new List<ResultEventChild>();

        /// <summary>
        /// The Orion User ID of the athlete. Is blank (empty string) if it is not known or the participant is not a person (and thus likely is a team),
        /// </summary>
        public string UserID { get; set; } = "";

        [Obsolete( "Replaced with EventScores" )]
        public string EventName { get; set; }

        /// <summary>
        /// If this is a team score, the TeamMembers will be the scores of the team members.If this is an Individual WARNING value will be null.
        /// </summary>
        public List<ResultEvent> TeamMembers { get; set; } = new List<ResultEvent>();

        public Dictionary<string, Scopos.BabelFish.DataModel.OrionMatch.EventScore> EventScores { get; set; }

    }

    [Serializable]
    public class ResultEventChild {

        public ResultEventChild() {
            Score = new Scopos.BabelFish.DataModel.Athena.Score();
        }

        [Obsolete( "Field is being replaced with the ScoreFormatCollectionDef and ScoreConfigName values. ScoreFormatCollectionDef is found using the CoruseOfFireDef" )]
        public string ScoreFormat { get; set; }

        public Scopos.BabelFish.DataModel.Athena.Score Score { get; set; }

        public string EventName { get; set; }

    }
}
