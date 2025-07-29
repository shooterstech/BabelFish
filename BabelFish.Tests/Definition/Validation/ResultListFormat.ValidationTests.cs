using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
	public class ResultListFormatValidationTests : BaseTestClass{

		[TestMethod]
		public async Task HappyPathAttributeValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:orion:3P Qualification" );

			var candidate = (await client.GetResultListFormatDefinitionAsync( setName )).Value;

			var validation = new IsResultListFormatValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

			Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
