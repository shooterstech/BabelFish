using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
	public class RankingRulesValidationTests : BaseTestClass {

		[TestMethod]
		public async Task HappyPathAttributeValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:orion:Generic Qualification" );

			var candidate = (await client.GetRankingRuleDefinitionAsync( setName )).Value;

			var validation = new IsRankingRulesValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
