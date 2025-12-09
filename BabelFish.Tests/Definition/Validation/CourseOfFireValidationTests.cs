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

            Assert.IsNotNull( candidate );

			var validation = new IsCourseOfFireValid();

			var valid = await validation.IsSatisfiedByAsync( candidate );

            Assert.IsTrue( valid, string.Join( ", ", validation.Messages ) );
        }

        [TestMethod]
        public async Task TargetCollectionIndexInvalidTest() {

            var setName = SetName.Parse( "v1.0:cmp:Smallbore Rifle 3x20" );

            var candidate = await DefinitionCache.GetCourseOfFireDefinitionAsync( setName );

            Assert.IsNotNull( candidate );

            var validation = new IsCourseOfFireValid();

            //Should initially pass validation
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            //Set a singular's target colleciton index to an illegal value. Should fail validation.
            candidate.Singulars[0].TargetCollectionIndex = -1;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            //Set a singular's target colleciton index to an illegal value. Should fail validation.
            candidate.Singulars[0].TargetCollectionIndex = 100;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            //Restore the original (legal) value
            candidate.Singulars[0].TargetCollectionIndex = -0;

            candidate.RangeScripts[0].SegmentGroups[0].Segments[0].TargetCollectionIndex = -1;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            candidate.RangeScripts[0].SegmentGroups[0].Segments[0].TargetCollectionIndex = 100;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );
        }
    }
}
