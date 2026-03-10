
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    [TestClass]
    public class ResultListWizardTests : BaseTestClass {

        /// <summary>
        /// Tests that the ResultListWizard correctly creates ResultListAbbr for a CourseOfFireStructure with one Top Level Attribute.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task SingleTopLevelAttribute() {
            Match match = new Match();

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );
            CourseOfFireStructure cof;
            match.MatchStructure.TryGetCourseOfFireStructure( cofId, out cof );

            //Let the wizard do it's thing
            ResultListWizard wizard = new ResultListWizard( match );
            var resultLists = await wizard.GenerateAsync( cofId );

            Assert.IsNotNull( resultLists );

            ResultListAbbr? rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - All" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 0, rlUnderTest.AttributeFilter.Count );
            Assert.IsTrue( cof.AddResultList( rlUnderTest ) );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - All" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 0, rlUnderTest.AttributeFilter.Count );
            Assert.IsTrue( cof.AddResultList( rlUnderTest ) );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Sporter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.IsTrue( cof.AddResultList( rlUnderTest ) );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Sporter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.IsTrue( cof.AddResultList( rlUnderTest ) );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Precision" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.IsTrue( cof.AddResultList( rlUnderTest ) );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Precision" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.IsTrue( cof.AddResultList( rlUnderTest ) );

            Console.WriteLine( Newtonsoft.Json.JsonConvert.SerializeObject( match, Scopos.BabelFish.Helpers.SerializerOptions.NewtonsoftJsonSerializer ) );

        }

        /// <summary>
        /// Tests that the ResultListWizard correctly creates ResultListAbbr for a CourseOfFireStructure with one Top Level Attribute and one Mid Tier Attribute
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TwoDifferentTieredAttribute() {
            Match match = new Match();

            //Add a CourseOfFireStructure into the Match. The Three-Position Air Rifle 3x10 has one required attriubte (Air Rifle Type)
            SetName setName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" );
            var cofId = await match.MatchStructure.AddCourseOfFireAsync( setName );

            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );
            CourseOfFireStructure cof;
            match.MatchStructure.TryGetCourseOfFireStructure( cofId, out cof );
            Assert.IsNotNull( cof );
            cof.Attributes.Add( await AttributeConfiguration.FactoryAsync( newShooterSetName ) );

            //Let the Wizard do it's thing
            ResultListWizard wizard = new ResultListWizard( match );
            var resultLists = await wizard.GenerateAsync( cofId );

            Assert.IsNotNull( resultLists );

            ResultListAbbr? rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - All" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 0, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - All" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 0, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Sporter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Sporter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Precision" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Precision" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Sporter - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 2, rlUnderTest.AttributeFilter.Count );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Sporter - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 2, rlUnderTest.AttributeFilter.Count );
        }
    }
}
