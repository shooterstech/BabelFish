using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.OrionMatch {

    [TestClass]
    public class ShotMapperTests : BaseTestClass {

        /// <summary>
        /// Tests that the ShotMapper correctly receives shots and can return them  via the GetShots() method, based on the ResultCofId of the CourseOfFireEntryIndividual.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ShotListTestsAsync() {
            var matchName = "ShotListTests";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ) );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            //ShotMapper should have no shots when the MatchProject is created.
            //And more to the point, a random guid should return an empty list of shots.
            var randomShotList = await shotMapper.GetShotsBySequenceAsync( Guid.NewGuid().ToString() );
            Assert.AreEqual( 0, randomShotList.Count );

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                var k1 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 1 );
                var k2 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 2 );
                var k3 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 3 );

                shotMapper.ReceiveShot( this, new EventArgs<Shot>( k1 ) );
                shotMapper.ReceiveShot( this, new EventArgs<Shot>( k2 ) );
                shotMapper.ReceiveShot( this, new EventArgs<Shot>( k3 ) );

                var shotListBySequence = await shotMapper.GetShotsBySequenceAsync( invEntry.ResultCofId );
                Assert.AreEqual( 3, shotListBySequence.Count );

                // Tests that the sequence values are the same ones we used when we generated the shots.
                Assert.AreEqual( k1.Sequence, shotListBySequence["1"].Sequence );
                Assert.AreEqual( k2.Sequence, shotListBySequence["2"].Sequence );
                Assert.AreEqual( k3.Sequence, shotListBySequence["3"].Sequence );

                // Tests that the scores are the same.
                Assert.AreEqual( k1.Score.D, shotListBySequence["1"].Score.D );
                Assert.AreEqual( k2.Score.D, shotListBySequence["2"].Score.D );
                Assert.AreEqual( k3.Score.D, shotListBySequence["3"].Score.D );

                // Tests that the event names got mapped to the expected values.
                Assert.AreEqual( "K1", shotListBySequence["1"].EventName );
                Assert.AreEqual( "K2", shotListBySequence["2"].EventName );
                Assert.AreEqual( "K3", shotListBySequence["3"].EventName );

                // Tests that the GetLastShot() method returns the last shot.
                var lastShot = shotMapper.GetLastShot( invEntry.ResultCofId );
                Assert.IsNotNull( lastShot );
                Assert.AreEqual( k3.Score.D, lastShot.Score.D );

                var shotListByEventName = await shotMapper.GetShotsByEventNameAsync( invEntry.ResultCofId );
                Assert.AreEqual( k1.Score.D, shotListByEventName["K1"].Score.D );
                Assert.AreEqual( k2.Score.D, shotListByEventName["K2"].Score.D );
                Assert.AreEqual( k3.Score.D, shotListByEventName["K3"].Score.D );
                Assert.IsFalse( shotListByEventName.ContainsKey( "K4" ) );
                Assert.IsFalse( shotListByEventName.ContainsKey( "P1" ) );
                Assert.IsFalse( shotListByEventName.ContainsKey( "S1" ) );
            } else {
                Assert.Fail( $"Failed to get CourseOfFireEntryIndividual for participant {participant.Participant.DisplayName} and CourseOfFire {cofStructure.CourseOfFireId}" );
            }
        }

        [TestMethod]
        public async Task EventScoreCalculationTests() {
            var matchName = "EventScoreCalculationTests";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ) );
            cofStructure.ScoreConfigName = "Decimal";
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            //ShotMapper should have no shots when the MatchProject is created.
            //And more to the point, a random guid should return an empty list of shots.
            var randomShotList = await shotMapper.GetShotsBySequenceAsync( Guid.NewGuid().ToString() );
            Assert.AreEqual( 0, randomShotList.Count );

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                // Create a dictionary to keep track of the scores for each event, which we will use to verify that the scores are being calculated correctly.
                Dictionary<string, float> expectedSoreOfEvents = new Dictionary<string, float>();
                expectedSoreOfEvents[topLevelEvent.EventName] = 0;

                // Simulate shots for all stages and events in the course of fire, and send them to the ShotMapper.
                var sequence = 1;
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    var numberOfShots = stage.GetAllSingulars().Count;
                    expectedSoreOfEvents[stage.EventName] = 0;

                    for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                        var shot = await Shot.SimulateAsync( cofStructure, invEntry, stage.EventName, sequence++ );
                        shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                        expectedSoreOfEvents[stage.EventName] += shot.Score.D;
                        expectedSoreOfEvents[topLevelEvent.EventName] += shot.Score.D;
                    }
                }

                // Now verify that the scores for each event are being calculated correctly by the ShotMapper.
                var eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    Assert.IsTrue( eventScores.ContainsKey( stage.EventName ) );
                    Assert.IsTrue( Math.Abs( expectedSoreOfEvents[stage.EventName] - eventScores[stage.EventName].Score.D ) < 0.0001 );
                }

                Assert.IsTrue( eventScores.ContainsKey( topLevelEvent.EventName ) );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - eventScores[topLevelEvent.EventName].Score.D ) < 0.0001 );
            }
        }

        [TestMethod]
        public async Task AccumulativeFinalCalculationTests() {
            var matchName = "AccumulativeFinalCalculationTests";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v2.0:ntparc:40 Shot Standing plus Final" ) );
            cofStructure.ScoreConfigName = "Integer";
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            //ShotMapper should have no shots when the MatchProject is created.
            //And more to the point, a random guid should return an empty list of shots.
            var randomShotList = await shotMapper.GetShotsBySequenceAsync( Guid.NewGuid().ToString() );
            Assert.AreEqual( 0, randomShotList.Count );

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                // Create a dictionary to keep track of the scores for each event, which we will use to verify that the scores are being calculated correctly.
                Dictionary<string, float> expectedSoreOfEvents = new Dictionary<string, float>();
                expectedSoreOfEvents[topLevelEvent.EventName] = 0;

                // Simulate shots for all stages and events in the course of fire, and send them to the ShotMapper.
                var sequence = 1;
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    var numberOfShots = stage.GetAllSingulars().Count;
                    expectedSoreOfEvents[stage.EventName] = 0;

                    for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                        var shot = await Shot.SimulateAsync( cofStructure, invEntry, stage.EventName, sequence++ );
                        shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                        if (stage.EventName == "Standing") {
                            expectedSoreOfEvents[stage.EventName] += shot.Score.I;
                            expectedSoreOfEvents[topLevelEvent.EventName] += shot.Score.I;
                        } else {
                            //Final
                            expectedSoreOfEvents[stage.EventName] += shot.Score.D;
                            expectedSoreOfEvents[topLevelEvent.EventName] += shot.Score.D;
                        }
                    }
                }

                // Now verify that the scores for each event are being calculated correctly by the ShotMapper.
                var eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    Assert.IsTrue( eventScores.ContainsKey( stage.EventName ) );
                    if (stage.EventName == "Standing") {
                        Assert.IsTrue( Math.Abs( expectedSoreOfEvents[stage.EventName] - eventScores[stage.EventName].Score.I ) < 0.0001 );
                    } else {
                        // Final
                        Assert.IsTrue( Math.Abs( expectedSoreOfEvents[stage.EventName] - eventScores[stage.EventName].Score.D ) < 0.0001 );
                    }
                }

                Assert.IsTrue( eventScores.ContainsKey( topLevelEvent.EventName ) );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - eventScores[topLevelEvent.EventName].Score.S ) < 0.0001 );
            }
        }
    }
}
