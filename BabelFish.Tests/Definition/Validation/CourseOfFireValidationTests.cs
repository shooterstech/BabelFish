using System.IO;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {
    [TestClass]
	public class CourseOfFireValidationTests : BaseTestClass {

		[TestMethod]
		public async Task HappyPathCourseOfFireValid() {

			var client = new DefinitionAPIClient();
			var setName = SetName.Parse( "v1.0:cmp:Smallbore Rifle 3x20" );

			var candidate = (await client.GetCourseOfFireDefinitionAsync( setName )).Value;

			var validation = new IsCourseOfFireValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

            Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
