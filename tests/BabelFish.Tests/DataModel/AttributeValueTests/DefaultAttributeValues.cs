using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.DataModel.AttributeValueTests {

    /// <summary>
    /// Series of unit tests that test if new attribute values have expected default values
    /// </summary>
    [TestClass]
    public class DefaultAttributeValues : BaseTestClass {

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
            Assert.AreEqual( "1000", (string)testAttributeValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( 0, (int)testAttributeValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( 0, (float)testAttributeValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( false, (bool)testAttributeValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( defaultDate, (DateTime)testAttributeValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( 60, (float)testAttributeValue.GetFieldValue( "ATime" ) );

            Assert.IsTrue( ((List<string>)testAttributeValue.GetFieldValue( "AListOfStrings" )).Count == 0 );
            Assert.IsTrue( ((List<int>)testAttributeValue.GetFieldValue( "AListOfIntegers" )).Count == 0 );
            Assert.IsTrue( ((List<float>)testAttributeValue.GetFieldValue( "AListOfFloats" )).Count == 0 );
            Assert.IsTrue( ((List<DateTime>)testAttributeValue.GetFieldValue( "AListOfDates" )).Count == 0 );
            Assert.IsTrue( ((List<float>)testAttributeValue.GetFieldValue( "AListOfTimes" )).Count == 0 );
        }

        [TestMethod]
        public async Task DefaultValueForMultiValuesClosedKey() {
            /*
             * Social Media Accounts is a unique test case. It allows for multiple values and the key field is CLOSED.
             */
            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Social Media Accounts" );

            var socialMediaAv = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte );

            Assert.IsTrue( socialMediaAv.GetAttributeFieldKeys().Count == 0 );

            socialMediaAv.SetFieldValue( "ProfileName", "erik", "TWITTER" );
            socialMediaAv.SetFieldValue( "ProfileName", "ben", "INSTAGRAM" );

            Assert.IsTrue( socialMediaAv.GetAttributeFieldKeys().Count == 2 );

            Assert.AreEqual( "erik", socialMediaAv.GetFieldValue( "ProfileName", "TWITTER" ) );
            Assert.AreEqual( "ben", socialMediaAv.GetFieldValue( "ProfileName", "INSTAGRAM" ) );
        }

        /// <summary>
        /// Tests that the Copy() method on AttributeValue creates a deep copy of the attribute value. This means that changes to the copy do not affect the original and vice versa.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CopyAnAttributeValue() {

            //The Test Attribute defines an attribute meant for testing. Do you like the name I gave it? I came up with it myself.
            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );

            var testAttributeValue = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte );

            DateTime start = new DateTime( 2020, 1, 1 );
            DateTime end = DateTime.Today;
            int range = (end - start).Days;
            var defaultDate = start.AddDays( Random.Shared.Next( range + 1 ) );
            var defaultInt = Random.Shared.Next();
            var defaultFloat = (float)Random.Shared.NextDouble();
            var defaultBool = Random.Shared.Next( 2 ) == 0;
            var defaultTime = (float)Random.Shared.NextDouble();
            var defaultString = Helpers.RandomStringGenerator.RandomAlphaNumericString( 10 );

            testAttributeValue.SetFieldValue( "AString", defaultString );
            testAttributeValue.SetFieldValue( "AnInteger", defaultInt );
            testAttributeValue.SetFieldValue( "AFloat", defaultFloat );
            testAttributeValue.SetFieldValue( "ABoolean", defaultBool );
            testAttributeValue.SetFieldValue( "ADate", defaultDate );
            testAttributeValue.SetFieldValue( "ATime", defaultTime );

            var copyOfTestAttributeValue = testAttributeValue.Copy();

            //First test that the copy has the same values as the original
            Assert.IsNotNull( copyOfTestAttributeValue );
            Assert.AreEqual( defaultString, (string)copyOfTestAttributeValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( defaultInt, (int)copyOfTestAttributeValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( defaultFloat, (float)copyOfTestAttributeValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( defaultBool, (bool)copyOfTestAttributeValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( defaultDate, (DateTime)copyOfTestAttributeValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( defaultTime, (float)copyOfTestAttributeValue.GetFieldValue( "ATime" ) );

            //Update the value on the copy and test that the original does not change (proving that it is a deep copy)
            var newDate = start.AddDays( Random.Shared.Next( range + 1 ) );
            var newInt = Random.Shared.Next();
            var newFloat = (float)Random.Shared.NextDouble();
            var newBool = Random.Shared.Next( 2 ) == 0;
            var newTime = (float)Random.Shared.NextDouble();
            var newString = Helpers.RandomStringGenerator.RandomAlphaNumericString( 10 );

            copyOfTestAttributeValue.SetFieldValue( "AString", newString );
            copyOfTestAttributeValue.SetFieldValue( "AnInteger", newInt );
            copyOfTestAttributeValue.SetFieldValue( "AFloat", newFloat );
            copyOfTestAttributeValue.SetFieldValue( "ABoolean", newBool );
            copyOfTestAttributeValue.SetFieldValue( "ADate", newDate );
            copyOfTestAttributeValue.SetFieldValue( "ATime", newTime );

            //Test that the copy has the new values
            Assert.AreEqual( newString, (string)copyOfTestAttributeValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( newInt, (int)copyOfTestAttributeValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( newFloat, (float)copyOfTestAttributeValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( newBool, (bool)copyOfTestAttributeValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( newDate, (DateTime)copyOfTestAttributeValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( newTime, (float)copyOfTestAttributeValue.GetFieldValue( "ATime" ) );

            //Test tha the original still has the old values
            Assert.AreEqual( defaultString, (string)testAttributeValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( defaultInt, (int)testAttributeValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( defaultFloat, (float)testAttributeValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( defaultBool, (bool)testAttributeValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( defaultDate, (DateTime)testAttributeValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( defaultTime, (float)testAttributeValue.GetFieldValue( "ATime" ) );
        }
    }
}
