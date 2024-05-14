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

        public void CreateSampleResultEvent()
        {
            //need to figure out how to add eventScores.
            resultEvent.EventScores = new Dictionary<string, EventScore>();
            EventScore qually = new EventScore
            {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score { I = 99, D = 101.4f, X = 4 },
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
        }

        public void CreateSampleCOF()
        {
            courseOfFire.Singulars = new List<Singular>();
            courseOfFire.Events = new List<Event>();

            Singular blah = new Singular
            {
                Values = "1..20",
                EventName = "S{}",
                ScoreFormat = "Shots",
                StageLabel = "S",
                Type = SingularType.SHOT
            };
            Singular nah = new Singular();
            nah.Values = "1..20";
            nah.EventName = "P{}";
            nah.ScoreFormat = "Shots";
            nah.StageLabel = "P";
            nah.Type = SingularType.SHOT;
            courseOfFire.Singulars.Add(nah);
            courseOfFire.Singulars.Add( blah );

            Event @event = new Event();
            @event.EventName = "Qualification";
            @event.EventType = EventtType.EVENT;
            @event.Children = new List<string>() { "Standing","Prone" };
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

            courseOfFire.Events.Add(@event);
            courseOfFire.Events.Add(@stage1);
            courseOfFire.Events.Add(@stage2);
        }

        [TestMethod]
        public async Task ProjectedAvgScoresMakerTests()
        {
            CreateSampleCOF();
            CreateSampleResultEvent();

            resultEvent.ProjectScores(new ProjectScoresByAverageShotFired(courseOfFire));
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(resultEvent));
            Assert.AreEqual(resultEvent.EventScores["Standing"].Projected.I, 198);
            Assert.AreEqual(resultEvent.EventScores["Standing"].Projected.D, 202.8f);
            Assert.AreEqual(resultEvent.EventScores["Standing"].Projected.X, 8);
            Assert.AreEqual(resultEvent.EventScores["Prone"].Projected.I, 198);
            Assert.AreEqual(resultEvent.EventScores["Prone"].Projected.D, 202.8f);
            Assert.AreEqual(resultEvent.EventScores["Prone"].Projected.X, 8);
            Assert.AreEqual(resultEvent.EventScores["Qualification"].Projected.I, 396);
            Assert.AreEqual(resultEvent.EventScores["Qualification"].Projected.D, 405.6f);
            Assert.AreEqual(resultEvent.EventScores["Qualification"].Projected.X, 16);
        }
    }
}
