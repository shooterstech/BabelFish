using System.Threading.Tasks;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Tests.Miscellaneous {

    [TestClass]
    public class CommonTests : BaseTestClass {


        [TestMethod]
        public async Task PathSanitizerTests() {

            var goodFullPath = @"c:\temp\test.json";
            Assert.AreEqual( @"c:\temp\test.json", Common.SanitizeFullFilenamePath( goodFullPath ) );

            var badFileName = @"c:\temp\test?file.json";
			Assert.AreEqual( @"c:\temp\test-file.json", Common.SanitizeFullFilenamePath( badFileName ) );

			var badDirectoryName = @"c:\temp|dir\test.json";
			Assert.AreEqual( @"c:\temp-dir\test.json", Common.SanitizeFullFilenamePath( badDirectoryName ) );

			var badFileAndDirectoryName = @"c:\temp|dir\test?file.json";
			Assert.AreEqual( @"c:\temp-dir\test-file.json", Common.SanitizeFullFilenamePath( badFileAndDirectoryName ) );

            var goodDirectory = @"c:\temp";
            var goodFileName = "test.json";
			Assert.AreEqual( @"c:\temp\test.json", Common.SanitizePath( goodDirectory, goodFileName ) );

            var badFileName2 = "test\\file.json";
			Assert.AreEqual( @"c:\temp\test-file.json", Common.SanitizePath( goodDirectory, badFileName2 ) );

		}
    }
}
