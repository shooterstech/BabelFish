using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests {
    [TestClass]
    public class ParticipantDataModelTests : BaseTestClass {

        [TestMethod]
        public void DefaultDisplayNameForIndividuals() {
            var individual = new Individual {
                GivenName = "John",
                FamilyName = "Smith"
            };

            //Displayname and DisplayNameShort are the same since its under 12 chracters.
            Assert.AreEqual( "Smith, John", individual.DisplayName );
            Assert.AreEqual( "Smith, John", individual.GetDisplayNameShort() );
            Assert.IsTrue( individual.DefaultDisplayName );

            individual = new Individual {
                GivenName = "Johnathanwellingtoneliziah",
                FamilyName = "Smith"
            };

            //DisplayNameShort are truncated since the full name exceeds 12 characters.
            Assert.AreEqual( "Smith, Johnathanwellingtoneliziah", individual.DisplayName );
            Assert.AreEqual( "Smith J", individual.GetDisplayNameShort() );
            Assert.IsTrue( individual.DefaultDisplayName );

            individual = new Individual {
                GivenName = "Johnathan"
            };

            //Displayname and DisplayNameShort are the same since onlyh the first name was given and its under 12 chracters.
            Assert.AreEqual( "Johnathan", individual.DisplayName );
            Assert.AreEqual( "Johnathan", individual.GetDisplayNameShort() );
            Assert.IsTrue( individual.DefaultDisplayName );

            individual = new Individual {
                GivenName = "Johnathanwellingtoneliziah"
            };

            //Displayname and DisplayNameShort are the same since onlyh the first name was given and its under 12 chracters.
            Assert.AreEqual( "Johnathanwellingtoneliziah", individual.DisplayName );
            Assert.AreEqual( "Johnathanwellingt...", individual.GetDisplayNameShort() );
            Assert.IsTrue( individual.DefaultDisplayName );

            individual = new Individual {
                FamilyName = "Smith"
            };

            //Displayname and DisplayNameShort are the same since onlyh the last name was given and its under 12 chracters.
            Assert.AreEqual( "Smith", individual.DisplayName );
            Assert.AreEqual( "Smith", individual.GetDisplayNameShort() );
            Assert.IsTrue( individual.DefaultDisplayName );

            individual = new Individual {
                FamilyName = "Smithwellingtoneliziah"
            };

            //Displayname and DisplayNameShort are the same since onlyh the last name was given and its under 12 chracters.
            Assert.AreEqual( "Smithwellingtoneliziah", individual.DisplayName );
            Assert.AreEqual( "Smithwellingtonel...", individual.GetDisplayNameShort() );
            Assert.IsTrue( individual.DefaultDisplayName );
        }

        [TestMethod]
        public void DefaultDisplayNameForTeam() {
            var team = new Team {
                TeamName = "Team A"
            };
            //Displayname and DisplayNameShort are the same since its under 20 chracters.
            Assert.AreEqual( "Team A", team.DisplayName );
            Assert.AreEqual( "Team A", team.GetDisplayNameShort() );
            Assert.IsTrue( team.DefaultDisplayName );

            team = new Team {
                TeamName = "Team Alpha"
            };

            //Displayname and DisplayNameShort are the same since its under 20 chracters.
            Assert.AreEqual( "Team Alpha", team.DisplayName );
            Assert.AreEqual( "Team Alpha", team.GetDisplayNameShort() );
            Assert.IsTrue( team.DefaultDisplayName );

            team = new Team {
                TeamName = "Team Alpha Bravo Charlie"
            };

            //Displayname and DisplayNameShort are the same since its under 20 chracters.
            Assert.AreEqual( "Team Alpha Bravo Charlie", team.DisplayName );
            Assert.AreEqual( "Team Alpha Bravo...", team.GetDisplayNameShort() );
            Assert.IsTrue( team.DefaultDisplayName );

            //Changing the DisplayName should not change the TeamName, but should change the DisplayName and DisplayNameShort and set DefaultDisplayName to false.
            team.DisplayName = "Team ABC";
            Assert.AreEqual( "Team ABC", team.DisplayName );
            Assert.AreEqual( "Team Alpha Bravo Charlie", team.TeamName );
            Assert.AreEqual( "Team ABC", team.GetDisplayNameShort() );
            Assert.IsFalse( team.DefaultDisplayName );
        }

        [TestMethod]
        public async Task GetAttributeValueTests() {

            var matchName = "GetAttributeValueTests";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            ClearDirectory( project.ProjectDirectory.FullName );
            var match = project.Match;

            //Add a CourseOfFireStructure into the Match. 
            var cof = await match.MatchStructure.AddCourseOfFireAsync( SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ) );
            var cofId = cof.CourseOfFireId;
            // The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type), and should be added automatically when the CourseOfFireStructure is added to the Match.
            var airRifleTypeAttrConfig = cof.Attributes[0];
            var airRifleTypeSetName = airRifleTypeAttrConfig.AttributeDef;

            //Add a second attribute configuration to the CourseOfFireStructure for a new shooter attribute. 
            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            var newShooterAttrConfig = await cof.AddAttributeConfigurationAsync( newShooterSetName );

            //First check that the MatchParticipant got added with default values for the two attributes. 
            var mp = await project.CreateMatchParticipantAsync( "Doe", "John" );
            Assert.AreEqual( "Sporter", (await mp.Participant.GetAttributeValueAsync( airRifleTypeSetName, cofId )).AttributeValue.GetFieldValue() );
            Assert.AreEqual( "Old Shooter", (await mp.Participant.GetAttributeValueAsync( newShooterSetName, cofId )).AttributeValue.GetFieldValue() );

            //Change the values.
            (await mp.Participant.GetAttributeValueAsync( airRifleTypeSetName, cofId )).AttributeValue.SetFieldValue( "Precision" );
            (await mp.Participant.GetAttributeValueAsync( newShooterSetName, cofId )).AttributeValue.SetFieldValue( "New Shooter" );

            //Re-check with the new values.
            Assert.AreEqual( "Precision", (await mp.Participant.GetAttributeValueAsync( airRifleTypeSetName, cofId )).AttributeValue.GetFieldValue() );
            Assert.AreEqual( "New Shooter", (await mp.Participant.GetAttributeValueAsync( newShooterSetName, cofId )).AttributeValue.GetFieldValue() );

            //Mark the new shooter attribute value as being constant.
            newShooterAttrConfig.Constant = true;
            newShooterAttrConfig.AttributeValue.SetFieldValue( "Old Shooter" );

            //Now the particpant should have the constant value for the new shooter attribute, even if we try to set it to something else.
            Assert.AreEqual( "Old Shooter", (await mp.Participant.GetAttributeValueAsync( newShooterSetName, cofId )).AttributeValue.GetFieldValue() );

            //Lets add a Global Attribute to the mix.
            var ageCategorySetname = SetName.Parse( "v1.0:ntparc:Age Categories" );
            var ageCategoryAttrConfig = await match.MatchStructure.AddAttributeConfigurationAsync( ageCategorySetname );

            //The Participant should have a default value for the global attribute as well.
            Assert.AreEqual( "Age Group 1", (await mp.Participant.GetAttributeValueAsync( ageCategorySetname, 0 )).AttributeValue.GetFieldValue() );

            (await mp.Participant.GetAttributeValueAsync( ageCategorySetname, 0 )).AttributeValue.SetFieldValue( "Age Group 2" );
            Assert.AreEqual( "Age Group 2", (await mp.Participant.GetAttributeValueAsync( ageCategorySetname, 0 )).AttributeValue.GetFieldValue() );

            ageCategoryAttrConfig.Constant = true;
            ageCategoryAttrConfig.AttributeValue.SetFieldValue( "Age Group 3" );
            Assert.AreEqual( "Age Group 3", (await mp.Participant.GetAttributeValueAsync( ageCategorySetname, 0 )).AttributeValue.GetFieldValue() );

            project.SaveToFile();
        }
    }
}
