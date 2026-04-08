using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.OrionMatch {

    [TestClass]
    public class ShotMapperTests : BaseTestClass {

        [TestMethod]
        public async Task ShotListTestsAsync() {
            var matchName = "ShotListTests";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ) );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            //ShotMapper should have no shots when the MatchProject is created.
            //And more to the point, a random guid should return an empty list of shots.
            var randomShotList = shotMapper.GetShots( Guid.NewGuid().ToString() );
            Assert.AreEqual( 0, randomShotList.Count );

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                var kn1 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 1 );
                var kn2 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 2 );
                var kn3 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 3 );

                shotMapper.ReceiveShot( this, new EventArgs<Shot>( kn1 ) );
                shotMapper.ReceiveShot( this, new EventArgs<Shot>( kn2 ) );
                shotMapper.ReceiveShot( this, new EventArgs<Shot>( kn3 ) );

                var shotList = shotMapper.GetShots( invEntry.ResultCofId );
                Assert.AreEqual( 3, shotList.Count );

                Assert.AreEqual( kn1.Sequence, shotList["1"].Sequence );
                Assert.AreEqual( kn2.Sequence, shotList["2"].Sequence );
                Assert.AreEqual( kn3.Sequence, shotList["3"].Sequence );

                Assert.AreEqual( kn1.Score.D, shotList["1"].Score.D );
                Assert.AreEqual( kn2.Score.D, shotList["2"].Score.D );
                Assert.AreEqual( kn3.Score.D, shotList["3"].Score.D );
            } else {
                Assert.Fail( $"Failed to get CourseOfFireEntryIndividual for participant {participant.Participant.DisplayName} and CourseOfFire {cofStructure.CourseOfFireId}" );
            }
        }
    }
}
