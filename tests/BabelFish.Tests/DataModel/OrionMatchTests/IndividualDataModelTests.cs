using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests {
    [TestClass]
    public class IndividualDataModelTests : BaseTestClass {

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
    }
}
