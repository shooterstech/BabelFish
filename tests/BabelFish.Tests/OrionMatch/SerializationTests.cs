using System.IO;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class SerializationTests : BaseTestClass {

        /// <summary>
        /// Tests that the ISaveToFile interface methods are return the expected file names and paths, and that the file is actually created when saving.
        /// This test does not verify the contents of the file, only that it is created with the correct name and path.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task FileNameTests() {
            Match match = new Match();
            match.Name = "SerializationFileNameTest";
            var expectedFullFileName = Path.Combine( BaseTestClass.RelativeDirectoryForTesting.FullName, "SerializationFileNameTest", "SerializationFileNameTest.json" );
            if (File.Exists( expectedFullFileName ))
                File.Delete( expectedFullFileName );

            Assert.AreEqual( "SerializationFileNameTest.json", match.GetFileName() );
            Assert.AreEqual( Path.Combine( "SerializationFileNameTest", "SerializationFileNameTest.json" ), match.GetRelativePath() );
            var fullFileName = match.SaveToFile( BaseTestClass.RelativeDirectoryForTesting );
            Assert.AreEqual( expectedFullFileName, fullFileName );
            Assert.IsTrue( File.Exists( fullFileName ), $"File does not exist: {fullFileName}" );
        }

        [TestMethod]
        public async Task SerializaeDeserializeTests() {
            Match match = new Match();
            match.Name = "SerializationTest";

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

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

                //Console.WriteLine( Newtonsoft.Json.JsonConvert.SerializeObject( resultList, Scopos.BabelFish.Helpers.SerializerOptions.NewtonsoftJsonSerializer ) );
                //Console.WriteLine( resultList.AttributeFilter.GetHashCode() );
                //Console.WriteLine( ((AttributeFilterAttributeValue)resultList.AttributeFilter).Values[0].GetHashCode() );

                //Console.WriteLine( Newtonsoft.Json.JsonConvert.SerializeObject( deserializedResultList, Scopos.BabelFish.Helpers.SerializerOptions.NewtonsoftJsonSerializer ) );
                //Console.WriteLine( deserializedResultList.AttributeFilter.GetHashCode() );
                //Console.WriteLine( ((AttributeFilterAttributeValue)deserializedResultList.AttributeFilter).Values[0].GetHashCode() );

                Assert.AreEqual( resultList.GetHashCode(), deserializedResultList.GetHashCode() );
            }
        }
    }
}
