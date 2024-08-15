using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.OrionMatch;
using Amazon.DynamoDBv2.Model;
using Scopos.BabelFish.DataModel.Definitions;
using System.Collections.Generic;
using System;


namespace Scopos.BabelFish.Tests.OrionMatch
{

    [TestClass]
    public class AvgPredictionsTests
    {
        public CourseOfFire courseOfFire = new CourseOfFire();
        public ResultEvent resultEvent = new ResultEvent();

        public void CreateSample3x20COF()
        {
            courseOfFire.Singulars = new List<Singular>();
            courseOfFire.Events = new List<Event>();

            Singular stand = new Singular
            {
                Values = "1..20",
                EventName = "S{}",
                ScoreFormat = "Shots",
                StageLabel = "S",
                Type = SingularType.SHOT
            };
            Singular pron = new Singular();
            pron.Values = "1..20";
            pron.EventName = "P{}";
            pron.ScoreFormat = "Shots";
            pron.StageLabel = "P";
            pron.Type = SingularType.SHOT;
            Singular knee = new Singular
            {
                Values = "1..20",
                EventName = "K{}",
                ScoreFormat = "Shots",
                StageLabel = "K",
                Type = SingularType.SHOT
            };
            courseOfFire.Singulars.Add(stand);
            courseOfFire.Singulars.Add(pron);
            courseOfFire.Singulars.Add(knee);

            Event @event = new Event();
            @event.EventName = "Qualification";
            @event.EventType = EventtType.EVENT;
            @event.Children = new List<string>() { "Kneeling", "Prone","Standing" };
            @event.Calculation = "SUM";
            @event.ScoreFormat = "Events";

            Event @stage1 = new Event();
            @stage1.EventName = "Prone";
            @stage1.EventType = EventtType.STAGE;
            var dict1 = new Dictionary<string, string>
            {
                { "EventName", "P{}" },
                { "Values", "1..20" }
            };
            @stage1.Children = dict1;
            @stage1.Calculation = "SUM";
            @stage1.ScoreFormat = "Events";

            Event @stage2 = new Event();
            @stage2.EventName = "Standing";
            @stage2.EventType = EventtType.STAGE;
            var dict2 = new Dictionary<string, string>
            {
                { "EventName", "S{}" },
                { "Values", "1..20" }
            };
            @stage2.Children = dict2;
            @stage2.Calculation = "SUM";
            @stage2.ScoreFormat = "Events";

            Event @stage3 = new Event();
            @stage3.EventName = "Kneeling";
            @stage3.EventType = EventtType.STAGE;
            var dict3 = new Dictionary<string, string>
            {
                { "EventName", "K{}" },
                { "Values", "1..20" }
            };
            @stage3.Children = dict3;
            @stage3.Calculation = "SUM";
            @stage3.ScoreFormat = "Events";

            courseOfFire.Events.Add(@event);
            courseOfFire.Events.Add(@stage1);
            courseOfFire.Events.Add(@stage2);
            courseOfFire.Events.Add(@stage3);
        }


        public void CreateSample4x10COF() {
            courseOfFire.Singulars = new List<Singular>();
            courseOfFire.Events = new List<Event>();

            Singular stand = new Singular {
                Values = "1..10",
                EventName = "S{}",
                ScoreFormat = "Shots",
                StageLabel = "S",
                Type = SingularType.SHOT
            };
            Singular pron = new Singular();
            pron.Values = "1..10";
            pron.EventName = "P{}";
            pron.ScoreFormat = "Shots";
            pron.StageLabel = "P";
            pron.Type = SingularType.SHOT;
            Singular knee = new Singular {
                Values = "1..10",
                EventName = "K{}",
                ScoreFormat = "Shots",
                StageLabel = "K",
                Type = SingularType.SHOT
            };
            Singular sitting = new Singular {
                Values = "1..10",
                EventName = "I{}",
                ScoreFormat = "Shots",
                StageLabel = "I",
                Type = SingularType.SHOT
            };
            Singular test = new Singular {
                Values = "1",
                EventName = "T{}",
                ScoreFormat = "Tests",
                StageLabel = "T",
                Type = SingularType.TEST
            };
            courseOfFire.Singulars.Add( pron );
            courseOfFire.Singulars.Add( stand );
            courseOfFire.Singulars.Add( sitting );
            courseOfFire.Singulars.Add( knee );
            courseOfFire.Singulars.Add( test );

            Event @event = new Event();
            @event.EventName = "Qualification";
            @event.EventType = EventtType.EVENT;
            @event.Children = new List<string>() { "Positions", "Test" };
            @event.Calculation = "SUM";
            @event.ScoreFormat = "Events";

            Event positions = new Event();
            positions.EventName = "Positions";
            positions.EventType = EventtType.NONE;
            positions.Children = new List<string>() { "Prone", "Standing", "Sitting", "Kneeling"};
            positions.Calculation = "SUM";
            positions.ScoreFormat = "Events";

            Event @stage1 = new Event();
            @stage1.EventName = "Prone";
            @stage1.EventType = EventtType.STAGE;
            var dict1 = new Dictionary<string, string>
            {
                { "EventName", "P{}" },
                { "Values", "1..10" }
            };
            @stage1.Children = dict1;
            @stage1.Calculation = "SUM";
            @stage1.ScoreFormat = "Events";

            Event @stage2 = new Event();
            @stage2.EventName = "Standing";
            @stage2.EventType = EventtType.STAGE;
            var dict2 = new Dictionary<string, string>
            {
                { "EventName", "S{}" },
                { "Values", "1..10" }
            };
            @stage2.Children = dict2;
            @stage2.Calculation = "SUM";
            @stage2.ScoreFormat = "Events";

            Event @stage3 = new Event();
            @stage3.EventName = "Kneeling";
            @stage3.EventType = EventtType.STAGE;
            var dict3 = new Dictionary<string, string>
            {
                { "EventName", "K{}" },
                { "Values", "1..10" }
            };
            @stage3.Children = dict3;
            @stage3.Calculation = "SUM";
            @stage3.ScoreFormat = "Events";

            Event @stage4 = new Event();
            @stage4.EventName = "Sitting";
            @stage4.EventType = EventtType.STAGE;
            var dict4 = new Dictionary<string, string>
            {
                { "EventName", "I{}" },
                { "Values", "1..10" }
            };
            @stage4.Children = dict4;
            @stage4.Calculation = "SUM";
            @stage4.ScoreFormat = "Events";

            Event @stage5 = new Event();
            @stage5.EventName = "Test";
            @stage5.EventType = EventtType.STAGE;
            @stage5.Children = new List<string>() { "T1" }; ;
            @stage5.Calculation = "SUM";
            @stage5.ScoreFormat = "Tests";

            courseOfFire.Events.Add( @event );
            courseOfFire.Events.Add( positions );
            courseOfFire.Events.Add( @stage1 );
            courseOfFire.Events.Add( @stage2 );
            courseOfFire.Events.Add( @stage3 );
            courseOfFire.Events.Add( @stage4 );
            courseOfFire.Events.Add( @stage5 );
        }

        [TestMethod]
        public async Task ProjectedAvgScoresMaker3x20Test()
        {
            CreateSample3x20COF();
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
            CreateSample4x10COF();
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
