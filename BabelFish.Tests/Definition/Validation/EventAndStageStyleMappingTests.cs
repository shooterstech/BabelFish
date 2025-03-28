using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
	public class EventAndStageStyleMappingTests : BaseTestClass {

		[TestMethod]
		public async Task HappyPathAttributeValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:orion:Air Rifle" );

			var candidate = (await client.GetEventAndStageStyleMappingDefinitionAsync( setName )).Value;

			var validation = new IsEventAndStageStyleMappingValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}

		[TestMethod]
		public async Task DefaultMappingDoesNotExistTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
			var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

			var validation = new IsEventAndStageStyleMappingValid();

			//Delete the DefaultMapping, which should cause a failure.
			definition.DefaultMapping = null;

			Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingHasDefaultStageStyleDefTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            //Delete the DefaultMapping, which should cause a failure.
            definition.DefaultMapping.DefaultStageStyleDef = "v1.0:orion:not a real definition";

            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingHasDefaultEventStyleDefTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            //Delete the DefaultMapping, which should cause a failure.
            definition.DefaultMapping.DefaultEventStyleDef = "v1.0:orion:not a real definition";

            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingHasEmptyAttributeValueAppellationTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            //Delete the DefaultMapping, which should cause a failure.
            definition.DefaultMapping.AttributeValueAppellation.Add( "Some Value" );

            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingHasEmptyTargetCollectionNameTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            //Delete the DefaultMapping, which should cause a failure.
            definition.DefaultMapping.TargetCollectionName.Add( "Some Value" );

            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingUniqueStageAppellationNamesTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            definition.DefaultMapping.StageStyleMappings[0].StageAppellation = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );

            foreach (var ssm in definition.DefaultMapping.StageStyleMappings)
                ssm.StageAppellation = "Some name";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 6, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingUniqueEventAppellationNamesTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            definition.DefaultMapping.EventStyleMappings[0].EventAppellation = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );

            foreach (var ssm in definition.DefaultMapping.EventStyleMappings)
                ssm.EventAppellation = "Some name";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingValidStageStyleDefTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            definition.DefaultMapping.StageStyleMappings[0].StageStyleDef = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task DefaultMappingValidEventStyleDefTest() {

            var setName = SetName.Parse( "v1.0:orion:Air Rifle" );
            var definition = (await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( setName )).Clone();

            var validation = new IsEventAndStageStyleMappingValid();

            definition.DefaultMapping.EventStyleMappings[0].EventStyleDef = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }
    }
}
