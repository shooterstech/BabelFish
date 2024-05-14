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

        public void CreateSampleCOF()
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
            @event.Children = new List<string>() { "Prone","Standing","Kneeling" };
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

        [TestMethod]
        public async Task ProjectedAvgScoresMakerTests()
        {
            CreateSampleCOF();
            resultEvent.EventScores = new Dictionary<string, EventScore>();
            EventScore qually = new EventScore
            {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "EVENT"
            };
            resultEvent.EventScores.Add("Qualification", qually);


            EventScore standing = new EventScore
            {
                EventName = "Standing",
                Score = new DataModel.Athena.Score { I = 99, D = 101.4f, X = 4 },
                ScoreFormatted = "",
                NumShotsFired = 10,
                EventType = "STAGE"
            };
            resultEvent.EventScores.Add("Standing", standing);


            EventScore prone = new EventScore
            {
                EventName = "Prone",
                Score = new DataModel.Athena.Score { I = 0, D = 0, X = 0 },
                ScoreFormatted = "",
                NumShotsFired = 0,
                EventType = "STAGE"
            };
            resultEvent.EventScores.Add("Prone", prone);


            EventScore knee = new EventScore
            {
                EventName = "Kneeling",
                Score = new DataModel.Athena.Score { I = 198, D = 201.3f, X = 12 },
                ScoreFormatted = "",
                NumShotsFired = 20,
                EventType = "STAGE"
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
    }
}
