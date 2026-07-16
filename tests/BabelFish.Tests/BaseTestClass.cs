using OfficeOpenXml;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {

        [TestInitialize]
        public virtual void InitializeTest() {
            var xApiKey = Environment.GetEnvironmentVariable( "ScoposXApiKey" );
            var excelPackageLicense = Environment.GetEnvironmentVariable( "ExcelPackageLicense" );

            //Initialize the system, without pre-poulating the Definitino Cache (which avoids unnecessary API calls).
            Initializer.Initialize( xApiKey, false );
            //add EPPlus license, was unable to add it to app.config
            if (excelPackageLicense != null) {
                ExcelPackage.License.SetCommercial(excelPackageLicense);
            }
            else {
                //print no license found
                Console.WriteLine("No excel package license found, excel tests will fail");
            }

            DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"C:\temp" );

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }
    }
}
