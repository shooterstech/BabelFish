using System.IO;
using System.Net;
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

        /// <summary>
        /// Tests IsTargetCollectionIndexValid class, which verifies .TargetCollectionIndex has a valid value.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TargetCollectionIndexInvalidTest() {

            var setName = SetName.Parse( "v1.0:cmp:Smallbore Rifle 3x20" );

            var candidate = await DefinitionCache.GetCourseOfFireDefinitionAsync( setName );

            Assert.IsNotNull( candidate );

            var validation = new IsTargetCollectionIndexValid();

            //Should initially pass validation
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            Event firstStageEvent = null;
            foreach (var e in candidate.Events) {
                if (e.EventType == EventtType.STAGE) {
                    firstStageEvent = e;
                    break;
                }
            }

            //Set a singular's target colleciton index to an illegal value. Should fail validation.
            firstStageEvent.TargetCollectionIndex = -1;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            //Set a singular's target colleciton index to an illegal value. Should fail validation.
            firstStageEvent.TargetCollectionIndex = 100;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            //Restore the original (legal) value
            firstStageEvent.TargetCollectionIndex = -0;

            candidate.RangeScripts[0].SegmentGroups[0].Segments[0].TargetCollectionIndex = -1;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            candidate.RangeScripts[0].SegmentGroups[0].Segments[0].TargetCollectionIndex = 100;
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );
        }

        [TestMethod]
        public async Task GrowEventTreeSandboxTest() {
            var client = new DefinitionAPIClient() { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v1.0:orion:Test Informal Practice Air Rifle" );

            var result = await client.GetCourseOfFireDefinitionAsync( setName );
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );

            var definition = result.Definition;
            Assert.IsNotNull( definition );

            var eventTree = EventComposite.GrowEventTree( definition );
            var events = eventTree.GetEvents( singular: false );

        }

        [TestMethod]
        public async Task EventTreeValidTest() {
            var client = new DefinitionAPIClient() { IgnoreInMemoryCache = true };
            var setName = SetName.Parse( "v1.0:orion:Test Informal Practice Air Rifle" );

            var result = await client.GetCourseOfFireDefinitionAsync( setName );
            Assert.AreEqual( HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}." );

            var definition = result.Definition;
            Assert.IsNotNull( definition );

            var cofSpec = new IsCourseOfFireEventTreeValid();
            bool sat = await cofSpec.IsSatisfiedByAsync( definition );
            Assert.IsTrue( sat );
        }
    }
}
