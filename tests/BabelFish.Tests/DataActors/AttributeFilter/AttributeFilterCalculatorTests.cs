using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.DataActors.AttributeFilter {

    [TestClass]
    public class AttributeFilterCalculatorTests : BaseTestClass {

        [TestMethod]
        public async Task SingleAttributeValue() {
            Participant participant = new Individual();
            SetName setName = SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" );
            var attrValue = await DataModel.AttributeValue.AttributeValue.CreateAsync( setName );
            attrValue.SetFieldValue( "Three-Position Air Rifle Type", "Sporter" );
            participant.AttributeValues.Add( new AttributeValueDataPacketMatch() {
                AttributeDef = setName.ToString(),
                AttributeValue = attrValue
            } );

            AttributeFilterAttributeValue sporterFilterHasOne = new AttributeFilterAttributeValue();
            sporterFilterHasOne.AttributeDef = setName;
            sporterFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            sporterFilterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasOne = new AttributeFilterAttributeValue();
            precisionFilterHasOne.AttributeDef = setName;
            precisionFilterHasOne.FilterRule = AttributeFilterRule.HAVE_ONE;
            precisionFilterHasOne.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterAttributeValue sporterFilterHasAll = new AttributeFilterAttributeValue();
            sporterFilterHasAll.AttributeDef = setName;
            sporterFilterHasAll.FilterRule = AttributeFilterRule.HAVE_ALL;
            sporterFilterHasAll.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasAll = new AttributeFilterAttributeValue();
            precisionFilterHasAll.AttributeDef = setName;
            precisionFilterHasAll.FilterRule = AttributeFilterRule.HAVE_ALL;
            precisionFilterHasAll.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterAttributeValue sporterFilterHasNone = new AttributeFilterAttributeValue();
            sporterFilterHasNone.AttributeDef = setName;
            sporterFilterHasNone.FilterRule = AttributeFilterRule.NOT_HAVE_ANY;
            sporterFilterHasNone.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Sporter" ) );

            AttributeFilterAttributeValue precisionFilterHasNone = new AttributeFilterAttributeValue();
            precisionFilterHasNone.AttributeDef = setName;
            precisionFilterHasNone.FilterRule = AttributeFilterRule.NOT_HAVE_ANY;
            precisionFilterHasNone.Values.Add( new Tuple<string, dynamic>( "Three-Position Air Rifle Type", "Precision" ) );

            AttributeFilterCalculator calculator = new AttributeFilterCalculator();

            Assert.IsTrue( calculator.Passes( sporterFilterHasOne, participant ) );
            Assert.IsFalse( calculator.Passes( precisionFilterHasOne, participant ) );
            Assert.IsTrue( calculator.Passes( sporterFilterHasAll, participant ) );
            Assert.IsFalse( calculator.Passes( precisionFilterHasAll, participant ) );
            Assert.IsFalse( calculator.Passes( sporterFilterHasNone, participant ) );
            Assert.IsTrue( calculator.Passes( precisionFilterHasNone, participant ) );
        }
    }
}
