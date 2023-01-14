using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultEvent {

        public ResultEvent() {
            Score = new Score();
            Children = new List<ResultEventChild>();
            //Purposefully set TeamMemebers to null so if it is an individual the attribute doesn't get added into the JSON
            TeamMembers = null;
        }

        public string DisplayName { get; set; }

        [Obsolete("Field is being replaced with the ScoreFormatCollectionDef and ScoreConfigName values. ScoreFormatCollectionDef is found using the CoruseOfFireDef")]
        public string ScoreFormat { get; set; }
        
        public string ResultCOFID { get; set; }

        public Score Score { get; set; }

        public int Rank { get; set; }

        /// <summary>
        /// Contains the participants scores for the child events directly under this event. This is not a complete tree, for a complete
        /// tree look up the ResultCOF using the ResultCOFID.
        /// </summary>
        public List<ResultEventChild> Children { get; set; } = new List<ResultEventChild>();

        /// <summary>
        /// The Orion User ID of the athlete. Is blank (empty string) if it is not known or the participant is not a person (and thus likely is a team),
        /// </summary>
        public string UserID { get; set; } = "";

        public List<ResultEventTeamMember> TeamMembers { get; set; } = new List<ResultEventTeamMember>();

    }

    [Serializable]
    public class ResultEventTeamMember {

        public ResultEventTeamMember() {
            Score = new Score();
            Children = new List<ResultEventChild>();
        }

        public string ScoreFormat { get; set; }



        public string DisplayName { get; set; }

        public string UserID { get; set; }

        public Score Score { get; set; }

        public List<ResultEventChild> Children { get; set; }

        public string ResultCOFID { get; set; }
    }

    [Serializable]
    public class ResultEventChild {

        public ResultEventChild() {
            Score = new Score();
        }

        public string ScoreFormat { get; set; }

        public Score Score { get; set; }

        public string EventName { get; set; }

    }
}
