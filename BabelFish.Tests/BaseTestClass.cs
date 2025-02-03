
namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {

        [TestInitialize]
        public virtual void InitializeTest() {
            Initializer.Initialize( Constants.X_API_KEY );
        }
    }
}
