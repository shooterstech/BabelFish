using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {

    [TestClass]
    public class CompareTests : BaseTestClass {

        List<SquaddingAssignmentFiringPoint> SquaddingAssignmentFPList = new List<SquaddingAssignmentFiringPoint>();
        string JONES_CHRIS_R1_FP1 = "Jones, Chris";
        string JONES_MONIKA_R1_FP2 = "Jones, Monika";
        string SMITH_DEREK_R2_FP1 = "Smith, Derek";
        string SMITH_JANET_R2_FP2 = "Smith, Janet";

        [TestInitialize]
        public override void InitializeTest() {
            base.InitializeTest();

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

        [TestMethod]
        public async Task CompareResultProjectedRanksListTests()
        {
            var comparer = new CompareResultByRank(CompareResultByRank.CompareMethod.PROJECTED_RANK_ORDER, Scopos.BabelFish.Helpers.SortBy.ASCENDING);
            var resultEvent1 = new ResultEvent
            {
                ProjectedRank = 1,
                Rank = 1
            };
            var resultEvent2 = new ResultEvent
            {
                ProjectedRank = 2,
                Rank = 2
            };
            var resultEvent3 = new ResultEvent
            {
                ProjectedRank = 0,
                Rank = 3
            };
            var resultEvent4 = new ResultEvent
            {
                ProjectedRank = 0,
                Rank = 0
            };

            Assert.IsTrue(comparer.Compare(resultEvent1, resultEvent2) < 0 ); // -1 = 1 compareTo 2
            Assert.IsTrue(comparer.Compare(resultEvent2, resultEvent3) < 0 ); // -1 = 2 compareTo 3 #3 is missing a Projected ranks, but it will use the Rank to place them then.
            Assert.IsTrue(comparer.Compare(resultEvent3, resultEvent4) > 0 ); // 1 = 3 compareTo 0 since RE4 does not have anything set. I am not sure how else to write that...
            Assert.IsTrue(comparer.Compare(resultEvent4, resultEvent1) < 0 ); // -1 = 0 compareTo 1 #4 again has nothing set, I don't think anything will let that happen, but it basically puts them at the bottom.

            Assert.IsTrue(comparer.Compare(resultEvent2, resultEvent1) > 0);  // 1 = 2 compareTo 1
            Assert.IsTrue(comparer.Compare(resultEvent1, resultEvent1) == 0); // 0 = 1 compareTo 1
        }
    }
}
