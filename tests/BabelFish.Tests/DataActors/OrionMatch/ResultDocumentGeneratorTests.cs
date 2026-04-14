using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.OrionMatch {

    [TestClass]
    public class ResultDocumentGeneratorTests : BaseTestClass {

        /// <summary>
        /// This is a basic unit test to verify that the ResultDocumentGenerator can generate a ResultCOF for a simple 3x10 Air Rifle course of fire, and that the
        /// scores are being calculated correctly based on the shots that are sent to the ShotMapper and included in the ResultCOF.
        /// This test also verifies that participant attributes are being correctly included in the ResultCOF.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GenerateBasicResultCOF() {

            var matchName = "GenerateBasicResultCOF";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ) );
            cofStructure.ScoreConfigName = "Decimal";
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );
            var airRifleSetName = cofStructure.Attributes[0].AttributeDef;

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            // Create a participant and set the air rifle type attribute to a value other than the default.
            var participant = await project.CreateMatchParticipantAsync( "Smith", "John" );
            var airRifleTypeAttributeValue = await participant.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId );
            airRifleTypeAttributeValue.AttributeValue.SetFieldValue( "Precision" );

            CourseOfFireEntryIndividual invEntry;
            if (participant.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                invEntry = (CourseOfFireEntryIndividual)entry;

                // Create a dictionary to keep track of the scores for each event, which we will use to verify that the scores are being calculated correctly.
                Dictionary<string, float> expectedSoreOfEvents = new Dictionary<string, float>();
                expectedSoreOfEvents[topLevelEvent.EventName] = 0;

                // Simulate shots for all stages and events in the course of fire, and send them to the ShotMapper.
                var sequence = 1;
                foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                    var numberOfShots = stage.GetAllSingulars().Count - 5;
                    expectedSoreOfEvents[stage.EventName] = 0;

                    for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                        var shot = await Shot.SimulateAsync( cofStructure, invEntry, stage.EventName, sequence++ );
                        shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                        expectedSoreOfEvents[stage.EventName] += shot.Score.D;
                        expectedSoreOfEvents[topLevelEvent.EventName] += shot.Score.D;
                    }
                }

                ResultCOF resultCof = await project.ResultGenerator.GenerateResultCOFAsync( invEntry.ResultCofId, "Unit Test" );
                Assert.IsNotNull( resultCof );

                Assert.AreEqual( participant.Participant.DisplayName, resultCof.Participant.DisplayName );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - resultCof.EventScores[topLevelEvent.EventName].Score.D ) < 0.001 );
                //Assert.AreEqual( 30, resultCof.Shots.Count );
                Assert.AreEqual( "Precision", resultCof.Participant.AttributeValues[0].AttributeValue.GetFieldValue() );

                resultCof.SaveToFile( project.MatchObjectDirectory );
            }
        }
    }
}
