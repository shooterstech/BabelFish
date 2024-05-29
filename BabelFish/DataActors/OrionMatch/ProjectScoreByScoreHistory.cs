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

        private ScoreAverageStageStyleEntry RetrieveScoreAvgEntry(string user, string stageStyle, ScoreAverageStageStyleEntry entry) //TODO if not found, should retrieve
        {
            if (scoreHistoryCache.ContainsKey(user))
            {
                if (scoreHistoryCache[user].ContainsKey(stageStyle))
                {
                    return scoreHistoryCache[user][stageStyle];
                }
            }

            return new ScoreAverageStageStyleEntry(); // TODO look up? otherwise zero scores?
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

            return eventScore.Projected;



        }

        /*
         * Projects the score of a single event of type Stage
         */
        private Score ProjectStageScore(EventComposite stageEvent, EventScore es)
        {
            //TODO project stage score

            //get singulars for stage I am in, then count those and that is how many shots to take total.
            var singulars = stageEvent.GetEvents(false, false, false, false, false, true);
            var shotsFired = es.NumShotsFired;
            //we want to always project shots if we have ANY remaining.
            var shotsRemaining = singulars.Count - shotsFired;
            var projectedIntThisStage = 0.0f;
            var projectedDecThisStage = 0.0f;
            var projectedXPerShot = 0.0f;
            if (shotsFired > 0 && singulars.Count > 0)
            { //if there are shots fired in this stage, and there should be, we should use those 
                projectedIntThisStage = (float)es.Score.I / (float)shotsFired;
                projectedDecThisStage = es.Score.D / shotsFired;
                projectedXPerShot = (float)es.Score.X / (float)shotsFired;

            }
            else if (shotsFired == 0)
            {
                if (!string.IsNullOrEmpty(es.StageStyleDef) && StageStyleScores.TryGetValue(es.StageStyleDef, out Score stageScores) && stageScores.NumShotsFired > 0)
                {//If we have scores for the stage style, use that as our default 
                    projectedIntThisStage = (float)stageScores.I / (float)stageScores.NumShotsFired;
                    projectedDecThisStage = stageScores.D / stageScores.NumShotsFired;
                    projectedXPerShot = (float)stageScores.X / (float)stageScores.NumShotsFired;
                }
                else
                {//If we donn't, use the overall average shot fired
                    projectedIntThisStage = (float)TopLevelEventScore.I / (float)TopLevelEventScore.NumShotsFired;
                    projectedDecThisStage = TopLevelEventScore.D / TopLevelEventScore.NumShotsFired;
                    projectedXPerShot = (float)TopLevelEventScore.X / (float)TopLevelEventScore.NumShotsFired;
                }
            }

            //project the scores
            es.Projected = new DataModel.Athena.Score();
            es.Projected.I = es.Score.I + (int)(projectedIntThisStage * shotsRemaining);
            es.Projected.D = es.Score.D + (projectedDecThisStage * shotsRemaining);
            es.Projected.D = (float)Math.Round(es.Projected.D, 1);
            es.Projected.S = es.Projected.D;
            es.Projected.X = (int)(es.Score.X + (projectedXPerShot * shotsRemaining));

            return es.Projected;


        }

    }
        
}
