using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.AttributeValue {

    /// <summary>
    /// The methods in this class largely test the AttributeField class, and if data types are stored and returned in their expected format.
    /// </summary>
    [TestClass]
    public class SettingFieldValueTests : BaseTestClass {

        [TestMethod]
        public void HappyPathSingleAttributeFieldDataTypes() {

            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );
            var testAttrValue = Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte ).Result;

            //Create some random data to store
            var random = new Random();
            var myInt = random.Next();
            var myFloat = (float)random.NextDouble();
            var myBool = random.NextInt64() % 2 == 0;
            var myDate = (new DateTime( random.NextInt64( 0, DateTime.MaxValue.Ticks ) )).Date;
            var myDateTime = new DateTime( random.NextInt64( 0, DateTime.MaxValue.Ticks ) );
            var myTime = (float)random.NextDouble();
            var myString = Scopos.BabelFish.Helpers.RandomStringGenerator.RandomAlphaString( 8 );

            //Set values to the attribute value.
            testAttrValue.SetFieldValue( "AString", myString );
            testAttrValue.SetFieldValue( "AnInteger", myInt );
            testAttrValue.SetFieldValue( "AFloat", myFloat );
            testAttrValue.SetFieldValue( "ABoolean", myBool );
            testAttrValue.SetFieldValue( "ADate", myDate );
            testAttrValue.SetFieldValue( "ATime", myTime );
            //testAttrValue.SetFieldValue( "ADateTime", myDateTime );

            //Now test that the GetFieldValue return the same data that we stored
            Assert.AreEqual( myString, (string)testAttrValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( myInt, (int)testAttrValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( myFloat, (float)testAttrValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( myBool, (bool)testAttrValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( myDate, (DateTime)testAttrValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( myTime, (float)testAttrValue.GetFieldValue( "ATime" ) );
            //Because Times and DateTime are stored with known rounding error, will allow this much tolerance in teh comparison.
            //Assert.IsTrue( Math.Abs( ((DateTime)testAttrValue.GetFieldValue( "ADateTime" ) - myDateTime).TotalMilliseconds ) < .001D );
        }

        [TestMethod]
        public void HappyPathListAttributeFieldDataTypes() {

            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );
            var testAttrValue = Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte ).Result;

            //Create some random data to store
            List<string> myListOfStrings = new List<string>();
            for (int i = 0; i < 10; i++) {
                myListOfStrings.Add( Scopos.BabelFish.Helpers.RandomStringGenerator.RandomAlphaString( 8 ) );
            }

            //Set values to the attribute value.
            testAttrValue.SetFieldValue( "AListOfStrings", myListOfStrings );

            //Now test that the GetFieldValue return the same data that we stored
            var myReturnedListOfStrings = (List<string>)testAttrValue.GetFieldValue( "AListOfStrings" );

            for (int i = 0; i < 10; i++) {
                Assert.AreEqual( myListOfStrings[i], myReturnedListOfStrings[i] );
            }
        }

        /// <summary>
        /// Tries an stores an integer to a string field. Should throw an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueValidationException ) )]
        public void WrongDataTypeForString() {

            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );
            var testAttrValue = Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte ).Result;

            testAttrValue.SetFieldValue( "AString", 1234 );
        }

        /// <summary>
        /// Tries an stores an double to an intgeer field. Should throw an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueValidationException ) )]
        public void WrongDataTypeForInt() {

            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );
            var testAttrValue = Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte ).Result;

            testAttrValue.SetFieldValue( "AnInteger", 1234.5678 );
        }

        /// <summary>
        /// Tries an stores an field value, with a field key, to a Attribute that does not have multiple values.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueException ) )]
        public void WrongUseOfSetFieldValue1() {

            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );
            var testAttrValue = Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte ).Result;

            testAttrValue.SetFieldValue( "AString", 1234, "MyFieldKey" );
        }

        [TestMethod]
        public async Task EriksPlayground() {
            var setName = SetName.Parse( "v1.0:nra:Age Categories" );
            var attrValue = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            attrValue.SetFieldValue( "Age Category", "Sub Junior" );
        }
    }
}
