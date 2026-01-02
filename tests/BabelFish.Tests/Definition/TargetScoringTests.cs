using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class TargetScoringTests : BaseTestClass {

        [TestMethod]
        public async Task AirRifleScoresTest() {

            var airRifleTargetSetName = SetName.Parse( "v1.0:issf:10m Air Rifle" );
            var airRifleTarget = await DefinitionCache.GetTargetDefinitionAsync( airRifleTargetSetName );

            var s1 = airRifleTarget.Score( 2.5f, 0f, 4.5f );
            Assert.AreEqual( 10, s1.I );
            Assert.AreEqual( 10.0f, s1.D );
            Assert.AreEqual( 0, s1.X );

            var s2 = airRifleTarget.Score( -2.5f, 0f, 4.5f );
            Assert.AreEqual( 10, s2.I );
            Assert.AreEqual( 10.0f, s2.D );
            Assert.AreEqual( 0, s2.X );

            var s3 = airRifleTarget.Score( 0f, 2.5f, 4.5f );
            Assert.AreEqual( 10, s3.I );
            Assert.AreEqual( 10.0f, s3.D );
            Assert.AreEqual( 0, s3.X );

            var s4 = airRifleTarget.Score( 0f, -2.5f, 4.5f );
            Assert.AreEqual( 10, s4.I );
            Assert.AreEqual( 10.0f, s4.D );
            Assert.AreEqual( 0, s4.X );

            var s5 = airRifleTarget.Score( 0f, 0f, 4.5f );
            Assert.AreEqual( 10, s5.I );
            Assert.AreEqual( 10.9f, s5.D );
            Assert.AreEqual( 1, s5.X );

            var s6 = airRifleTarget.Score( 0.1f, 0.1f, 4.5f );
            Assert.AreEqual( 10, s6.I );
            Assert.AreEqual( 10.9f, s6.D );
            Assert.AreEqual( 1, s6.X );

            var s7 = airRifleTarget.Score( 0.2f, 0.2f, 4.5f );
            Assert.AreEqual( 10, s7.I );
            Assert.AreEqual( 10.8f, s7.D );
            Assert.AreEqual( 1, s7.X );

            var s8 = airRifleTarget.Score( 0.4f, 0.4f, 4.5f );
            Assert.AreEqual( 10, s8.I );
            Assert.AreEqual( 10.7f, s8.D );
            Assert.AreEqual( 1, s8.X );

            var s9 = airRifleTarget.Score( 1.1f, 1.1f, 4.5f );
            Assert.AreEqual( 10, s9.I );
            Assert.AreEqual( 10.3f, s9.D );
            Assert.AreEqual( 1, s9.X );

            var s10 = airRifleTarget.Score( 1.5f, 1.5f, 4.5f );
            Assert.AreEqual( 10, s10.I );
            Assert.AreEqual( 10.1f, s10.D );
            Assert.AreEqual( 0, s10.X );

            var s11 = airRifleTarget.Score( 1.9f, 1.9f, 4.5f );
            Assert.AreEqual( 9, s11.I );
            Assert.AreEqual( 9.9f, s11.D );
            Assert.AreEqual( 0, s11.X );
        }

    }
}
