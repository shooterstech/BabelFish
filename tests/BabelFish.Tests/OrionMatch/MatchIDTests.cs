using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class MatchIDTests {

        [TestMethod]
        /// <summary>
        /// Tests that we can cast MatchID objects to strings and that the resulting string is in the correct format.
        /// </summary>
        public void SetNameCastingTests() {
            MatchID matchId = MatchID.Parse( "1.2000.12345678.1" );

            // Implicit cast
            string mIdAsString = matchId;

            // Explicit cast
            string mIdAsString2 = (string)matchId;

            Assert.AreEqual( "1.2000.12345678.1", mIdAsString );
            Assert.AreEqual( "1.2000.12345678.1", mIdAsString2 );
        }
    }
}
