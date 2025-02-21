using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.AttributeValue {

    /// <summary>
    /// Series of unit tests that test if new attribute values have expected default values
    /// </summary>
    [TestClass]
    public class DefaultAttributeValues  : BaseTestClass {

        /// <summary>
        /// As of Feb 2023 AttributeField is *NOT* reading the default values as specified by the definition. The class is instead using 
        /// hard coded default values. When AttributeField is updated to use default values from the definition this unit test will
        /// need to be updated.
        /// </summary>
        [TestMethod]
        public async Task DefaultValuesTest() {

            //The Test Attribute defines an attribute meant for testing. Do you like the name I gave it? I came up with it myself.
            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );

            var testAttributeValue = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte );
            var defaultDate = new DateTime( 2022, 1, 1 );

            //The default values are defined in the attribute definition. If the Attribute definition changes, so will these values to test against. 
            Assert.AreEqual( "1000", (string) testAttributeValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( 0, (int)testAttributeValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( 0, (float) testAttributeValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( false, (bool)testAttributeValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( defaultDate, (DateTime) testAttributeValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( 60, (float)testAttributeValue.GetFieldValue( "ATime" ) );

            Assert.IsTrue( ((List<string>)testAttributeValue.GetFieldValue( "AListOfStrings" )).Count == 0 );
            Assert.IsTrue( ((List<int>)testAttributeValue.GetFieldValue( "AListOfIntegers" )).Count == 0 );
            Assert.IsTrue( ((List<float>)testAttributeValue.GetFieldValue( "AListOfFloats" )).Count == 0 );
            Assert.IsTrue( ((List<DateTime>)testAttributeValue.GetFieldValue( "AListOfDates" )).Count == 0 );
            Assert.IsTrue( ((List<float>)testAttributeValue.GetFieldValue( "AListOfTimes" )).Count == 0 );
        }
    }
}
