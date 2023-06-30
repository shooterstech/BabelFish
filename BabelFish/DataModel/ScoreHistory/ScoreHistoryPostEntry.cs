using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.DataModel.ScoreHistory {
    public class PostStageStyleScore {
        public PostStageStyleScore(){
        }
        public PostStageStyleScore(string stageStyleDef, Score score){
            StageStyleDef = stageStyleDef;
            this.Score = score;
        }

        public string StageStyleDef { get; set; } = "";
        public Score Score {get; set;}

    }

    public class ScoreHistoryPostEntry { //used for both post and patch

        public ScoreHistoryPostEntry() {}

        public string ResultCOFID { get; set; } = "";

        public string MatchID { get; set; } = "";

        public string CourseOfFireDef { get; set; } = "";

        public DateTime LocalDate { get; set; } = DateTime.Today;

        public string MatchType { get; set; } = "";

        public string MatchName { get; set; } = "";

        public string MatchLocation { get; set; } = "";

        public string EventStyleDef { get; set; } = "";

        public List<PostStageStyleScore> StageScores {get; set;} = new List<PostStageStyleScore>();

    }
}
