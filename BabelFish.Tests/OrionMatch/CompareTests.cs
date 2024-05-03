using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.Tests;
using Newtonsoft.Json;

namespace BabelFish.Tests.OrionMatch
{

    [TestClass]
    public class CompareTests
    {

        [TestMethod]
        public async Task CompareParticipatns()
        {
            //Call the GetSquaddingList api
            var client = new OrionMatchAPIClient(Constants.X_API_KEY, APIStage.BETA);

            //This match id has three relays of 20 athletes
            var matchId = new MatchID("1.1.2023022315342668.0");
            var squaddingListName = "Individual";
            var taskSquaddingListResponse = client.GetSquaddingListPublicAsync(matchId, squaddingListName);
            var squaddingListResponse = taskSquaddingListResponse.Result;

            Assert.AreEqual(HttpStatusCode.OK, squaddingListResponse.StatusCode);
            var squaddingList = squaddingListResponse.SquaddingList;

            Assert.AreEqual(matchId.ToString(), squaddingList.MatchID);
            Assert.AreEqual(squaddingListName, squaddingList.EventName);

            Assert.IsTrue(squaddingList.Items.Count > 0);
            var squaddingListSqAssignments = squaddingList.Items;
            //Call the GetSquaddingList api
            //var squaddingList = new List<SquaddingAssignment>();

            var sortByLastNameAsc = new CompareParticipant(CompareParticipant.CompareMethod.LASTNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING);
            squaddingListSqAssignments.Sort(sortByLastNameAsc);
            Console.WriteLine("sortByLastNameAsc");
            foreach (var fp in squaddingListSqAssignments) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }

            Console.WriteLine("\n\nNEXT\n");

            var sortByDisplayNameDsc = new CompareParticipant(CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING);
            squaddingListSqAssignments.Sort(sortByDisplayNameDsc);
            Console.WriteLine("sortByDisplayNameDsc");
            foreach (var fp in squaddingListSqAssignments) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }

        }

        [TestMethod]
        public async Task CompareSquaddings()
        {

            //Call the GetSquaddingList api
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID("1.1.2023022315342668.0");
            var squaddingListName = "Individual";
            var taskSquaddingListResponse = client.GetSquaddingListPublicAsync(matchId, squaddingListName);
            var squaddingListResponse = taskSquaddingListResponse.Result;

            Assert.AreEqual(HttpStatusCode.OK, squaddingListResponse.StatusCode);
            var squaddingList = squaddingListResponse.SquaddingList;

            Assert.AreEqual(matchId.ToString(), squaddingList.MatchID);
            Assert.AreEqual(squaddingListName, squaddingList.EventName);

            Assert.IsTrue(squaddingList.Items.Count > 0);
            var squaddingListSqAssignments = squaddingList.Items;
            var concreteSAFPList = new List<SquaddingAssignmentFiringPoint>();
            foreach (var fp in squaddingListSqAssignments)
            {
                concreteSAFPList.Add((SquaddingAssignmentFiringPoint)fp);
            }

            var sortByRelayFiringPointAsc = new CompareSquadding(CompareSquadding.CompareMethod.RELAYFIRINGPOINT, Scopos.BabelFish.Helpers.SortBy.ASCENDING);
            concreteSAFPList.Sort(sortByRelayFiringPointAsc);
            Console.WriteLine("sortByRelayFiringPointAsc");
            foreach (var fp in concreteSAFPList) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }

            Console.WriteLine("\n\nNEXT\n");

            var sortByRelayAsc = new CompareSquadding(CompareSquadding.CompareMethod.RELAY, Scopos.BabelFish.Helpers.SortBy.ASCENDING);
            concreteSAFPList.Sort(sortByRelayAsc);
            Console.WriteLine("sortByRelayAsc");
            foreach (var fp in concreteSAFPList) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }

            Console.WriteLine("\n\nNEXT\n");

            var sortByFiringPointDsc = new CompareSquadding(CompareSquadding.CompareMethod.FIRINGPOINT, Scopos.BabelFish.Helpers.SortBy.DESCENDING);
            concreteSAFPList.Sort(sortByFiringPointDsc);
            Console.WriteLine("sortByFiringPointDsc");
            foreach (var fp in concreteSAFPList) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }

            Console.WriteLine("\n\nNEXT\n");

            var sortByRelayFiringPointDsc = new CompareSquadding(CompareSquadding.CompareMethod.RELAYFIRINGPOINT, Scopos.BabelFish.Helpers.SortBy.DESCENDING);
            concreteSAFPList.Sort(sortByRelayFiringPointDsc);
            Console.WriteLine("sortByRelayFiringPointDsc");
            foreach (var fp in concreteSAFPList) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }

            Console.WriteLine("\n\nNEXT\n");

            var sortByRelayFirstLastAsc = new CompareSquadding(CompareSquadding.CompareMethod.RELAYFIRSTLAST, Scopos.BabelFish.Helpers.SortBy.ASCENDING);
            concreteSAFPList.Sort(sortByRelayFirstLastAsc);
            Console.WriteLine("sortByRelayFirstLastAsc");
            foreach (var fp in concreteSAFPList) { Console.WriteLine(fp.ToString()); Console.WriteLine(fp.Participant.DisplayName); }
        }
    }
}
