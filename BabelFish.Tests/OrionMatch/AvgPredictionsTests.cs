using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Tests.Definition;


namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class AvgPredictionsTests : BaseTestClass
    {
        public CourseOfFire courseOfFire = new CourseOfFire();
        public ResultEvent resultEvent = new ResultEvent();


        [TestMethod]
        public async Task ProjectedAvgScoresMaker3x20Test()
        {
            courseOfFire = CourseOfFireHelper.Get_3x20_KPS_Cof();
            resultEvent.EventScores = new Dictionary<string, EventScore>();
            EventScore qually = new EventScore
            {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "EVENT",
                Status = ResultStatus.INTERMEDIATE,
                EventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle"
            };
            resultEvent.EventScores.Add("Qualification", qually);


            EventScore standing = new EventScore
            {
                EventName = "Standing",
                Score = new DataModel.Athena.Score { I = 99, D = 101.4f, X = 4 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "STAGE",
                Status = ResultStatus.INTERMEDIATE,
                StageStyleDef = "v1.0:ntparc:Sporter Air Rifle Standing"
            };
            resultEvent.EventScores.Add("Standing", standing);


            EventScore prone = new EventScore
            {
                EventName = "Prone",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 0,
                EventType = "STAGE",
                Status = ResultStatus.FUTURE,
                StageStyleDef = "v1.0:ntparc:Sporter Air Rifle Prone"
            };
            resultEvent.EventScores.Add("Prone", prone);


            EventScore knee = new EventScore
            {
                EventName = "Kneeling",
                Score = new DataModel.Athena.Score { I = 198, D = 201.3f, X = 12 },
                ScoreFormatted = "",
                NumShotsFired = 20,
                EventType = "STAGE",
                Status = ResultStatus.UNOFFICIAL,
                StageStyleDef = "v1.0:ntparc:Sporter Air Rifle Kneeling"
            };
            resultEvent.EventScores.Add("Kneeling", knee);


            resultEvent.ProjectScores(new ProjectScoresByAverageShotFired(courseOfFire));
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(resultEvent));

            //flip these around plz, better output on fail.
            Assert.AreEqual(198,    resultEvent.EventScores["Standing"].Projected.I);
            Assert.AreEqual(202.8f, resultEvent.EventScores["Standing"].Projected.D);
            Assert.AreEqual(8,      resultEvent.EventScores["Standing"].Projected.X);

            Assert.AreEqual(198,    resultEvent.EventScores["Prone"].Projected.I);
            Assert.AreEqual(202.1f, resultEvent.EventScores["Prone"].Projected.D);
            Assert.AreEqual(10,      resultEvent.EventScores["Prone"].Projected.X);

            Assert.AreEqual(198,    resultEvent.EventScores["Kneeling"].Projected.I);
            Assert.AreEqual(201.3f, resultEvent.EventScores["Kneeling"].Projected.D);
            Assert.AreEqual(12,     resultEvent.EventScores["Kneeling"].Projected.X);

            Assert.AreEqual(594,    resultEvent.EventScores["Qualification"].Projected.I);
            Assert.AreEqual(606.2f, resultEvent.EventScores["Qualification"].Projected.D);
            Assert.AreEqual(30,     resultEvent.EventScores["Qualification"].Projected.X);
        }

        [TestMethod]
        public async Task ProjectedAvgScoresMaker4x10Test() {

            courseOfFire = CourseOfFireHelper.Get_4x10_Cof();

            resultEvent.EventScores = new Dictionary<string, EventScore>();
            EventScore qually = new EventScore {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "EVENT",
                Status = ResultStatus.INTERMEDIATE,
                EventStyleDef = "v1.0:nra:BB Gun 4P with Test Qualification"
            };
            resultEvent.EventScores.Add( "Qualification", qually );

            EventScore positions = new EventScore {
                EventName = "Positions",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "NONE",
                Status = ResultStatus.INTERMEDIATE
            };
            resultEvent.EventScores.Add( "Positions", positions );


            EventScore prone = new EventScore {
                EventName = "Prone",
                Score = new DataModel.Athena.Score { I = 97, D = 101.4f, X = 4 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "STAGE",
                Status = ResultStatus.INTERMEDIATE,
                StageStyleDef = "v1.0:nra:BB Gun Prone"
            };
            resultEvent.EventScores.Add( "Prone", prone );


            EventScore standing = new EventScore {
                EventName = "Standing",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 0,
                EventType = "STAGE",
                Status = ResultStatus.FUTURE,
                StageStyleDef = "v1.0:nra:BB Gun Standing"
            };
            resultEvent.EventScores.Add( "Standing", standing );


            EventScore sitting = new EventScore {
                EventName = "Sitting",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 0,
                EventType = "STAGE",
                Status = ResultStatus.UNOFFICIAL,
                StageStyleDef = "v1.0:nra:BB Gun Sitting"
            };
            resultEvent.EventScores.Add( "Sitting", sitting );


            EventScore knee = new EventScore {
                EventName = "Kneeling",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 0,
                EventType = "STAGE",
                Status = ResultStatus.UNOFFICIAL,
                StageStyleDef = "v1.0:nra:BB Gun Kneeling"
            };
            resultEvent.EventScores.Add( "Kneeling", knee );

            /*
             * It is actually an error to say that the number of shots fired on a Test is 10 shots,
             * it should be *1* shot. However, that 1 shot is worth 100 points, so when doing so
             * it will throw off the calculations for projected score. By setting the number of 
             * shots to 10, we avoid the larger issue ... for now.
             */
            EventScore test = new EventScore {
                EventName = "Test",
                Score = new DataModel.Athena.Score { I = 85, D = 85, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "STAGE",
                Status = ResultStatus.UNOFFICIAL,
                StageStyleDef = "v1.0:nra:BB Gun Test"
            };
            resultEvent.EventScores.Add( "Test", test );


            resultEvent.ProjectScores( new ProjectScoresByAverageShotFired( courseOfFire ) );
            Console.WriteLine( System.Text.Json.JsonSerializer.Serialize( resultEvent ) );

            Assert.AreEqual( 97, resultEvent.EventScores["Prone"].Projected.I );
            Assert.AreEqual( 101.4f, resultEvent.EventScores["Prone"].Projected.D );
            Assert.AreEqual( 4, resultEvent.EventScores["Prone"].Projected.X );

            Assert.AreEqual( 85, resultEvent.EventScores["Test"].Projected.I );

            Assert.AreEqual( 91, resultEvent.EventScores["Standing"].Projected.I );
            Assert.AreEqual( 93.2f, resultEvent.EventScores["Standing"].Projected.D );
            Assert.AreEqual( 2, resultEvent.EventScores["Standing"].Projected.X );

            Assert.AreEqual( 91, resultEvent.EventScores["Kneeling"].Projected.I );
            Assert.AreEqual( 93.2f, resultEvent.EventScores["Kneeling"].Projected.D );
            Assert.AreEqual( 2, resultEvent.EventScores["Kneeling"].Projected.X );

            Assert.AreEqual( 455, resultEvent.EventScores["Qualification"].Projected.I );
            Assert.AreEqual( 466.0f, resultEvent.EventScores["Qualification"].Projected.D );
            Assert.AreEqual( 10, resultEvent.EventScores["Qualification"].Projected.X );
        }
    }
}
