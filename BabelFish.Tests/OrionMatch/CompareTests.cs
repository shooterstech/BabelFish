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

        List<SquaddingAssignmentFiringPoint> SquaddingAssignmentFPList = new List<SquaddingAssignmentFiringPoint>();
        string JONES_CHRIS = "Jones, Chris";
        string JONES_MONIKA = "Jones, Monika";
        string SMITH_DEREK = "Smith, Derek";
        string SMITH_JANET = "Smith, Janet";

        [TestInitialize] 
        public void TestInitialize() {
            SquaddingAssignmentFPList = new List<SquaddingAssignmentFiringPoint>();

            SquaddingAssignmentFPList.Add(
                new SquaddingAssignmentFiringPoint() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Chris",
                        DisplayName = JONES_CHRIS,
                        CompetitorNumber = "101"
                    },
                    FiringPoint = "1",
                    Relay = "1"
                }
                );

            SquaddingAssignmentFPList.Add(
                new SquaddingAssignmentFiringPoint() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Monika",
                        DisplayName = JONES_MONIKA,
                        CompetitorNumber = "102"
                    },
                    FiringPoint = "2",
                    Relay = "1"
                }
                );

            SquaddingAssignmentFPList.Add(
                new SquaddingAssignmentFiringPoint() {
                    Participant = new Individual() {
                        FamilyName = "Smith",
                        GivenName = "Derek",
                        DisplayName = SMITH_DEREK,
                        CompetitorNumber = "103"
                    },
                    FiringPoint = "1",
                    Relay = "2"
                }
                );

            SquaddingAssignmentFPList.Add(
                new SquaddingAssignmentFiringPoint() {
                    Participant = new Individual() {
                        FamilyName = "Smith",
                        GivenName = "Janet",
                        DisplayName = SMITH_JANET,
                        CompetitorNumber = "104"
                    },
                    FiringPoint = "2",
                    Relay = "2"
                }
                );
        }

        [TestMethod]
        public async Task CompareParticipantTests() {

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.FAMILYNAME_GIVENNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.FAMILYNAME_GIVENNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( SMITH_JANET, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA,  SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( JONES_MONIKA, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( SMITH_JANET, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS, SquaddingAssignmentFPList[3].Participant.DisplayName );

        }

        [TestMethod]
        public async Task CompareSquaddings() {

            var sortByRelayFiringPointAsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFiringPointAsc );
            Assert.AreEqual(JONES_CHRIS, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(SMITH_JANET, SquaddingAssignmentFPList[3].Participant.DisplayName);

            var sortByRelayFiringPointDsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFiringPointDsc );
            Assert.AreEqual(SMITH_JANET, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(JONES_CHRIS, SquaddingAssignmentFPList[3].Participant.DisplayName);

            var sortByRelayFirstLastAsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFirstLastAsc );
            Assert.AreEqual(JONES_CHRIS, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(SMITH_JANET, SquaddingAssignmentFPList[3].Participant.DisplayName);
        }
    }
}
