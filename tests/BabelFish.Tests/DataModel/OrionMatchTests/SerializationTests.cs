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
            Match match = new Match();
            match.Name = "MatchFileNameTests";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( match, RelativeDirectoryForTesting );
            Directory.Delete( project.ProjectDirectory.FullName, true );

            var expectedFullFileName = Path.Combine( BaseTestClass.RelativeDirectoryForTesting.FullName, "MatchFileNameTests", "MatchFileNameTests.json" );
            if (File.Exists( expectedFullFileName ))
                File.Delete( expectedFullFileName );

            Assert.AreEqual( "MatchFileNameTests.json", match.GetFileName() );
            Assert.AreEqual( Path.Combine( "MatchFileNameTests", "MatchFileNameTests.json" ), match.GetRelativePath() );
            var fullFileName = match.SaveToFile( BaseTestClass.RelativeDirectoryForTesting );
            Assert.AreEqual( expectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );
        }

        [TestMethod]
        public async Task MatchSerializaeDeserializeTests() {
            Match match = new Match();
            match.Name = "MatchSerializaeDeserializeTests";

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( match, RelativeDirectoryForTesting );
            Directory.Delete( project.ProjectDirectory.FullName, true );

            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            CourseOfFireStructure cof, deserializedCof;
            match.MatchStructure.TryGetCourseOfFireStructure( cofId, out cof );
            Assert.IsNotNull( cof );
            cof.Attributes.Add( await AttributeConfiguration.FactoryAsync( newShooterSetName ) );

            //Let the Wizard do it's thing
            ResultListWizard wizard = new ResultListWizard( match );
            var resultLists = await wizard.GenerateAsync( cofId );

            //Add all the result lists so we have plenty to serialize and deserialize
            foreach (var resultList in resultLists)
                cof.AddResultList( resultList );

            var fullFileName = match.SaveToFile( BaseTestClass.RelativeDirectoryForTesting );
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

            Match match = new Match();
            match.Name = "CourseOfFireEntryFileNameTest";

            //Create the MatchProject so we have a directory to save to. Then remove the directory to ensure a clean slate for the test.
            MatchProject project = await MatchProject.CreateAsync( match, RelativeDirectoryForTesting );
            ClearDirectory( project.ProjectDirectory.FullName );

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

            //Add a second Attribute for good measure.
            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            CourseOfFireStructure cof, deserializedCof;
            match.MatchStructure.TryGetCourseOfFireStructure( cofId, out cof );
            Assert.IsNotNull( cof );
            cof.Attributes.Add( await AttributeConfiguration.FactoryAsync( newShooterSetName ) );

            var johnSmith = await project.CreateMatchParticipantAsync( "Smith", "John" );
            var janeDoe = await project.CreateMatchParticipantAsync( "Doe", "Jane" );

            var johnSmithExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, "CourseOfFireEntryFileNameTest", MatchParticipant.FOLDER_NAME, $"{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json" );
            var janeDoeExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, "CourseOfFireEntryFileNameTest", MatchParticipant.FOLDER_NAME, $"{janeDoe.Participant.DisplayName} {janeDoe.ParticipantID}.json" );

            Assert.AreEqual( $"{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json", johnSmith.GetFileName() );
            Assert.AreEqual( $"{MatchParticipant.FOLDER_NAME}\\{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json", johnSmith.GetRelativePath() );

            var fullFileName = johnSmith.SaveToFile();
            Assert.AreEqual( johnSmithExpectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );

            fullFileName = janeDoe.SaveToFile();
            Assert.AreEqual( janeDoeExpectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );

            //Update the display name and verify that the file name changes accordingly
            johnSmith.Participant.DisplayName = "Johnathan Smith";
            var johnSmithUpdatedExpectedFullFileName = Path.Combine( RelativeDirectoryForTesting.FullName, "CourseOfFireEntryFileNameTest", MatchParticipant.FOLDER_NAME, $"{johnSmith.Participant.DisplayName} {johnSmith.ParticipantID}.json" );
            Assert.IsTrue( File.Exists( johnSmithUpdatedExpectedFullFileName ), $"File does not exist: {johnSmithUpdatedExpectedFullFileName}" );

            //Now try and deserialize one of the files.
            var newJohnSmith = await MatchParticipant.LoadFromFileAsync( johnSmithUpdatedExpectedFullFileName );
            Assert.AreEqual( johnSmith.Participant.DisplayName, newJohnSmith.Participant.DisplayName );
            Assert.AreEqual( johnSmith.ParticipantID, newJohnSmith.ParticipantID );
            Assert.AreEqual( ((Individual)johnSmith.Participant).GivenName, ((Individual)newJohnSmith.Participant).GivenName );
            Assert.AreEqual( ((Individual)johnSmith.Participant).FamilyName, ((Individual)newJohnSmith.Participant).FamilyName );
            Assert.IsTrue( newJohnSmith.Entries.Count == 1 );
            Assert.AreEqual( ((CourseOfFireEntryIndividual)johnSmith.Entries[0]).ResultCofId, ((CourseOfFireEntryIndividual)newJohnSmith.Entries[0]).ResultCofId );
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
