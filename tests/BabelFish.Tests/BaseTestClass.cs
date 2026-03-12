using System.IO;
using OfficeOpenXml;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {


        public static DirectoryInfo RelativeDirectoryForTesting { get; set; } = new System.IO.DirectoryInfo( @"C:\temp" );

        [TestInitialize]
        public virtual void InitializeTest() {
            var xApiKey = Environment.GetEnvironmentVariable( "ScoposXApiKey" );
            var excelPackageLicense = Environment.GetEnvironmentVariable( "ExcelPackageLicense" );

            //Initialize the system, without pre-poulating the Definitino Cache (which avoids unnecessary API calls).
            Initializer.Initialize( xApiKey, false );

            if(excelPackageLicense != null) {
                //add EPPlus license, was unable to add it to app.config
                ExcelPackage.License.SetCommercial(excelPackageLicense);
            }
            else {
                Console.WriteLine("ExcelPackageLicense environment variable not set, EPPlus will run in non-commercial mode which may cause some features to not work.");
            }
            

            DefinitionAPIClient.LocalStoreDirectory = RelativeDirectoryForTesting;

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }
    }
}
