using System.IO;
using OfficeOpenXml;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Clubs;

namespace Scopos.BabelFish.Tests {

    [TestClass]
    public class BaseTestClass {


        public static DirectoryInfo RelativeDirectoryForDefinitions { get; set; } = new System.IO.DirectoryInfo( @"C:\temp" );
        public static DirectoryInfo RelativeDirectoryForTesting { get; set; } = new System.IO.DirectoryInfo( @"C:\temp\UnitTests" );

        /// <summary>
        /// Orion for Clubs account number 15 is a special account that is used for unit testing. Its visibility is set to Protected.
        /// </summary>
        public static ClubAbbr TestClubProtectedAbbr { get; set; } = new ClubAbbr() {
            AccountNumber = 15,
            Name = "Unit Test - Protected Club"
        };

        /// <summary>
        /// Orion for Clubs account number 16 is a special account that is used for unit testing. Its visibility is set to Public.
        /// </summary>
        public static ClubAbbr TestClubPublicAbbr { get; set; } = new ClubAbbr() {
            AccountNumber = 16,
            Name = "Unit Test - Public Club"
        };

        [TestInitialize]
        public virtual void InitializeTest() {
            var xApiKey = Environment.GetEnvironmentVariable( "ScoposXApiKey" );
            var excelPackageLicense = Environment.GetEnvironmentVariable( "ExcelPackageLicense" );

            //Initialize the system, without pre-poulating the Definitino Cache (which avoids unnecessary API calls).
            Initializer.Initialize( xApiKey, false );

            if (excelPackageLicense != null) {
                //add EPPlus license, was unable to add it to app.config
                ExcelPackage.License.SetCommercial( excelPackageLicense );
            } else {
                Console.WriteLine( "ExcelPackageLicense environment variable not set, EPPlus will run in non-commercial mode which may cause some features to not work." );
            }


            DefinitionAPIClient.LocalStoreDirectory = RelativeDirectoryForDefinitions;

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }

        /// <summary>
        /// Helper method to clean up a directory by deleting all files and subdirectories within it. This is used to ensure a clean slate for tests that involve file creation and serialization.
        /// </summary>
        /// <param name="directory"></param>
        protected void ClearDirectory( string directory ) {
            // Delete files
            foreach (var file in Directory.GetFiles( directory )) {
                File.Delete( file );
            }

            // Delete subdirectories
            foreach (var dir in Directory.GetDirectories( directory )) {
                Directory.Delete( dir, recursive: true );
            }
        }
    }
}
