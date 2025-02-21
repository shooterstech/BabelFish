
namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {

        [TestInitialize]
        public virtual void InitializeTest() {
            //Initialize the system, without pre-poulating the Definitino Cache (which avoids unnecessary API calls).
            Initializer.Initialize( Constants.X_API_KEY, false );
        }
    }
}
