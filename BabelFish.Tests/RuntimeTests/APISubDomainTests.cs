using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.Tests.RuntimeTests
{

    [TestClass]
    public class APISubDomainTests
    {

        [TestMethod]
        public void SubDomainNameExstentionTest() {

            var api = APISubDomain.API;
            var authapi = APISubDomain.AUTHAPI;
            var internalapi = APISubDomain.INTERNAL;

            Assert.AreEqual( "api", api.SubDomainName() );
            Assert.AreEqual( "authapi", authapi.SubDomainName() );
            Assert.AreEqual( "internalapi", internalapi.SubDomainName() );
        }

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
