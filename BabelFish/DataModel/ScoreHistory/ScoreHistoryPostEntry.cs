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
        }

        public string StageStyleDef { get; set; } = "";
        public Score Score {get; set;}
        public int NumShots { get; set; } = 0;

    }

    public class ScoreHistoryPostEntry { //used for both post and patch

        public ScoreHistoryPostEntry() {}

        public string ResultCOFID { get; set; } = "";

        public string MatchID { get; set; } = "";

        public string CourseOfFireDef { get; set; } = "";

        public DateTime LocalDate { get; set; } = DateTime.Today;

		
		public VisibilityOption Visibility { get; set; } = VisibilityOption.PROTECTED;

		
		public MatchTypeOptions MatchType { get; set; } = MatchTypeOptions.PRACTICE;

        public string MatchName { get; set; } = "";

        public string MatchLocation { get; set; } = "";

        public string EventStyleDef { get; set; } = "";

        public List<PostStageStyleScore> StageScores {get; set;} = new List<PostStageStyleScore>();

    }
}
