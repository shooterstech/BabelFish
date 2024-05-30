using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.OrionMatch
{
    public class ProjectScoresByScoreHistory : ProjectorOfScores
    {
        private ScoreHistoryAPIClient scoreHistoryClient;
        public ProjectScoresByScoreHistory(CourseOfFire courseOfFire) : base(courseOfFire)
        {
            scoreHistoryClient = new ScoreHistoryAPIClient("uONGn6tHGw14kreLdqbfJ9rwR2C55uS8a9rGnmIf", APIStage.BETA);
        }

        public ProjectScoresByScoreHistory(CourseOfFire courseOfFire, CompareByRankingDirective teamMemberComparer) : base(courseOfFire, teamMemberComparer)
        {
            scoreHistoryClient = new ScoreHistoryAPIClient("uONGn6tHGw14kreLdqbfJ9rwR2C55uS8a9rGnmIf", APIStage.BETA);
        }

        public override string ProjectionMadeBy
        {
            get
            {
                return "BabelFish ProjectScoresByScoreHistory";
            }
        }

        /* scoreHistoryCache 
         * {
         *      "userID":{
         *          "stagestyleSetname": (ScoreAverageStageStyleEntry),
         *          ...
         *      },
         *      ...
         * }
         */
        private static Dictionary<string, Dictionary<string, ScoreAverageStageStyleEntry>> scoreHistoryCache = new Dictionary<string, Dictionary<string, ScoreAverageStageStyleEntry>>();


        private void AddOrUpdateScoreAvgEntry(string user, string stageStyle, ScoreAverageStageStyleEntry entry)
        {
            if (!scoreHistoryCache.ContainsKey(user))
            {
                scoreHistoryCache[user] = new Dictionary<string, ScoreAverageStageStyleEntry>();
            }

            scoreHistoryCache[user][stageStyle] = entry;
        }

        private AveragedScore RetrieveAvgScoreHistory(string user, string stageStyle) //TODO if not found, should attempt retrieve?
        {
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(stageStyle) && scoreHistoryCache.ContainsKey(user))
            {
                if (scoreHistoryCache[user].ContainsKey(stageStyle))
                {
                    return scoreHistoryCache[user][stageStyle].ScoreAverage;
                }
            }

            return new AveragedScore(); // TODO look up? otherwise zero scores?
        }

        public override async Task PreInitAsync(List<IEventScoreProjection> listOfParticipants)
        {
            List<string> uids = new List<string>();
            HashSet<SetName> stageStyles = new HashSet<SetName>();
            foreach (var part in listOfParticipants) //get list of non-empty user ids
            {
                var ind = (Individual)part.Participant;
                if (!string.IsNullOrEmpty(ind.UserID))
                {
                    uids.Add(ind.UserID);
                }

                foreach (var ev in part.EventScores)
                {
                    if (!string.IsNullOrEmpty(ev.Value.StageStyleDef))
                    {
                        stageStyles.Add(SetName.Parse(ev.Value.StageStyleDef));
                    }
                }

            }

            /*var scoreAverageRequest = new GetScoreAveragePublicRequest();
            scoreAverageRequest.UserIds = uids;
            scoreAverageRequest.StageStyleDefs = stageStyles.ToList();
            var scoreHistoryClient = new ScoreHistoryAPIClient("uONGn6tHGw14kreLdqbfJ9rwR2C55uS8a9rGnmIf", APIStage.BETA); //TODO DONT HARD CODE!@!
            var scoreAverageResponse = await scoreHistoryClient.GetScoreAveragePublicAsync(scoreAverageRequest);
            Console.WriteLine("")
            if (System.Net.HttpStatusCode.OK == scoreAverageResponse.StatusCode)
            {

            }*/

            var scoreAverageRequest = new GetScoreAveragePublicRequest
            {
                UserIds = uids,
                StageStyleDefs = stageStyles.ToList()
            };



            try
            {
                var scoreAverageResponse = await scoreHistoryClient.GetScoreAveragePublicAsync(scoreAverageRequest);

                if (scoreAverageResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (ScoreAverageStageStyleEntry scoreAvg in scoreAverageResponse.ScoreAverageList.Items)
                    {
                        AddOrUpdateScoreAvgEntry(scoreAvg.UserId, scoreAvg.StageStyleDef, scoreAvg);
                    }

                }
                else
                {
                    // Handle non-successful response
                    Console.WriteLine($"Error: {scoreAverageResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }






        //Projection and AvgShots exists as class variable to make recussion easier. They need to be cleared with each call to ProjectAthleteScores()
        private IEventScoreProjection Projection { get; set; }

        //first key is stage style setname, second key is avgType [(I)INT, (D)DEC, (X)INNER] value is avg.
        private Dictionary<string, Score> StageStyleScores = new Dictionary<string, Score>();

        private Score TopLevelEventScore;

        /// <inheritdoc/>
        /// <param name="projection"></param>
        public override void ProjectAthleteScores(IEventScoreProjection projection)
        {

            if (projection.Participant is Team)
            {
                //This is more of an Assert statement. We shouldn't get here if ProjectorOfScores.ProjectEventScores is written correctly.
                throw new ArgumentException("ProjectAthleteScores can not project scores if the .Participant is a Team.");
            }

            this.Projection = projection;
            this.StageStyleScores.Clear();

            //calculate total score for each stage style
            //this value is used in the calculation of stages with no shots fired yet
            foreach (var es in projection.EventScores)
            {
                es.Value.Score.NumShotsFired = es.Value.NumShotsFired;
                if (!string.IsNullOrEmpty(es.Value.StageStyleDef))
                {
                    if (!StageStyleScores.ContainsKey(es.Value.StageStyleDef))
                    {
                        StageStyleScores.Add(es.Value.StageStyleDef, new Score());
                    }
                    StageStyleScores[es.Value.StageStyleDef] += es.Value.Score;
                }
            }


            if (!this.Projection.EventScores.TryGetValue(TopLevelEvent.EventName, out EventScore topEvent))
            {
                Console.Write("Event name not found, this should NOT happen!!");
                throw new ArgumentException("Top level name not found, this should NOT happen!!");
            }
            TopLevelEventScore = topEvent.Score; //used for overall avg shot fired

            RecurProjectScores(TopLevelEvent);

        }

        /*
         * recursively project the event score and modify this.Projection
         */
        private Score RecurProjectScores(EventComposite eventComposite)
        {
            EventScore eventScore;
            if (!this.Projection.EventScores.TryGetValue(eventComposite.EventName, out eventScore))
            {
                Console.Write("Event name not found, this should NOT happen!!");
            }

            if (!EventtType.TryParse(eventScore.EventType, out EventtType eventType))
            {
                Console.Write("Event type invalid, this should NOT happen!!");
                eventType = EventtType.NONE;
            }

            if (eventType == EventtType.STAGE)
            {
                return ProjectStageScore(eventComposite, eventScore);

            }

            eventScore.Projected = new Score();
            foreach (var childEvent in eventComposite.Children)
            {//To project the score for any stage, sum the projected scores of it's children
                Score childScore = RecurProjectScores(childEvent);
                if (eventComposite.Calculation != "SUM" && childEvent.Equals(eventComposite.Children[0])) //I am not entirely sure what this does but I am copying the logic from the AvgShotFiredProjector
                {//SUM(i,d) ... of which yes, we really ought to be parsing it, but i ain't got tiem for that
                    childScore.S = childScore.I;
                }

                eventScore.Projected += childScore; //RecurProjectScores(childEvent);
            }

            eventScore.Projected.D = (float)Math.Round(eventScore.Projected.D, 1);
            eventScore.Projected.S = (float)Math.Round(eventScore.Projected.S, 1);
            return eventScore.Projected;



        }

        /*
         * Projects the score of a single event of type Stage
         */
        private Score ProjectStageScore(EventComposite stageEvent, EventScore es)
        {
            //get singulars for stage I am in, then count those and that is how many shots to take total.
            var singulars = stageEvent.GetEvents(false, false, false, false, false, true);
            var shotsFired = es.NumShotsFired;
            var numShotsInEvent = singulars.Count;
            //we want to always project shots if we have ANY remaining.
            var shotsRemaining = numShotsInEvent - shotsFired;
            float percentageShotsTaken = (float)shotsFired / (float)numShotsInEvent;

            es.Projected = new DataModel.Athena.Score();
            if (numShotsInEvent <= 0)
            {
                //no expected shots for this stage, this shouldnt happen
                return es.Projected;
            }

            AveragedScore avgScoreThisStage; 
            if (shotsFired > 0 )
            { //if there are shots fired in this stage, and there should be, we should use those 
                avgScoreThisStage = new AveragedScore(es.Score);
                avgScoreThisStage /= (float)shotsFired;

            }
            else //no shots fired
            {
                if (!string.IsNullOrEmpty(es.StageStyleDef) && StageStyleScores.TryGetValue(es.StageStyleDef, out Score stageScores) && stageScores.NumShotsFired > 0)
                {//If we have scores for the stage style, use that as our default 
                    avgScoreThisStage = new AveragedScore(stageScores);
                    avgScoreThisStage /= (float)stageScores.NumShotsFired;
                }
                else if (TopLevelEventScore.NumShotsFired > 0)
                {//If we don't but have other shots in the match, use the overall average shot fired
                    avgScoreThisStage = new AveragedScore(TopLevelEventScore);
                    avgScoreThisStage /= (float)TopLevelEventScore.NumShotsFired;

                }
                else //otherwise no shots have been fired, predict 0
                {
                    avgScoreThisStage = new AveragedScore();
                }
               
            }

            AveragedScore scoreHistoryAvg = RetrieveAvgScoreHistory(((Individual)Projection.Participant).UserID, es.StageStyleDef);
            if (scoreHistoryAvg.IsZero)
            {
                scoreHistoryAvg = avgScoreThisStage; //If no score history, then use avg shot fired
            }
            
            es.Projected.I = (int)PredictScore(es.Score.I, avgScoreThisStage.I, scoreHistoryAvg.I, shotsRemaining, percentageShotsTaken);
            es.Projected.D = PredictScore(es.Score.D, avgScoreThisStage.D, scoreHistoryAvg.D, shotsRemaining, percentageShotsTaken);
            es.Projected.X = (int)PredictScore(es.Score.X, avgScoreThisStage.X, scoreHistoryAvg.X, shotsRemaining, percentageShotsTaken);

            es.Projected.D = (float)Math.Round(es.Projected.D, 1);
            es.Projected.S = es.Projected.D;

            return es.Projected;


        }


        /*   From SQL query
         CREATE FUNCTION predict_score(
		    current_score decimal(16, 8), 
		    num_shots_taken int,
		    num_shots_total int,
		    history_avg_shot decimal(16, 8)
	    ) 
	        RETURNS decimal(6, 1) DETERMINISTIC
        BEGIN
	        DECLARE num_shots_left int DEFAULT num_shots_total - num_shots_taken;
	        DECLARE match_avg_shot decimal(16, 8) DEFAULT IF(num_shots_taken>0, current_score / num_shots_taken, 0.0);
	        DECLARE percentage_shots_taken decimal(16, 8) DEFAULT num_shots_taken/num_shots_total;
	
	        IF (ISNULL(history_avg_shot) OR history_avg_shot = 0) THEN
		        RETURN current_score + (num_shots_left*match_avg_shot) ;
	        END IF;avgIntThisStage
    
	        RETURN current_score 
			        + (num_shots_left*match_avg_shot)*percentage_shots_taken 
			        + (num_shots_left*history_avg_shot)*(1.0-percentage_shots_taken);#TODO IMPROVE
	
        END              */
        private float PredictScore(float currentScore, float avgScore, float historyAvg, int shotsRemaining, float percentageShotsTaken)
        {
            return currentScore + shotsRemaining * (
                    avgScore * percentageShotsTaken +
                    historyAvg * (1.0f - percentageShotsTaken)
                );
        }

    }
        
}
