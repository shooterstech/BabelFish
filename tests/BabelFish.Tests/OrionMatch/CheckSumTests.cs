using System.Linq;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace BabelFish.Tests.OrionMatch {

    [TestClass]
    public class CheckSumTests : BaseTestClass {

        [TestMethod]
        public void ShotCheckSumTest() {
            var shot1 = new Shot() {
                EventName = "S2",
                BulletDiameter = 4.5f,
                ScoringDiameter = 4.5f,
                Location = new Location() { X = 10, Y = 20 },
                Score = new Score() { I = 10, D = 10.4f, X = 1 },
                TargetSetName = "v1.0:issf:50m Rifle",
                FiringPoint = "Firing Point 1",
                Attributes = new List<string>() { "SIGHTER" },
                StageLabel = "Stage 1",
                Sequence = 1,
                TargetName = "Target 1",
                ResultCOFID = Guid.NewGuid().ToString(),
                MatchID = MatchID.Parse( "1.1.123456.1" )
            };
            var shot2 = shot1.Clone();

            //Without changing any properties, the checksums should be the same
            Assert.AreEqual( shot1.CalculateChecksum(), shot2.CalculateChecksum(), "Identical shots should have the same checksum." );
            Console.WriteLine( shot1.CalculateChecksum() );
            Console.WriteLine( shot2.CalculateChecksum() );

            shot2.EventName = "S3";
            Assert.AreNotEqual( shot1.CalculateChecksum(), shot2.CalculateChecksum(), "Shots with different EventNames should have different checksums." );
            Console.WriteLine( shot2.CalculateChecksum() );

            var shot3 = shot1.Clone();
            shot3.Score.I = 9;
            Assert.AreNotEqual( shot1.CalculateChecksum(), shot3.CalculateChecksum(), "Shots with different Scores should have different checksums." );
            Console.WriteLine( shot3.CalculateChecksum() );

            var shot4 = shot1.Clone();
            shot4.Location.X = 15;
            Assert.AreNotEqual( shot1.CalculateChecksum(), shot4.CalculateChecksum(), "Shots with different Locations should have different checksums." );
            Console.WriteLine( shot4.CalculateChecksum() );

            var shot5 = shot1.Clone();
            shot5.Attributes.Clear();
            Assert.AreNotEqual( shot1.CalculateChecksum(), shot5.CalculateChecksum(), "Shots with different Attributes should have different checksums." );
            Console.WriteLine( shot5.CalculateChecksum() );

            var shot6 = shot1.Clone();
            shot6.AddPenalty( new Penalty() { RuleNumber = "1.2.3", PenaltyPoints = 2 } );
            Assert.AreNotEqual( shot1.CalculateChecksum(), shot6.CalculateChecksum(), "Shots with different Penalties should have different checksums." );
            Console.Write( shot6.CalculateChecksum() );
        }

        [TestMethod]
        public async Task IndividualCheckSumTest() {
            var individual1 = new Individual() {
                CompetitorNumber = "123",
                GivenName = "John",
                MiddleName = "A",
                FamilyName = "Doe",
                DisplayName = "John A Doe",
                UserID = "user123",
                Club = "Shooting Club",
                Country = "USA",
                HomeTown = "Minden, NE"
            };

            var individual2 = individual1.Clone();
            Assert.AreEqual( individual1.CalculateChecksum(), individual2.CalculateChecksum(), "Identical individuals should have the same checksum." );
            Console.WriteLine( individual1.CalculateChecksum() );
            Console.WriteLine( individual2.CalculateChecksum() );

            individual2.GivenName = "Jane";
            Assert.AreNotEqual( individual1.CalculateChecksum(), individual2.CalculateChecksum(), "Individuals with different FirstNames should have different checksums." );
            Console.WriteLine( individual2.CalculateChecksum() );

            // Due to their nature, AttributeValues do not clone with the method Clone().
            // So we need to create a new AttributeValue and add it to the individual to test that changing an AttributeValue changes the checksum.
            var airRifleTypeSetName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            var threePositionAirRifleAttr = await DefinitionCache.GetAttributeDefinitionAsync( airRifleTypeSetName );
            var av = await AttributeValue.CreateAsync( airRifleTypeSetName );

            var individual3 = individual1.Clone();
            individual3.AttributeValues.Add( new AttributeValueDataPacketMatch() {
                AttributeDef = airRifleTypeSetName.ToString(),
                Visibility = Scopos.BabelFish.DataModel.Common.VisibilityOption.PUBLIC,
                AttributeValue = av
            } );
            Assert.AreNotEqual( individual1.CalculateChecksum(), individual3.CalculateChecksum(), "Individuals with different AttributeValues should have different checksums." );
            Console.WriteLine( individual3.CalculateChecksum() );
        }

        [TestMethod]
        public async Task ResultListCheckSumTests() {
            var client = new OrionMatchAPIClient();
            var resultListResponse = await client.GetResultListAsync( MatchID.Parse( "1.2413.2026050816212089.0" ), "Individual - Sporter" );
            var resultList = resultListResponse.ResultList;

            // Each ResultEvent in .Items should have a different CheckSum.
            HashSet<ulong> checksums = new HashSet<ulong>();
            foreach (var resultEvent in resultList.Items) {
                var checksum = resultEvent.CalculateChecksum();
                if (checksums.Contains( checksum )) {
                    Assert.Fail( $"Duplicate checksum found for ResultEvent with CompetitorNumber {resultEvent.Participant.CompetitorNumber}." );
                }
                checksums.Add( checksum );
            }

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            var currentCheckSumToCheckAgainst = resultList.CalculateChecksum();
            stopWatch.Stop();
            Console.WriteLine( currentCheckSumToCheckAgainst );
            Console.WriteLine( stopWatch.ElapsedMilliseconds );

            // Changing the CompetitorNumber of the first ResultEvent should change the checksum of the entire ResultList.
            resultList.Items[0].Participant.CompetitorNumber = "999";
            Assert.AreNotEqual( currentCheckSumToCheckAgainst, resultList.CalculateChecksum(), "Changing a ResultEvent's CompetitorNumber should change the ResultList checksum." );
            currentCheckSumToCheckAgainst = resultList.CalculateChecksum();
            Console.WriteLine( currentCheckSumToCheckAgainst );

            // Changing the Score of the first ResultEvent should change the checksum of the entire ResultList.
            resultList.Items[0].EventScores.First().Value.Score.I = 5;
            Assert.AreNotEqual( currentCheckSumToCheckAgainst, resultList.CalculateChecksum(), "Changing a ResultEvent's Score should change the ResultList checksum." );
            currentCheckSumToCheckAgainst = resultList.CalculateChecksum();
            Console.WriteLine( currentCheckSumToCheckAgainst );
        }
    }
}
