using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition.Validation {

    [TestClass]
    public class DefinitionValidationTests : BaseTestClass {

        [TestMethod]
        public async Task IsOwnerValidTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var origOwner = stageStyle.Owner;
            var origHiername = stageStyle.HierarchicalName;

            var validation = new IsDefiniitonOwnerValid();

            //The unaltered should pass
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );

            //empty stirng should fail
            stageStyle.Owner = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //Wrong owner value should fail
            stageStyle.Owner = "OrionAcct001234";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //Wrong HierarchicalName shold fail
            stageStyle.Owner = origOwner;
            stageStyle.HierarchicalName = "notanamespace:Sporter Air Rifle Standing";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );
        }

        [TestMethod]
        public async Task IsVersionValidTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:ntparc:Sporter Air Rifle Standing" );

            var stageStyle = (await client.GetStageStyleDefinitionAsync( setName )).Value;

            var validation = new IsDefiniitonVersionValid();

            //The unaltered should pass
            Assert.IsTrue( await validation.IsSatisfiedByAsync( stageStyle ) );

            //empty stirng should fail
            stageStyle.Version = "";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //Wrong format should not pass
            stageStyle.Version = "v1.2";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //More wrong formats
            stageStyle.Version = "12";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );

            //More wrong formats
            stageStyle.Version = "a.b";
            Assert.IsFalse( await validation.IsSatisfiedByAsync( stageStyle ) );
        }
    }
}
