using System.IO;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests {
    [TestClass]
    public class SerializationTests : BaseTestClass {

        /// <summary>
        /// Tests that the ISaveToFile interface methods are return the expected file names and paths, and that the file is actually created when saving.
        /// This test does not verify the contents of the file, only that it is created with the correct name and path.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task MatchFileNameTests() {
            var matchName = "MatchFileNameTests";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            var match = project.Match;
            ClearDirectory( project.ProjectDirectory.FullName );


            Assert.AreEqual( $"{matchName}.json", match.GetFileName() );
            Assert.AreEqual( $"{matchName}.json", match.GetRelativePath() );

            var expectedFullFileName = Path.Combine( BaseTestClass.RelativeDirectoryForTesting.FullName, matchName, $"{matchName}.json" );
            var fullFileName = match.SaveToFile( project.ProjectDirectory );
            Assert.AreEqual( expectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );
        }

        [TestMethod]
        public async Task MatchSerializaeDeserializeTests() {
            var matchName = "MatchSerializaeDeserializeTests";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            ClearDirectory( project.ProjectDirectory.FullName );
            var match = project.Match;

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            CourseOfFireStructure cof, deserializedCof;
            match.MatchStructure.TryGetCourseOfFireStructure( cofId, out cof );
            Assert.IsNotNull( cof );
            cof.AddAttributeConfigurationAsync( newShooterSetName );

            //Let the Wizard do it's thing
            ResultListWizard wizard = new ResultListWizard( match );
            var resultLists = await wizard.GenerateAsync( cofId );

            //Add all the result lists so we have plenty to serialize and deserialize
            foreach (var resultList in resultLists)
                cof.AddResultList( resultList );

            var fullFileName = match.SaveToFile( project.ProjectDirectory );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist" );

            var deserializedMatch = await Match.LoadFromFileAsync( fullFileName );

            Assert.IsNotNull( deserializedMatch );
            Assert.AreEqual( match.Name, deserializedMatch.Name );
            Assert.AreEqual( match.Visibility, deserializedMatch.Visibility );
            Assert.AreEqual( match.MatchType, deserializedMatch.MatchType );
            Assert.AreEqual( match.StartDate, deserializedMatch.StartDate );
            Assert.AreEqual( match.EndDate, deserializedMatch.EndDate );

            deserializedMatch.MatchStructure.TryGetCourseOfFireStructure( cofId, out deserializedCof );
            Assert.IsNotNull( deserializedCof );

            for (int i = 0; i < cof.ResultLists.Count; i++) {
                var resultList = cof.ResultLists[i];
                var deserializedResultList = deserializedCof.ResultLists[i];

                Console.WriteLine( $"Comparing ResultList: {resultList.ResultName}" );
                Assert.AreEqual( resultList.ResultName, deserializedResultList.ResultName );
                Assert.AreEqual( resultList.AttributeFilter.Count, deserializedResultList.AttributeFilter.Count );

                Assert.AreEqual( resultList.GetHashCode(), deserializedResultList.GetHashCode() );
            }
        }

        /// <summary>
        /// Tests that the ISaveToFile interface methods are return the expected file names and paths, and that the file is actually created when saving.
        /// This test does not verify the contents of the file, only that it is created with the correct name and path.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CourseOfFireEntryFileNameTests() {
            var matchName = "CourseOfFireEntryFileNameTest";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            var match = project.Match;
            ClearDirectory( project.ProjectDirectory.FullName );

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

            //Add a second Attribute for good measure.
            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            CourseOfFireStructure cof, deserializedCof;
            match.MatchStructure.TryGetCourseOfFireStructure( cofId, out cof );
            Assert.IsNotNull( cof );
            cof.AddAttributeConfigurationAsync( newShooterSetName );

            var johnSmith = await project.CreateMatchParticipantAsync( "Smith", "John" );
            var janeDoe = await project.CreateMatchParticipantAsync( "Doe", "Jane" );
            var aTeam = await project.CreateMatchParticipantAsync( "Team A" );

            var johnSmithExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, matchName, MatchParticipant.FOLDER_NAME, $"{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json" );
            var janeDoeExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, matchName, MatchParticipant.FOLDER_NAME, $"{janeDoe.Participant.DisplayName} {janeDoe.ParticipantID}.json" );
            var aTeamExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, matchName, MatchParticipant.FOLDER_NAME, $"{aTeam.Participant.DisplayName} {aTeam.ParticipantID}.json" );


            Assert.AreEqual( $"{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json", johnSmith.GetFileName() );
            Assert.AreEqual( $"{MatchParticipant.FOLDER_NAME}\\{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json", johnSmith.GetRelativePath() );

            var fullFileName = johnSmith.SaveToFile();
            Assert.AreEqual( johnSmithExpectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );

            fullFileName = janeDoe.SaveToFile();
            Assert.AreEqual( janeDoeExpectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );

            fullFileName = aTeam.SaveToFile();
            Assert.AreEqual( aTeamExpectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );

            //Update the display name for an individual and verify that the file name changes accordingly
            johnSmith.Participant.DisplayName = "Johnathan Smith";
            var johnSmithUpdatedExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, matchName, MatchParticipant.FOLDER_NAME, $"{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json" );
            Assert.IsTrue( File.Exists( johnSmithUpdatedExpectedFullFileName ), $"File does not exist: {johnSmithUpdatedExpectedFullFileName}" );

            aTeam.Participant.DisplayName = "Team Alpha";
            var aTeamUpdatedExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, matchName, MatchParticipant.FOLDER_NAME, $"{aTeam.Participant.DisplayName} {aTeam.ParticipantID}.json" );
            Assert.IsTrue( File.Exists( aTeamUpdatedExpectedFullFileName ), $"File does not exist: {aTeamUpdatedExpectedFullFileName}" );

            //Now try and deserialize one of the individual files.
            var newJohnSmith = await MatchParticipant.LoadFromFileAsync( johnSmithUpdatedExpectedFullFileName );
            Assert.AreEqual( johnSmith.Participant.DisplayName, newJohnSmith.Participant.DisplayName );
            Assert.AreEqual( johnSmith.ParticipantID, newJohnSmith.ParticipantID );
            Assert.AreEqual( ((Individual)johnSmith.Participant).GivenName, ((Individual)newJohnSmith.Participant).GivenName );
            Assert.AreEqual( ((Individual)johnSmith.Participant).FamilyName, ((Individual)newJohnSmith.Participant).FamilyName );
            Assert.IsTrue( newJohnSmith.Entries.Count == 1 );
            Assert.AreEqual( ((CourseOfFireEntryIndividual)johnSmith.Entries[0]).ResultCofId, ((CourseOfFireEntryIndividual)newJohnSmith.Entries[0]).ResultCofId );

            var newATeam = await MatchParticipant.LoadFromFileAsync( aTeamUpdatedExpectedFullFileName );
            Assert.AreEqual( aTeam.Participant.DisplayName, newATeam.Participant.DisplayName );
            Assert.AreEqual( aTeam.ParticipantID, newATeam.ParticipantID );
            Assert.AreEqual( ((Team)aTeam.Participant).TeamName, ((Team)newATeam.Participant).TeamName );
            Assert.IsTrue( newATeam.Entries.Count == 1 );
        }

        [TestMethod]
        public async Task MatchProjectSerializationTest() {
            var matchName = "MatchProjectSerializationTest";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( TestClubAbbr, matchName, RelativeDirectoryForTesting );
            var match = project.Match;
            ClearDirectory( project.ProjectDirectory.FullName );

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

            var johnSmith = await project.CreateMatchParticipantAsync( "Smith", "John" );
            var janeDoe = await project.CreateMatchParticipantAsync( "Doe", "Jane" );
            var aTeam = await project.CreateMatchParticipantAsync( "Team A" );

            Assert.AreEqual( "MatchProjectSerializationTest.orion", project.GetFileName() );

            var expectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, matchName, project.GetFileName() );

            project.SaveToFile();
            Assert.IsTrue( File.Exists( expectedFullFileName ), $"File does not exist: {expectedFullFileName}" );

            var newProject = await MatchProject.LoadFromFileAsync( expectedFullFileName );
            Assert.IsNotNull( newProject );
            Assert.AreEqual( project.ProjectName, newProject.ProjectName );
            Assert.AreEqual( project.Match.Name, newProject.Match.Name );
            Assert.AreEqual( project.Participants.Count, newProject.Participants.Count );
        }

        /// <summary>
        /// Helper method to clean up a directory by deleting all files and subdirectories within it. This is used to ensure a clean slate for tests that involve file creation and serialization.
        /// </summary>
        /// <param name="directory"></param>
        void ClearDirectory( string directory ) {
            // Delete files
            foreach (var file in Directory.GetFiles( directory )) {
                File.Delete( file );
            }

            // Delete subdirectories
            foreach (var dir in Directory.GetDirectories( directory )) {
                Directory.Delete( dir, recursive: true );
            }
        }
    }
}
