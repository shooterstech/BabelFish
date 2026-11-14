using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.DataModel.Definition.Validation {
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
            Assert.IsTrue( sat, string.Join( ", ", cofSpec.Messages ) );
        }

        [TestMethod]
        public async Task PaperTargetLabelsTests() {

            var validation = new IsCourseOfFirePaperTargetLabelValid();

            var candidate = new CourseOfFire();

            // A default CourseOfFire should be valid, b/c the constructor should auto add NONE label to the PaperTargetLabels list.
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            // If the NONE label is removed, the validation should fail.
            candidate.PaperTargetLabels.Clear();
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            // This should fail for multiple reasons: 1) PaperTargetLabelName is empty, 2) ShotsPerBull is 0
            candidate.PaperTargetLabels.Add( new PaperTargetLabel() {
                PaperTargetLabelName = string.Empty,
                ShotsPerBull = 0,
                Labels = new List<BarcodeLabel>()
            } );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            //Correct it, and it should pass.
            candidate.PaperTargetLabels[0].PaperTargetLabelName = "Test Label";
            candidate.PaperTargetLabels[0].ShotsPerBull = 1;
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            //This should fail because a corresponding Singular doesn't exist with a StageLabel of "S"
            candidate.PaperTargetLabels[0].Labels.Add( new BarcodeLabel() {
                StageLabel = "S",
                Series = new ValueSeries( "1" ),
                LabelSize = BarcodeLabelSize.OL385
            } );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            // Now should pass as we have a Singular with a StageLabel of "S"
            candidate.Singulars.Add( new Singular() {
                StageLabel = "S",
            } );
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

        }

        [TestMethod]
        public async Task StageLabelReferenceTests() {
            var validation = new IsCourseOfFireStageLabelReferencesValid();
            var candidate = new CourseOfFire();

            // Define a largely empty RangeScript with a single SegmentGroup and a single Segment. This will be used to test StageLabel references.

            var firstRangeScript = candidate.AddRangeScript( "Test RangeScript" );
            firstRangeScript.DesignedForEST = true;
            var firstSegmentGroup = firstRangeScript.AddSegmentGroup( "Test SegmentGroup" );
            var firstSegment = firstSegmentGroup.AddSegment();

            // A default CourseOfFire should not be valid, b/c we have yet to define any Singulars.
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            // Make a Singular with a StageLabel of "S". 
            candidate.RangeScripts[0].SegmentGroups[0].Segments[0].StageLabel = "S";

            // Should still be invalid, as we have no Singulars defined.
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            // Now add a Singular with a StageLabel of "S". This should make the CourseOfFire valid.
            candidate.Singulars.Add( new Singular() { StageLabel = "S" } );

            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            // Add a new Segment with a StageLabel of "X". This should make the CourseOfFire invalid, as there is no Singular with a StageLabel of "X".
            var secondSegment = firstSegmentGroup.AddSegment();
            secondSegment.StageLabel = "X";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );

            // Add a new Singular with a StageLabel of "X". This should make the CourseOfFire valid again.
            candidate.Singulars.Add( new Singular() { StageLabel = "X" } );
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            // Add a third Segment, but keep the StageLabel empty.
            var thirdSegment = firstSegmentGroup.AddSegment();

            // Now define the default segment to have a StageLabel of "S". This should make the CourseOfFire valid again, as the third segment will inherit the default segment's StageLabel.
            firstSegmentGroup.DefaultSegment.StageLabel = "S";
            Assert.IsTrue( await validation.IsSatisfiedByAsync( candidate ), string.Join( " : ", validation.Messages ) );

            // Now add a new Singular with a StageLabel of "Y". This should fail validation, as there is no Segment currently using a StageLabel of "Y".
            candidate.Singulars.Add( new Singular() { StageLabel = "Y" } );
            Assert.IsFalse( await validation.IsSatisfiedByAsync( candidate ) );
        }
    }
}
