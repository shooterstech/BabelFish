using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataModel.ScoreHistory {
    public class PostStageStyleScore {
        public PostStageStyleScore(){
        }

        public PostStageStyleScore(string stageStyleDef, Score score){
            StageStyleDef = stageStyleDef;
            this.Score = score;
            if (this.Score.NumShotsFired > 0 )
                this.NumShots = this.Score.NumShotsFired;
        }

        public PostStageStyleScore( string stageStyleDef, Score score, int numberOfShots ) {
            StageStyleDef = stageStyleDef;
            this.Score = score;
            this.NumShots = numberOfShots;
        }

        public string StageStyleDef { get; set; } = "";

        public Score Score {get; set;}

        public int NumShots { get; set; } = 0;

    }

    /// <summary>
    /// POST and PATCH data structure for submitting or updating scores to Score History
    /// </summary>
    public class ScoreHistoryPostEntry { 

        public ScoreHistoryPostEntry() {}

        public string ResultCOFID { get; set; } = string.Empty;

        public string MatchID { get; set; } = string.Empty;

        public string CourseOfFireDef { get; set; } = string.Empty;

        public DateTime LocalDate { get; set; } = DateTime.Today;

		
		public VisibilityOption Visibility { get; set; } = VisibilityOption.PROTECTED;

		
		public MatchTypeOptions MatchType { get; set; } = MatchTypeOptions.PRACTICE;

        public string MatchName { get; set; } = string.Empty;

        public string MatchLocation { get; set; } = string.Empty;

        public string EventStyleDef { get; set; } = string.Empty;

        public List<PostStageStyleScore> StageScores {get; set;} = new List<PostStageStyleScore>();

        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// Typical values are Integer or Decimal
        /// </summary>
        public string ScoreConfigName { get; set; } = "Integer";

        /// <summary>
        /// Typical values are Events or Shots
        /// </summary>
        public string ScoreFormatName { get; set; } = "Events";

    }
}
