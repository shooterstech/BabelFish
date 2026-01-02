using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;

namespace Scopos.BabelFish.Tests.Definition {
    [TestClass]
    public class DefinitionVersionTests : BaseTestClass {


        [TestMethod]
        public async Task GetDefinitionVersionTests() {

            var client = new DefinitionAPIClient();
            var setName = SetName.Parse( "v1.0:colorado4h:Air Rifle 4x10" );
            var request = new GetDefinitionVersionPublicRequest( setName, DefinitionType.COURSEOFFIRE );

            var response = await client.GetDefinitionVersionPublicAsync( request );
            Assert.IsNotNull( response );

            var sd = response.SparseDefinition;
            Assert.IsNotNull( sd );
            var minVersion = new DefinitionVersion( "1.1" );
            Assert.IsTrue( sd.GetDefinitionVersion() >= minVersion );

        }

        [TestMethod]
        public void DefinitionVersion() {
            var v1_1 = new DefinitionVersion( "1.1" );
            var v1_2 = new DefinitionVersion( "1.2" );
            var v2_1 = new DefinitionVersion( "2.1" );
            var v2_2 = new DefinitionVersion( "2.2" );
            var v3_10 = new DefinitionVersion( "3.10" );
            var v3_20 = new DefinitionVersion( "3.20" );
            var v1_1a = new DefinitionVersion( "1.1" );

            Assert.IsTrue( v1_1 < v1_2 );
            Assert.IsTrue( v1_1 <= v1_2 );
            Assert.IsFalse( v1_1 > v2_1 );
            Assert.IsFalse( v1_1 >= v2_1 );
            Assert.IsTrue( v1_1 != v1_2 );
            Assert.IsFalse( v1_1 == v1_2 );

            Assert.IsTrue( v1_1 < v1_2 );
            Assert.IsTrue( v1_1 < v2_2 );
            Assert.IsTrue( v1_1 < v3_10 );
            Assert.IsTrue( v1_1 < v3_20 );

            Assert.IsTrue( v3_20 > v3_10 );
            Assert.IsTrue( v3_20 > v2_1 );
            Assert.IsTrue( v3_20 > v1_1 );

            Assert.IsTrue( v1_1 == v1_1a );
            Assert.IsFalse( v1_1 != v1_1a );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadVersionString1() {
            var v = new DefinitionVersion( "" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadVersionString2() {
            var v = new DefinitionVersion( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadVersionString3() {
            var v = new DefinitionVersion( "3.0" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadVersionString4() {
            var v = new DefinitionVersion( "0.0" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void BadVersionString5() {
            var v = new DefinitionVersion( "not a real version string" );
        }

        [TestMethod]
        public async Task MostRecentVersionTests() {

            var setName_0_0 = SetName.Parse( "v0.0:orion:Phone Number" );
            var setName_1_0 = SetName.Parse( "v1.0:orion:Phone Number" );
            var setName_2_0 = SetName.Parse( "v2.0:orion:Phone Number" );
            var setName_1_1 = SetName.Parse( "v1.1:orion:Phone Number" );
            var setName_2_1 = SetName.Parse( "v2.1:orion:Phone Number" );

            var phoneNumber_0_0 = await DefinitionCache.GetAttributeDefinitionAsync( setName_0_0 ); //Most Recent Version
            var phoneNumber_1_0 = await DefinitionCache.GetAttributeDefinitionAsync( setName_1_0 ); //Most recent major version 1.x
            var phoneNumber_2_0 = await DefinitionCache.GetAttributeDefinitionAsync( setName_2_0 ); //Most recent major version 2.x
            var phoneNumber_1_1 = await DefinitionCache.GetAttributeDefinitionAsync( setName_1_1 );
            var phoneNumber_2_1 = await DefinitionCache.GetAttributeDefinitionAsync( setName_2_1 );

            Assert.IsFalse( await phoneNumber_0_0.IsVersionUpdateAvaliableAsync() );

            Assert.IsFalse( await phoneNumber_1_0.IsVersionUpdateAvaliableAsync() );

            Assert.IsFalse( await phoneNumber_2_0.IsVersionUpdateAvaliableAsync() );

            Assert.IsFalse( await phoneNumber_1_1.IsVersionUpdateAvaliableAsync() );

            Assert.IsFalse( await phoneNumber_2_1.IsVersionUpdateAvaliableAsync() );

            var notYetInRestAPIDefinition = new StageStyle();
            notYetInRestAPIDefinition.Version = "1.1";
            notYetInRestAPIDefinition.HierarchicalName = "orion:Not Yet in API";
            notYetInRestAPIDefinition.SetName = $"v{notYetInRestAPIDefinition.Version}:{notYetInRestAPIDefinition.HierarchicalName}";

            Assert.IsFalse( await notYetInRestAPIDefinition.IsVersionUpdateAvaliableAsync() );
        }
    }
}
