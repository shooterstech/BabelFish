using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.AttributeValue {

    /// <summary>
    /// Series of unit tests that test if new attribute values have expected default values
    /// </summary>
    [TestClass]
    public class DefaultAttributeValues {
        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

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
            var today = DateTime.Today;
            var now = DateTime.UtcNow;
            var zero = TimeSpan.Zero;

            Assert.AreEqual( "", (string) testAttributeValue.GetFieldValue( "AString" ) );
            Assert.AreEqual( 0, (int)testAttributeValue.GetFieldValue( "AnInteger" ) );
            Assert.AreEqual( 0, (float) testAttributeValue.GetFieldValue( "AFloat" ) );
            Assert.AreEqual( false, (bool)testAttributeValue.GetFieldValue( "ABoolean" ) );
            Assert.AreEqual( today, (DateTime) testAttributeValue.GetFieldValue( "ADate" ) );
            Assert.AreEqual( zero, (TimeSpan)testAttributeValue.GetFieldValue( "ATime" ) );
            Assert.IsTrue( Math.Abs( (((DateTime)testAttributeValue.GetFieldValue( "ADateTime" )).ToUniversalTime() - now).TotalSeconds ) < .1D );

            Assert.IsTrue( ((List<string>)testAttributeValue.GetFieldValue( "AListOfStrings" )).Count == 0 );
            Assert.IsTrue( ((List<int>)testAttributeValue.GetFieldValue( "AListOfIntegers" )).Count == 0 );
            Assert.IsTrue( ((List<float>)testAttributeValue.GetFieldValue( "AListOfFloats" )).Count == 0 );
            Assert.IsTrue( ((List<DateTime>)testAttributeValue.GetFieldValue( "AListOfDates" )).Count == 0 );
            Assert.IsTrue( ((List<DateTime>)testAttributeValue.GetFieldValue( "AListOfDateTimes" )).Count == 0 );
            Assert.IsTrue( ((List<TimeSpan>)testAttributeValue.GetFieldValue( "AListOfTimes" )).Count == 0 );
        }

        /// <summary>
        /// Note really a test, more of an example of how to get default value and test values against validation.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [ExpectedException( typeof( AttributeValueValidationException ) )]
        public async Task FieldsTest() {

            //The Test Attribute defines an attribute meant for testing. Do you like the name I gave it? I came up with it myself.
            var setNameTestAttriubte = SetName.Parse( "v1.0:orion:Test Attribute" );

            var testAttributeValue = await Scopos.BabelFish.DataModel.AttributeValue.AttributeValue.CreateAsync( setNameTestAttriubte );

            //Loop through a list of the fields
            foreach( var field in testAttributeValue.GetDefintionFields() ) {
                var fieldName = field.FieldName;  //Name of the field
                var defaultValue = field.DefaultValue; //Note the return type is dynamic so you have to know what to cast it to
                var feidlType = field.FieldType; //And this is how you learn what type of field it is. 
                var validation = field.Validation; //This is the validation rule. Note, not fully implemented yet.
                var passesValidation = field.ValidateFieldValue( "input value" ); //returns true or false if the input value passes the validation test.
                testAttributeValue.SetFieldValue( field.FieldName, "input value" ); //Will throw a AttributeValueValidationException if the input does not pass validation.
            }
        }
    }
}
