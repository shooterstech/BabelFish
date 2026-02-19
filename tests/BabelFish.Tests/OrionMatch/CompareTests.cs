using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {

    
    [TestClass]
    public class CompareTests : BaseTestClass {

        List<Squadding> SquaddingAssignmentFPList = new List<Squadding>();
        string JONES_CHRIS_R1_FP1 = "Jones, Chris";
        string JONES_MONIKA_R1_FP2 = "Jones, Monika";
        string SMITH_DEREK_R2_FP1 = "Smith, Derek";
        string SMITH_JANET_R2_FP2 = "Smith, Janet";

        [TestInitialize]
        public override void InitializeTest() {
            base.InitializeTest();

            SquaddingAssignmentFPList = new List<Squadding>();

            SquaddingAssignmentFPList.Add(
                new Squadding() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Chris",
                        DisplayName = JONES_CHRIS_R1_FP1,
                        CompetitorNumber = "101"
                    },
                    SquaddingAssignment = new SquaddingAssignmentFiringPoint() {
                        FiringPoint = "1",
                        Relay = "1"
                    }
                }
                );

            SquaddingAssignmentFPList.Add(
                new Squadding() {
                    Participant = new Individual() {
                        LastName = "Jones",
                        GivenName = "Monika",
                        DisplayName = JONES_MONIKA_R1_FP2,
                        CompetitorNumber = "102"
                    },
					SquaddingAssignment = new SquaddingAssignmentFiringPoint() {
                        FiringPoint = "2",
                        Relay = "1"
                    }
                }
                );

            SquaddingAssignmentFPList.Add(
                new Squadding() {
                    Participant = new Individual() {
                        FamilyName = "Smith",
                        GivenName = "Derek",
                        DisplayName = SMITH_DEREK_R2_FP1,
                        CompetitorNumber = "103"
                    },
					SquaddingAssignment = new SquaddingAssignmentFiringPoint() {
                        FiringPoint = "1",
                        Relay = "2"
                    }
                }
                );

            SquaddingAssignmentFPList.Add(
                new Squadding() {
                    Participant = new Individual() {
                        FamilyName = "Smith",
                        GivenName = "Janet",
                        DisplayName = SMITH_JANET_R2_FP2,
                        CompetitorNumber = "104"
                    },
					SquaddingAssignment = new SquaddingAssignmentFiringPoint() {
                        FiringPoint = "2",
                        Relay = "2"
                    }
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

            var sortByRelayFiringPointAsc = new CompareSquadding( CompareSquadding.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFiringPointAsc );
            Assert.AreEqual(JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[3].Participant.DisplayName);

            var sortByRelayFiringPointDsc = new CompareSquadding( CompareSquadding.CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.DESCENDING );
            SquaddingAssignmentFPList.Sort( sortByRelayFiringPointDsc );
            Assert.AreEqual(SMITH_JANET_R2_FP2, SquaddingAssignmentFPList[0].Participant.DisplayName);
            Assert.AreEqual(SMITH_DEREK_R2_FP1, SquaddingAssignmentFPList[1].Participant.DisplayName);
            Assert.AreEqual(JONES_MONIKA_R1_FP2, SquaddingAssignmentFPList[2].Participant.DisplayName);
            Assert.AreEqual(JONES_CHRIS_R1_FP1, SquaddingAssignmentFPList[3].Participant.DisplayName);

            var sortByRelayFirstLastAsc = new CompareSquadding( CompareSquadding.CompareMethod.RELAY_DISPLAYNAME, Scopos.BabelFish.Helpers.SortBy.ASCENDING );
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

        [TestMethod]
        public async Task CompareMatchAbbrTests() {
            var comparer_1 = new CompareMatchAbbr(CompareMatchAbbr.CompareMethod.DISTANCE, SortBy.DESCENDING);

            Scopos.BabelFish.DataModel.Common.Location matchLocation_1 = new Location();
            MatchID matchID_1 = new MatchID("1.15.2026021816010937.0");
            matchLocation_1.Distance = 1;
            var matchAbbr_1 = new MatchAbbr {
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(-14),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                OwnerId = "OrionAccount000015",
                MatchID = matchID_1,
                MatchName = "Match1 with I Dunno.",
                Location = matchLocation_1
            };
            Scopos.BabelFish.DataModel.Common.Location matchLocation_2 = new Location();
            MatchID matchID_2 = new MatchID("1.16.2026021816010937.0");
            matchLocation_2.Distance = 2;
            var matchAbbr_2 = new MatchAbbr {
                StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(-15),
                EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(-1),
                OwnerId = "OrionAccount000016",
                MatchID = matchID_2,
                MatchName = "Match2 with extra stuff??",
                Location = matchLocation_2
            };

            Assert.IsTrue(comparer_1.Compare(matchAbbr_1, matchAbbr_2) > 0); //Compare distances, 1 < 2

            var comparer_2 = new CompareMatchAbbr(CompareMatchAbbr.CompareMethod.START_DATE, SortBy.DESCENDING);
            Assert.IsTrue(comparer_2.Compare(matchAbbr_1, matchAbbr_2) < 0); //Compare StartDates, 1 > 2

            var comparer_3 = new CompareMatchAbbr(CompareMatchAbbr.CompareMethod.END_DATE, SortBy.DESCENDING);
            Assert.IsTrue(comparer_3.Compare(matchAbbr_1, matchAbbr_2) < 0); //Compare EndDates, 1 > 2

            var comparer_4 = new CompareMatchAbbr(CompareMatchAbbr.CompareMethod.OWNER_ID, SortBy.DESCENDING);
            Assert.IsTrue(comparer_4.Compare(matchAbbr_1, matchAbbr_2) > 0); //Compare ownerID, 1 < 2

            var comparer_5 = new CompareMatchAbbr(CompareMatchAbbr.CompareMethod.MATCH_ID, SortBy.DESCENDING);
            Assert.IsTrue(comparer_5.Compare(matchAbbr_1, matchAbbr_2) > 0); //Compare EndDates, 1 < 2

            var comparer_6 = new CompareMatchAbbr(CompareMatchAbbr.CompareMethod.MATCH_NAME, SortBy.DESCENDING);
            Assert.IsTrue(comparer_6.Compare(matchAbbr_1, matchAbbr_2) > 0); //Compare EndDates, 1 < 2
        }
    }
    
}
