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
using Scopos.BabelFish.DataActors.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch
{

    [TestClass]
    public class CompareTests {

        List<SquaddingAssignmentFiringPoint> SquaddingAssignmentFPList = new List<SquaddingAssignmentFiringPoint>();
        string JONES_CHRIS_R1_FP1 = "Jones, Chris";
        string JONES_MONIKA_R1_FP2 = "Jones, Monika";
        string SMITH_DEREK_R2_FP1 = "Smith, Derek";
        string SMITH_JANET_R2_FP2 = "Smith, Janet";

        [TestInitialize] 
        public void TestInitialize() {
            SquaddingAssignmentFPList = new List<SquaddingAssignmentFiringPoint>();

            SquaddingAssignmentFPList.Add(
                new SquaddingAssignmentFiringPoint() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Chris",
                        DisplayName = JONES_CHRIS_R1_FP1,
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
                        DisplayName = JONES_MONIKA_R1_FP2,
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
                        DisplayName = SMITH_DEREK_R2_FP1,
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
                        DisplayName = SMITH_JANET_R2_FP2,
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
            Assert.AreEqual( JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.FAMILYNAME_GIVENNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA_R1_FP2,  SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.GIVENNAME_FAMILYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING ) );
            Assert.AreEqual( JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[3].Participant.DisplayName );

            SquaddingAssignmentFPList.Sort( new CompareParticipant( CompareParticipant.CompareMethod.DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING ) );
            Assert.AreEqual( SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[0].Participant.DisplayName );
            Assert.AreEqual( SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[1].Participant.DisplayName );
            Assert.AreEqual( JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[2].Participant.DisplayName );
            Assert.AreEqual( JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[3].Participant.DisplayName );

        }

        [TestMethod]
        public async Task CompareSquaddingTests() {

            var sortByRelayFiringPointAsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFiringPointAsc );
            Assert.AreEqual(JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[3].Participant.DisplayName);

            var sortByRelayFiringPointDsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFiringPointDsc );
            Assert.AreEqual(SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[3].Participant.DisplayName);

            var sortByRelayFirstLastAsc = new CompareSquaddingAssignmentFiringPoint( CompareSquaddingAssignmentFiringPoint.CompareMethod.RELAY_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFirstLastAsc );
            Assert.AreEqual(JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[3].Participant.DisplayName);
        }

        [TestMethod]
        public async Task CompareResultStatusTests() {

            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.FUTURE, ResultStatus.FUTURE ) == 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.FUTURE, ResultStatus.INTERMEDIATE ) < 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.FUTURE, ResultStatus.UNOFFICIAL ) < 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.FUTURE, ResultStatus.OFFICIAL ) < 0 );

            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.INTERMEDIATE, ResultStatus.FUTURE ) > 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.INTERMEDIATE, ResultStatus.INTERMEDIATE ) == 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.INTERMEDIATE, ResultStatus.UNOFFICIAL ) < 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.INTERMEDIATE, ResultStatus.OFFICIAL ) < 0);

            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.UNOFFICIAL, ResultStatus.FUTURE ) > 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.UNOFFICIAL, ResultStatus.INTERMEDIATE ) > 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.UNOFFICIAL, ResultStatus.UNOFFICIAL ) == 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.UNOFFICIAL, ResultStatus.OFFICIAL ) < 0 );

            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.OFFICIAL, ResultStatus.FUTURE ) > 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.OFFICIAL, ResultStatus.INTERMEDIATE ) > 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.OFFICIAL, ResultStatus.UNOFFICIAL ) > 0 );
            Assert.IsTrue( CompareResultStatus.COMPARER.Compare( ResultStatus.OFFICIAL, ResultStatus.OFFICIAL ) == 0 );
        }
    }
}
