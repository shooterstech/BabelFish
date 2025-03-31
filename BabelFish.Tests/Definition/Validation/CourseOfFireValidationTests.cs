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

			Initializer.UpdateLocalStoreDirectory( @"c:\temp" );
			//var setName = SetName.Parse( "v1.0:cmp:Smallbore Rifle 3x20" );
			var setName = SetName.Parse( "v1.0:cmp:High Power Rifle National Match Course" );

			var candidate = await DefinitionCache.GetCourseOfFireDefinitionAsync( setName );

			var validation = new IsCourseOfFireValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

            Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
		}
	}
}
