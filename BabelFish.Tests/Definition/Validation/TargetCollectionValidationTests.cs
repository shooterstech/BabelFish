using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
	public class TargetCollectionValidationTests : BaseTestClass {

		[TestMethod]
		public async Task HappyPathAttributeValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:ntparc:Air Rifle" );

			var candidate = (await client.GetTargetCollectionDefinitionAsync( setName )).Value;

			var validation = new IsTargetCollectionValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}

		[TestMethod]
		public async Task TargetCollectionNamesUniqueTest() {

			//Retreive a TARGET COLLECTION as a base, copy it so we dont' mess up the original.
            var setName = SetName.Parse( "v1.0:ntparc:Air Rifle" );
			var definition = (await DefinitionCache.GetTargetCollectionDefinitionAsync( setName )).Clone();

            var validation = new IsTargetCollectionValid();

            //Set the TargetCollectionName to an empty string which should fail the test.
            definition.TargetCollections[0].TargetCollectionName = string.Empty;
			Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );

			//Set each TargetCollectionName to the same value, which should also fail
			foreach (var item in definition.TargetCollections)
				item.TargetCollectionName = "AAAA";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task TargetCollectionEmptyRangeDistanceTest() {

            //Retreive a TARGET COLLECTION as a base, copy it so we dont' mess up the original.
            var setName = SetName.Parse( "v1.0:ntparc:Air Rifle" );
            var definition = (await DefinitionCache.GetTargetCollectionDefinitionAsync( setName )).Clone();

            var validation = new IsTargetCollectionValid();

            //Set the RangeDistance to an empty string which should fail the test.
            definition.TargetCollections[0].RangeDistance = string.Empty;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task TargetCollectionEmptyTargetDefsListTest() {

            //Retreive a TARGET COLLECTION as a base, copy it so we dont' mess up the original.
            var setName = SetName.Parse( "v1.0:ntparc:Air Rifle" );
            var definition = (await DefinitionCache.GetTargetCollectionDefinitionAsync( setName )).Clone();

            var validation = new IsTargetCollectionValid();

            //Set the TargetDefs to an empty list.
            definition.TargetCollections[0].TargetDefs.Clear();
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }

        [TestMethod]
        public async Task TargetCollectionInvalidTargetDefsTest() {

            //Retreive a TARGET COLLECTION as a base, copy it so we dont' mess up the original.
            var setName = SetName.Parse( "v1.0:ntparc:Air Rifle" );
            var definition = (await DefinitionCache.GetTargetCollectionDefinitionAsync( setName )).Clone();

            var validation = new IsTargetCollectionValid();

            //Set the TargetDefs to an empty list.
            definition.TargetCollections[0].TargetDefs.Clear();
            definition.TargetCollections[0].TargetDefs.Add( "v1.0:orion:not a real target" );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( definition ) );
            Assert.AreEqual( 1, validation.Messages.Count );
        }
    }
}
