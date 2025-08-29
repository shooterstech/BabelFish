using System.IO;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.Tests.DataActors
{
    [TestClass]
    public class CourseOfFireSpecificationTests : BaseTestClass
    {
        [TestMethod]
        public async Task GrowEventTreeSandboxTest()
        {
            var client = new DefinitionAPIClient() { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:orion:Test Informal Practice Air Rifle");

            var result = await client.GetCourseOfFireDefinitionAsync(setName);
            Assert.AreEqual(HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}.");

            var definition = result.Definition;
            Assert.IsNotNull(definition);

            var eventTree = EventComposite.GrowEventTree(definition);
            var events = eventTree.GetEvents(singular: false );

        }

        [TestMethod]
        public async Task EventTreeValidTest()
        {
            var client = new DefinitionAPIClient() { IgnoreInMemoryCache = true };
            var setName = SetName.Parse("v1.0:orion:Test Informal Practice Air Rifle");

            var result = await client.GetCourseOfFireDefinitionAsync(setName);
            Assert.AreEqual(HttpStatusCode.OK, result.RestApiStatusCode, $"Expecting and OK status code, instead received {result.RestApiStatusCode}.");

            var definition = result.Definition;
            Assert.IsNotNull(definition);

            var cofSpec = new IsCourseOfFireEventTreeValid();
            bool sat = await cofSpec.IsSatisfiedByAsync(definition);
            Assert.IsTrue( sat );
        }
    }
}