using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests.Definition {
    [TestClass]
    public class DefinitionVersionTests {


        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

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
    }
}
