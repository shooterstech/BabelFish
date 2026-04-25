
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.OrionMatch {
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
            var cof = await match.MatchStructure.AddCourseOfFireAsync( setName );
            var cofId = cof.CourseOfFireId;

            //Let the wizard do it's thing
            ResultListWizard wizard = new ResultListWizard( match );
            var resultLists = await wizard.GenerateAsync( cof );

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
            var cof = await match.MatchStructure.AddCourseOfFireAsync( setName );
            var cofId = cof.CourseOfFireId;

            SetName airRifleTypeSetName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            SetName newShooterSetName = SetName.Parse( "v1.0:ntparc:Three-Position New Shooter" );

            // Check that the GroupByPriority on the attribute definitions are the expected values.
            var airRifleDefinition = await DefinitionCache.GetAttributeDefinitionAsync( airRifleTypeSetName );
            var newShooterDefinition = await DefinitionCache.GetAttributeDefinitionAsync( newShooterSetName );
            Assert.AreEqual( 1, airRifleDefinition.GroupByPriority, "The GroupByPriority for the Air Rifle Type ATTRIBUTE is incorrect." );
            Assert.AreEqual( 2, newShooterDefinition.GroupByPriority, "The GroupByPriority for the New Shooter ATTRIBUTE is incorrect." );

            cof.Attributes.Add( await AttributeConfiguration.CreateAsync( newShooterSetName ) );
            SetName expectedRankingRule = SetName.Parse( "v2.0:ntparc:Three-Position Air Rifle Qualification" );
            //Let the Wizard do it's thing
            ResultListWizard wizard = new ResultListWizard( match );
            var resultLists = await wizard.GenerateAsync( cof );

            Assert.IsNotNull( resultLists );

            ResultListAbbr? rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - All" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 0, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - All" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 0, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Sporter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Sporter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Precision" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Precision" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 1, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Individual - Sporter - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 2, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );

            rlUnderTest = resultLists.Find( x => x.ResultName == "Team - Sporter - New Shooter" );
            Assert.IsTrue( rlUnderTest != null );
            Assert.AreEqual( 2, rlUnderTest.AttributeFilter.Count );
            Assert.AreEqual( expectedRankingRule, rlUnderTest.RankingRuleDef );
        }
    }
}
