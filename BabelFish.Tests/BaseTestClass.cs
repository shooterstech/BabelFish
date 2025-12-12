
using System.Configuration;
using OfficeOpenXml;

namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {

        [TestInitialize]
        public virtual void InitializeTest() {
            var xApiKey = Environment.GetEnvironmentVariable("ScoposXApiKey");
            var excelPackageLicense = Environment.GetEnvironmentVariable( "ExcelPackageLicense" );

            //Initialize the system, without pre-poulating the Definitino Cache (which avoids unnecessary API calls).
            Initializer.Initialize( xApiKey , false );
            //add EPPlus license, was unable to add it to app.config
            ExcelPackage.License.SetCommercial( excelPackageLicense );
        }
    }
}
