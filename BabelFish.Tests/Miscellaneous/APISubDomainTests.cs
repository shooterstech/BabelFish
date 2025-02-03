﻿using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests.Miscellaneous {

    [TestClass]
    public class APISubDomainTests : BaseTestClass
    {

        [TestMethod]
        public void DumbTest() {
            Assert.AreEqual( 1, 1 );
        }

        /// <summary>
        /// Tests that the Exstention method .SubDomainName() works as expected.
        /// </summary>
        [TestMethod]
        public void SubDomainNameExstentionTest() {

            var api = APISubDomain.API;
            var authapi = APISubDomain.AUTHAPI;
            var internalapi = APISubDomain.INTERNAL;

            Assert.AreEqual( "api", api.SubDomainName() );
            Assert.AreEqual( "authapi", authapi.SubDomainName() );
            Assert.AreEqual( "internalapi", internalapi.SubDomainName() );
        }

        /// <summary>
        /// Tests that the Exstention method .SubDomainNameWithState works as expected.
        /// </summary>
        [TestMethod]
        public void SubDomainNameWithStageExstentionTest() {

            var api = APISubDomain.API;
            var authapi = APISubDomain.AUTHAPI;
            var internalapi = APISubDomain.INTERNAL;

            Assert.AreEqual( "api-stage", api.SubDomainNameWithStage() );
            Assert.AreEqual( "authapi-stage", authapi.SubDomainNameWithStage() );
            Assert.AreEqual( "internalapi-stage", internalapi.SubDomainNameWithStage() );
        }
    }
}
