using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.OrionMatch;
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
                    var numberOfShots = stage.GetAllSingulars().Count;
                    expectedSoreOfEvents[stage.EventName] = 0;

                    for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                        var shot = await Shot.SimulateAsync( cofStructure, invEntry, stage.EventName, sequence++ );
                        shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                        expectedSoreOfEvents[stage.EventName] += shot.Score.D;
                        expectedSoreOfEvents[topLevelEvent.EventName] += shot.Score.D;
                    }
                }

                ResultCOF resultCof = await project.ResultGenerator.GenerateResultCOFAsync( invEntry, "Unit Test" );
                Assert.IsNotNull( resultCof );

                Assert.AreEqual( participant.Participant.DisplayName, resultCof.Participant.DisplayName );
                Assert.IsTrue( Math.Abs( expectedSoreOfEvents[topLevelEvent.EventName] - resultCof.EventScores[topLevelEvent.EventName].Score.D ) < 0.001 );
                Assert.AreEqual( 30, resultCof.Shots.Count );
                Assert.AreEqual( "Precision", resultCof.Participant.AttributeValues[0].AttributeValue.GetFieldValue() );

                resultCof.SaveToFile( project.MatchObjectDirectory );
            }
        }

        [TestMethod]
        public async Task GenerateBasicResultList() {

            var matchName = "GenerateBasicResultList";

            //Create the MatchProject which will generate a ShotMapper.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            this.ClearDirectory( project.ProjectDirectory.FullName ); //Don't really need to call Clear Directory, as we are really not writing any files in this test, but just to be safe.

            var cofStructure = await project.Match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ) );
            cofStructure.ScoreConfigName = "Decimal";
            cofStructure.NumberOfTeamMembers = 2;
            cofStructure.MaxNumberOfTeamMembers = 100;
            var cofDefinition = await cofStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( cofDefinition );
            var airRifleSetName = cofStructure.Attributes[0].AttributeDef;

            ResultListWizard wizard = new ResultListWizard( project.Match );
            var resultLists = await wizard.GenerateAsync( cofStructure );
            cofStructure.AddResultList( resultLists );

            project.SaveToFile( project.ProjectDirectory );

            var shotMapper = project.ShotMapper;
            shotMapper.InMemoryOnly = true;

            //ShotMapper should be created when the MatchProject is created.
            Assert.IsNotNull( shotMapper );

            // Create a few participant and set the air rifle type attribute to different values.
            var participantJohn = await project.CreateMatchParticipantAsync( "Smith", "John" );
            (await participantJohn.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId )).AttributeValue.SetFieldValue( "Precision" );

            var participantJane = await project.CreateMatchParticipantAsync( "Smith", "Jane" );
            (await participantJane.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId )).AttributeValue.SetFieldValue( "Precision" );

            var participantMorgan = await project.CreateMatchParticipantAsync( "Smith", "Morgan" );
            (await participantMorgan.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId )).AttributeValue.SetFieldValue( "Sporter" );

            var participantKyle = await project.CreateMatchParticipantAsync( "Smith", "Kyle" );
            (await participantKyle.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId )).AttributeValue.SetFieldValue( "Sporter" );

            var participantEmily = await project.CreateMatchParticipantAsync( "Smith", "Emily" );
            (await participantEmily.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId )).AttributeValue.SetFieldValue( "Sporter" );

            var participantForrest = await project.CreateMatchParticipantAsync( "Smith", "Forrest" );
            (await participantForrest.Participant.GetAttributeValueAsync( airRifleSetName, cofStructure.CourseOfFireId )).AttributeValue.SetFieldValue( "Precision" );

            // Simulate shots for all stages and events in the course of fire, and send them to the ShotMapper.
            foreach (var mp in project.Participants) {
                CourseOfFireEntryIndividual invEntry;
                if (mp.TryGetEntryByCourseOfFireId( cofStructure.CourseOfFireId, out var entry )) {
                    invEntry = (CourseOfFireEntryIndividual)entry;
                    var sequence = 1;
                    foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
                        var numberOfShots = stage.GetAllSingulars().Count;
                        for (int shotNum = 1; shotNum <= numberOfShots; shotNum++) {
                            var shot = await Shot.SimulateAsync( cofStructure, invEntry, stage.EventName, sequence++ );
                            shotMapper.ReceiveShot( this, new EventArgs<Shot>( shot ) );
                        }
                    }
                }
            }

            /**** Individual Result List Testing ****/

            var invAllAbbr = resultLists.Find( rl => rl.ResultName == "Individual - All" );
            Assert.IsNotNull( invAllAbbr );

            var invAllResultList = await project.ResultGenerator.GenerateResultListAsync( invAllAbbr, string.Empty );
            Assert.AreEqual( 6, invAllResultList.Items.Count );
            invAllResultList.SaveToFile( project.MatchObjectDirectory );
            // Test that the ranking was done correctly
            Assert.IsTrue( invAllResultList.Items[0].EventScores[invAllResultList.EventName].Score.D >= invAllResultList.Items[1].EventScores[invAllResultList.EventName].Score.D );
            Assert.IsTrue( invAllResultList.Items[1].EventScores[invAllResultList.EventName].Score.D >= invAllResultList.Items[2].EventScores[invAllResultList.EventName].Score.D );
            Assert.IsTrue( invAllResultList.Items[2].EventScores[invAllResultList.EventName].Score.D >= invAllResultList.Items[3].EventScores[invAllResultList.EventName].Score.D );
            Assert.IsTrue( invAllResultList.Items[3].EventScores[invAllResultList.EventName].Score.D >= invAllResultList.Items[4].EventScores[invAllResultList.EventName].Score.D );
            Assert.IsTrue( invAllResultList.Items[4].EventScores[invAllResultList.EventName].Score.D >= invAllResultList.Items[5].EventScores[invAllResultList.EventName].Score.D );

            var invSporterAbbr = resultLists.Find( rl => rl.ResultName == "Individual - Sporter" );
            Assert.IsNotNull( invSporterAbbr );

            var invSporterResultList = await project.ResultGenerator.GenerateResultListAsync( invSporterAbbr, string.Empty );
            Assert.AreEqual( 3, invSporterResultList.Items.Count );
            invSporterResultList.SaveToFile( project.MatchObjectDirectory );

            var invPrecisionAbbr = resultLists.Find( rl => rl.ResultName == "Individual - Precision" );
            Assert.IsNotNull( invPrecisionAbbr );

            var invPrecisionResultList = await project.ResultGenerator.GenerateResultListAsync( invPrecisionAbbr, string.Empty );
            Assert.AreEqual( 3, invPrecisionResultList.Items.Count );
            invPrecisionResultList.SaveToFile( project.MatchObjectDirectory );

            /**** Team Result List Testing ****/
            var teamA = await project.CreateMatchParticipantAsync( "A Team" );
            ((Team)teamA.Participant).TeamMembers.Add( participantJohn.Participant );
            ((Team)teamA.Participant).TeamMembers.Add( participantJane.Participant );
            ((Team)teamA.Participant).TeamMembers.Add( participantForrest.Participant );

            var teamB = await project.CreateMatchParticipantAsync( "B Team" );
            ((Team)teamB.Participant).TeamMembers.Add( participantMorgan.Participant );
            ((Team)teamB.Participant).TeamMembers.Add( participantKyle.Participant );
            ((Team)teamB.Participant).TeamMembers.Add( participantEmily.Participant );

            var teamAll = resultLists.Find( rl => rl.ResultName == "Team - All" );
            Assert.IsNotNull( teamAll );
            var teamAllResultList = await project.ResultGenerator.GenerateResultListAsync( teamAll, string.Empty );
            Assert.AreEqual( 2, teamAllResultList.Items.Count );
            teamAllResultList.SaveToFile( project.MatchObjectDirectory );

            Assert.IsTrue( teamAllResultList.Items[0].EventScores[teamAllResultList.EventName].Score.D >= teamAllResultList.Items[1].EventScores[teamAllResultList.EventName].Score.D );
        }
    }
}
