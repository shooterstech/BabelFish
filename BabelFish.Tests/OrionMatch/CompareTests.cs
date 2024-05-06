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
using System.ComponentModel;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class CompareTests {

        List<MatchParticipant> participants = new List<MatchParticipant>();
        string JONES_CHRIS = "Jones, Chris";
        string JONES_MONIKA = "Jones, Monika";
        string SMITH_DEREK = "Smith, Derek";
        string SMITH_JANET = "Smith, Janet";

        [TestInitialize] 
        public void TestInitialize() {
            participants = new List<MatchParticipant>();

            participants.Add(
                new MatchParticipant() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Chris",
                        DisplayName = JONES_CHRIS,
                        CompetitorNumber = "101"
                    }
                }
                );

            participants.Add(
                new MatchParticipant() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Monika",
                        DisplayName = JONES_MONIKA,
                        CompetitorNumber = "102"
                    }
                }
                );

            participants.Add(
                new MatchParticipant() {
                    Participant = new Individual() {
                        FamilyName = "Smith",
                        GivenName = "Derek",
                        DisplayName = SMITH_DEREK,
                        CompetitorNumber = "103"
                    }
                }
                );

            participants.Add(
                new MatchParticipant() {
                    Participant = new Individual() {
                        FamilyName = "Smith",
                        GivenName = "Janet",
                        DisplayName = SMITH_JANET,
                        CompetitorNumber = "104"
                    }
                }
                );
        }

        [TestMethod]
        public async Task CompareParticipantTests() {

            participants.Sort( new CompareParticipant( CompareParticipant.CompareMethod.FAMILYNAME_GIVENNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS, participants[0].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, participants[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, participants[2].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, participants[3].Participant.DisplayName );

            participants.Sort( new CompareParticipant( CompareParticipant.CompareMethod.FAMILYNAME_GIVENNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( SMITH_JANET, participants[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, participants[1].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, participants[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS, participants[3].Participant.DisplayName );

            participants.Sort( new CompareParticipant( CompareParticipant.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS, participants[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, participants[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, participants[2].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, participants[3].Participant.DisplayName );

            participants.Sort( new CompareParticipant( CompareParticipant.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( JONES_MONIKA, participants[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, participants[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, participants[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS, participants[3].Participant.DisplayName );

            participants.Sort( new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS, participants[0].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, participants[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, participants[2].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, participants[3].Participant.DisplayName );

            participants.Sort( new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( SMITH_JANET, participants[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, participants[1].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, participants[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS, participants[3].Participant.DisplayName );

        }

        [TestMethod]
        public async Task CompareSquaddings() {

            //Call the GetSquaddingList api
            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.BETA );

            //This match id has three relays of 20 athletes
            var matchId = new MatchID( "1.1.2023022315342668.0" );
            var squaddingListName = "Individual";
            var taskSquaddingListResponse = client.GetSquaddingListPublicAsync( matchId, squaddingListName );
            var squaddingListResponse = taskSquaddingListResponse.Result;

            Assert.AreEqual( HttpStatusCode.OK, squaddingListResponse.StatusCode );
            var squaddingList = squaddingListResponse.SquaddingList;

            Assert.AreEqual( matchId.ToString(), squaddingList.MatchID );
            Assert.AreEqual( squaddingListName, squaddingList.EventName );

            Assert.IsTrue( squaddingList.Items.Count > 0 );
            var squaddingListSqAssignments = squaddingList.Items;
            var concreteSAFPList = new List<SquaddingAssignmentFiringPoint>();
            foreach (var fp in squaddingListSqAssignments) {
                concreteSAFPList.Add( (SquaddingAssignmentFiringPoint)fp );
            }

            var sortByRelayFiringPointAsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            concreteSAFPList.Sort( sortByRelayFiringPointAsc );
            Console.WriteLine( "sortByRelayFiringPointAsc" );
            foreach (var fp in concreteSAFPList) { Console.WriteLine( fp.ToString() ); Console.WriteLine( fp.Participant.DisplayName ); }


            Console.WriteLine( "\n\nNEXT\n" );

            var sortByRelayFiringPointDsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING );
            concreteSAFPList.Sort( sortByRelayFiringPointDsc );
            Console.WriteLine( "sortByRelayFiringPointDsc" );
            foreach (var fp in concreteSAFPList) { Console.WriteLine( fp.ToString() ); Console.WriteLine( fp.Participant.DisplayName ); }

            Console.WriteLine( "\n\nNEXT\n" );

            var sortByRelayFirstLastAsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            concreteSAFPList.Sort( sortByRelayFirstLastAsc );
            Console.WriteLine( "sortByRelayFirstLastAsc" );
            foreach (var fp in concreteSAFPList) { Console.WriteLine( fp.ToString() ); Console.WriteLine( fp.Participant.DisplayName ); }
        }
    }
}
