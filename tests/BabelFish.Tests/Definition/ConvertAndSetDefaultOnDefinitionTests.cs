using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class ConvertAndSetDefaultOnDefinitionTests : BaseTestClass {

        /// <summary>
        /// Tests the SetDefaultValues() method on the ResultListFormat definition.
        /// </summary>
        [TestMethod]
        public void SetDefaultOnNewResultListFormatTest() {

            var definition = new ResultListFormat();
            definition.SetDefaultValues();

            Assert.IsTrue( definition.Fields.Count == 2 );
            Assert.IsTrue( definition.Format.Columns.Count == 8 );
            Assert.IsTrue( definition.Format.Display.Header.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.Display.Body.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.Display.Children.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.Display.Footer.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.DisplayForTeam.Header.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.DisplayForTeam.Body.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.DisplayForTeam.Children.ClassSet.Count == 1 );
            Assert.IsTrue( definition.Format.DisplayForTeam.Footer.ClassSet.Count == 1 );
        }

        [TestMethod]
        public async Task ConvertValuesAfterResultListFormatReadTest() {
            SetName sn = SetName.Parse( "v1.0:orion:Test Convert on Read" );
            //Convert Values get called after read from Rest API.
            var definition = await DefinitionCache.GetResultListFormatDefinitionAsync( sn );


            Assert.AreEqual( "rlf-col-rank", definition.Format.Columns.First().ClassSet.First().Name );
            Assert.AreEqual( "rlf-row-header", definition.Format.Display.Header.ClassSet.First().Name );
            Assert.AreEqual( "rlf-row-team", definition.Format.DisplayForTeam.Body.ClassSet.First().Name );
        }
    }
}
