
using OfficeOpenXml;

namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {

        [TestInitialize]
        public virtual void InitializeTest() {
            //Initialize the system, without pre-poulating the Definitino Cache (which avoids unnecessary API calls).
            Initializer.Initialize( Constants.X_API_KEY, false );
            //add EPPlus license, was unable to add it to app.config
            ExcelPackage.License.SetCommercial("1VeFo1xFNO0YNHS3Kqf2b/hoiDBi3yxU65Ti/jU1vXs+VVsEmxQsCh4+SLL9mtZu1UYE5lwjtqOMrNGfn+kHuwEGRDMzQTYz5wc+ARkEAQIA");
        }
    }
}
