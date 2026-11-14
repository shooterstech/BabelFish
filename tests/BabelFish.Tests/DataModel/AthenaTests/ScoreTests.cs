using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.Tests.DataModel.AthenaTests {
    [TestClass]
    public class ScoreTests : BaseTestClass {
        [TestMethod]
        public void AddScoreComponentTests() {

            var qualification = new Score();
            qualification.X = 12;
            qualification.I = 380;
            qualification.D = 400.5f;
            qualification.S = 400.5f;

            var final = new Score();
            final.X = 2;
            final.I = 90;
            final.D = 93.5f;
            final.S = 93.5f;

            var aggregate = new Score();
            Assert.AreEqual( 0, aggregate.I );
            Assert.AreEqual( 0, aggregate.D );
            Assert.AreEqual( 0, aggregate.S );

            aggregate.Add( qualification, BabelFish.DataModel.Definitions.ScoreComponent.I );
            Assert.AreEqual( 380, aggregate.I );
            Assert.AreEqual( 400.5f, aggregate.D );
            Assert.AreEqual( 380f, aggregate.S );

            aggregate.Add( final, BabelFish.DataModel.Definitions.ScoreComponent.D );

            Assert.AreEqual( 470, aggregate.I );
            Assert.AreEqual( 494.0f, aggregate.D );
            Assert.AreEqual( 473.5f, aggregate.S );
        }
    }
}
