using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Miscellaneous {
    [TestClass]
    public class CloningTests : BaseTestClass {

        [TestMethod]
        public async Task CloneAnAttributeTest() {

            var attrDefinition = await DefinitionFactory.Build<Scopos.BabelFish.DataModel.Definitions.Attribute>( "OrionAcct000001", "Not a Real Attribute", "orion" );
            attrDefinition.SetDefaultValues();

            var attrClone = attrDefinition.Clone();
            Assert.IsNotNull( attrClone );

            Assert.AreEqual( attrDefinition.HierarchicalName, attrClone.HierarchicalName );
        }
    }
}
