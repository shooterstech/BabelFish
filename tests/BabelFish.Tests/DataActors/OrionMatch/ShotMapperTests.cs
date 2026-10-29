using System.IO;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.OrionMatch {

    [TestClass]
    public class ShotMapperTests : BaseTestClass {

        /// <summary>
        /// Tests that the ShotMapper correctly receives shots and can return them  via the GetShots() method,
        /// based on the ResultCofId of the CourseOfFireEntryIndividual. Then tests that the shots are correctly
        /// serialized and deserialized when the MatchProject is saved to file and loaded back from file.
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
            shotMapper.InMemoryOnly = false;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            //ShotMapper should have no shots when the MatchProject is created.
            //And more to the point, a random guid should return an empty list of shots.
            var randomShotList = await shotMapper.GetShotsBySequenceAsync( Guid.NewGuid().ToString() );
            Assert.AreEqual( 0, randomShotList.Count );

            Shot k1, k2, k3;
            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (!participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                Assert.Fail( $"Failed to get CourseOfFireEntryIndividual for participant {participant.Participant.DisplayName} and CourseOfFire {cofStructure.CourseOfFireId}" );
            }
            invEntry = (CourseOfFireEntryIndividual)entry;

            k1 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 1 );
            k2 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 2 );
            k3 = await Shot.SimulateAsync( cofStructure, invEntry, "Kneeling", 3 );

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

            // Serialize then deserialize the MatchProject, and then verify that the shots are still there and correct after deserialization.
            project.SaveToFile();

            var deserializedProject = await MatchProject.LoadFromFileAsync( Path.Combine( project.ProjectDirectory.ToString(), project.GetFileName() ) );
            var deserializedShotMapper = deserializedProject.ShotMapper;
            var deserializedInvEntry = (CourseOfFireEntryIndividual)deserializedProject.Participants[0].Entries[0];

            var deserializedShotListBySequence = await deserializedShotMapper.GetShotsBySequenceAsync( deserializedInvEntry.ResultCofId );
            Assert.AreEqual( 3, deserializedShotListBySequence.Count );

            // Tests that the sequence values are the same ones we used when we generated the shots.
            Assert.AreEqual( k1.Sequence, deserializedShotListBySequence["1"].Sequence );
            Assert.AreEqual( k2.Sequence, deserializedShotListBySequence["2"].Sequence );
            Assert.AreEqual( k3.Sequence, deserializedShotListBySequence["3"].Sequence );

            // Tests that the scores are the same.
            Assert.AreEqual( k1.Score.D, deserializedShotListBySequence["1"].Score.D );
            Assert.AreEqual( k2.Score.D, deserializedShotListBySequence["2"].Score.D );
            Assert.AreEqual( k3.Score.D, deserializedShotListBySequence["3"].Score.D );

            // Tests that the event names got mapped to the expected values.
            Assert.AreEqual( "K1", deserializedShotListBySequence["1"].EventName );
            Assert.AreEqual( "K2", deserializedShotListBySequence["2"].EventName );
            Assert.AreEqual( "K3", deserializedShotListBySequence["3"].EventName );

            // Tests that the GetLastShot() method returns the last shot.
            var deserializedLastShot = deserializedShotMapper.GetLastShot( invEntry.ResultCofId );
            Assert.IsNotNull( deserializedLastShot );
            Assert.AreEqual( k3.Score.D, deserializedLastShot.Score.D );

            var deserializedShotListByEventName = await deserializedShotMapper.GetShotsByEventNameAsync( invEntry.ResultCofId );
            Assert.AreEqual( k1.Score.D, deserializedShotListByEventName["K1"].Score.D );
            Assert.AreEqual( k2.Score.D, deserializedShotListByEventName["K2"].Score.D );
            Assert.AreEqual( k3.Score.D, deserializedShotListByEventName["K3"].Score.D );
            Assert.IsFalse( deserializedShotListByEventName.ContainsKey( "K4" ) );
            Assert.IsFalse( deserializedShotListByEventName.ContainsKey( "P1" ) );
            Assert.IsFalse( deserializedShotListByEventName.ContainsKey( "S1" ) );
        }

        /// <summary>
        /// Tests that ShotMapper correctly calculates the Scores for each event, using a commong COF tree, namely the 3x10 air rifle.
        /// </summary>
        /// <returns></returns>
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
            shotMapper.InMemoryOnly = false;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            // This first if block tests the basically happy path for EST Scored shots. Sending ShotMapper one shot at a time using the ReceiveShot() method,
            // and then verifying that the scores for each event are being calculated correctly

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;
                Console.WriteLine( $"ResultCofId for {participant.Participant.DisplayName}: {invEntry.ResultCofId}" );

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
                    Assert.AreEqual( stage.GetAllSingulars().Count, eventScores[stage.EventName].NumShotsFired );
                    Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores[stage.EventName].Status );

                    if (stage.EventName == "Kneeling")
                        Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Kneeling" ), eventScores[stage.EventName].StageStyleDef );
                    else if (stage.EventName == "Prone")
                        Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Prone" ), eventScores[stage.EventName].StageStyleDef );
                    else if (stage.EventName == "Standing")
                        Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" ), eventScores[stage.EventName].StageStyleDef );
                }

                Assert.IsTrue( eventScores.ContainsKey( topLevelEvent.EventName ) );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - eventScores[topLevelEvent.EventName].Score.D ) < 0.0001 );
                Assert.AreEqual( topLevelEvent.GetAllSingulars().Count, eventScores[topLevelEvent.EventName].NumShotsFired );
                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores[topLevelEvent.EventName].Status );
                Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" ), eventScores[topLevelEvent.EventName].EventStyleDef );
                Console.WriteLine( $"Total Score for {participant.Participant.DisplayName}: {eventScores[topLevelEvent.EventName].Score.D}" );
            }


            // The second if block tests the happy path for Externally Scored shots. Sending ShotMapper a list of shots at once using the ReceiveExternallyScoredShots() method.

            var participantJane = await project.CreateMatchParticipantAsync( "Smith", "Jane" );
            if (participantJane.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entryJane )) {
                invEntry = (CourseOfFireEntryIndividual)entryJane;
                Console.WriteLine( $"ResultCofId for {participantJane.Participant.DisplayName}: {invEntry.ResultCofId}" );

                // Create a dictionary to keep track of the scores for each event, which we will use to verify that the scores are being calculated correctly.
                Dictionary<string, float> expectedSoreOfEvents = new Dictionary<string, float>();
                expectedSoreOfEvents[topLevelEvent.EventName] = 0;

                // Simulate shots for all stages and events in the course of fire, and send them to the ShotMapper.
                var sequence = 1;
                List<Shot> shotsOnPaper = new List<Shot>();
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    var numberOfShots = stage.GetAllSingulars().Count;
                    expectedSoreOfEvents[stage.EventName] = 0;

                    for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                        var shot = await Shot.SimulateAsync( cofStructure, invEntry, stage.EventName, sequence++ );
                        shotsOnPaper.Add( shot );
                        expectedSoreOfEvents[stage.EventName] += shot.Score.D;
                        expectedSoreOfEvents[topLevelEvent.EventName] += shot.Score.D;
                    }
                }

                shotMapper.ReceiveExternallyScoredShots( invEntry.ResultCofId, shotsOnPaper );

                // Now verify that the scores for each event are being calculated correctly by the ShotMapper.
                var eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    Assert.IsTrue( eventScores.ContainsKey( stage.EventName ) );
                    Assert.IsTrue( Math.Abs( expectedSoreOfEvents[stage.EventName] - eventScores[stage.EventName].Score.D ) < 0.0001 );
                    Assert.AreEqual( stage.GetAllSingulars().Count, eventScores[stage.EventName].NumShotsFired );
                    Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores[stage.EventName].Status );

                    if (stage.EventName == "Kneeling")
                        Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Kneeling" ), eventScores[stage.EventName].StageStyleDef );
                    else if (stage.EventName == "Prone")
                        Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Prone" ), eventScores[stage.EventName].StageStyleDef );
                    else if (stage.EventName == "Standing")
                        Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" ), eventScores[stage.EventName].StageStyleDef );
                }

                Assert.IsTrue( eventScores.ContainsKey( topLevelEvent.EventName ) );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - eventScores[topLevelEvent.EventName].Score.D ) < 0.0001 );
                Assert.AreEqual( topLevelEvent.GetAllSingulars().Count, eventScores[topLevelEvent.EventName].NumShotsFired );
                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores[topLevelEvent.EventName].Status );
                Assert.AreEqual( SetName.Parse( "v1.0:ntparc:Three-Position Sporter Air Rifle" ), eventScores[topLevelEvent.EventName].EventStyleDef );
                Console.WriteLine( $"Total Score for {participantJane.Participant.DisplayName}: {eventScores[topLevelEvent.EventName].Score.D}" );
            }
        }


        /// <summary>
        /// This test is similiar to the above EventScoreCalculationTests, but it tests with a COF that defines how the special sum ("S")
        /// score is calculated from two different score types ("I" and "D"). This is to test that the ShotMapper correctly calculates the "S"
        /// score based on the "I" and "D" scores of the shots, according to the calculation defined in the COF.
        /// </summary>
        /// <returns></returns>
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
                    Assert.AreEqual( stage.GetAllSingulars().Count, eventScores[stage.EventName].NumShotsFired );
                    Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores[stage.EventName].Status );
                    if (stage.EventName == "Standing") {
                        Assert.IsTrue( Math.Abs( expectedSoreOfEvents[stage.EventName] - eventScores[stage.EventName].Score.I ) < 0.0001 );
                    } else {
                        // Final
                        Assert.IsTrue( Math.Abs( expectedSoreOfEvents[stage.EventName] - eventScores[stage.EventName].Score.D ) < 0.0001 );
                    }
                }

                Assert.IsTrue( eventScores.ContainsKey( topLevelEvent.EventName ) );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - eventScores[topLevelEvent.EventName].Score.S ) < 0.0001 );
                Assert.AreEqual( topLevelEvent.GetAllSingulars().Count, eventScores[topLevelEvent.EventName].NumShotsFired );
                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores[topLevelEvent.EventName].Status );
            }
        }


        /// <summary>
        /// Tests that as a shots are fired within a COF, that the ShotMapper correctly updates the ResultStatus of the events in the COF,
        /// based on the number of shots fired and the official status of the COF. Also tests that a DSQ remark on the entry causes all events to be UNOFFICIAL
        /// and that the scores go to zero.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ResultStatusCalculationTests() {
            var matchName = "ResultStatusCalculationTests";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v1.0:ntparc:40 Shot Standing" ) );
            cofStructure.ScoreConfigName = "Integer";
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                // Simulate shots in groups of five. A bit tedious, but we'll check the ResultStatus after each group of five shots to make sure it's being updated correctly. 
                var group = 0;
                var numberOfShots = 40;

                // After 0 shots.
                var eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );
                Assert.AreEqual( ResultStatus.FUTURE, eventScores["Standing"].Status );
                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 1"].Status );
                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 2"].Status );
                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 3"].Status );
                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );

                for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                    var shot = await Shot.SimulateAsync( cofStructure, invEntry, "Standing", shotNum );
                    shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );

                    if (shotNum % 5 == 0) {
                        group = shotNum / 5;
                        eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );

                        switch (group) {
                            case 1: // After 5 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );
                                break;

                            case 2: // After 10 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );
                                break;

                            case 3: // After 15 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );
                                break;

                            case 4: // After 20 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );
                                break;

                            case 5: // After 25 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );
                                break;

                            case 6: // After 30 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.FUTURE, eventScores["ST 4"].Status );
                                break;

                            case 7: // After 35 shots.
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.INTERMEDIATE, eventScores["ST 4"].Status );

                                //Do a seperate test for the impact of a DSQ remark on the ResultStatus of the events. The DSQ remark should cause all events to be UNOFFICIAL, even if they were previously INTERMEDIATE or OFFICIAL.
                                entry.RemarkList.AddShowParticipantRemark( ParticipantRemark.DSQ, "Testing DSQ Remark Status", 0 );
                                eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 4"].Status );
                                //All scores should also be zero.
                                Assert.IsTrue( eventScores["Standing"].Score.IsZero );
                                Assert.IsTrue( eventScores["ST 1"].Score.IsZero );
                                Assert.IsTrue( eventScores["ST 2"].Score.IsZero );
                                Assert.IsTrue( eventScores["ST 3"].Score.IsZero );
                                Assert.IsTrue( eventScores["ST 4"].Score.IsZero );
                                //Remove the DSQ remark so we can continue with the test.
                                entry.RemarkList.Clear();
                                break;

                            case 8: // After 40 shots.
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["Standing"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 1"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 2"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 3"].Status );
                                Assert.AreEqual( ResultStatus.UNOFFICIAL, eventScores["ST 4"].Status );
                                break;
                        }
                    }
                }

                // After 40 shots, and marking the COF Structure as official.
                cofStructure.Official = true;
                eventScores = await shotMapper.GetEventScoresAsync( invEntry.ResultCofId );
                Assert.AreEqual( ResultStatus.OFFICIAL, eventScores["Standing"].Status );
                Assert.AreEqual( ResultStatus.OFFICIAL, eventScores["ST 1"].Status );
                Assert.AreEqual( ResultStatus.OFFICIAL, eventScores["ST 2"].Status );
                Assert.AreEqual( ResultStatus.OFFICIAL, eventScores["ST 3"].Status );
                Assert.AreEqual( ResultStatus.OFFICIAL, eventScores["ST 4"].Status );
            }
        }

        [TestMethod]
        public async Task SightersInShotMapperTest() {
            var matchName = "SightersInShotMapperTest";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v1.0:ntparc:40 Shot Standing" ) );
            cofStructure.ScoreConfigName = "Integer";
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                var numberOfShots = 40;
                var shotDictionary = await shotMapper.GetShotsBySequenceAsync( invEntry.ResultCofId, true );
                Assert.AreEqual( 0, shotDictionary.Count );

                //Simulate 5 sighter shots.
                for (int shotNum = 1; shotNum <= 5; shotNum++) {
                    var shot = await Shot.SimulateAsync( cofStructure, invEntry, "Standing", shotNum );
                    shot.AddAttribute( Shot.SHOT_ATTRIBUTE_SIGHTER );
                    shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                }

                shotDictionary = await shotMapper.GetShotsBySequenceAsync( invEntry.ResultCofId, true );
                Assert.AreEqual( 5, shotDictionary.Count );

                shotDictionary = await shotMapper.GetShotsBySequenceAsync( invEntry.ResultCofId, false );
                Assert.AreEqual( 0, shotDictionary.Count );

                var lastShot = shotMapper.GetLastShot( invEntry.ResultCofId, true );
                Assert.IsTrue( lastShot.IsASighter );

                //Simulate 40 record shots.
                for (int shotNum = 6; shotNum <= 45; shotNum++) {
                    var shot = await Shot.SimulateAsync( cofStructure, invEntry, "Standing", shotNum );
                    shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                }

                shotDictionary = await shotMapper.GetShotsBySequenceAsync( invEntry.ResultCofId, true );
                Assert.AreEqual( 45, shotDictionary.Count );

                shotDictionary = await shotMapper.GetShotsBySequenceAsync( invEntry.ResultCofId, false );
                Assert.AreEqual( 40, shotDictionary.Count );

                lastShot = shotMapper.GetLastShot( invEntry.ResultCofId, true );
                Assert.IsFalse( lastShot.IsASighter );
            }
        }
    }
}
